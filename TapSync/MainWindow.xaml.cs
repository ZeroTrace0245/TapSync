using System;
using System.Collections.Generic;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using TapSync.Services;
using TapSync.Views;
using Windows.UI;

namespace TapSync
{
    public sealed partial class MainWindow : Window
    {
        private static readonly Dictionary<string, Type> Pages = new()
        {
            ["Dashboard"] = typeof(DashboardPage),
            ["Map"] = typeof(MapPage),
            ["EventAdmin"] = typeof(EventAdminPage),
            ["Registration"] = typeof(RegistrationPage),
            ["NFCCredentials"] = typeof(NFCCredentialsPage),
            ["AccessRules"] = typeof(AccessRulesPage),
            ["RollCall"] = typeof(RollCallPage),
        };

        private readonly AppSettingsService _settings = AppSettingsService.Instance;
        private string? _loadedImagePath;
        private int _applyVersion;

        public MainWindow()
        {
            InitializeComponent();

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
            AppWindow.SetIcon("AppIcon.ico");
            SizeAndCenterWindow(1280, 840);

            _settings.SettingsChanged += Settings_Changed;
            RootGrid.ActualThemeChanged += (_, _) => ApplyOverlay();
            Closed += (_, _) => _settings.SettingsChanged -= Settings_Changed;

            ApplySettings();

            // Selecting the first item raises SelectionChanged, which performs the first navigation.
            NavView.SelectedItem = NavView.MenuItems[0];
        }

        /// <summary>Opens at the preferred size, shrunk to fit the screen's work area, and centred.</summary>
        private void SizeAndCenterWindow(int preferredWidth, int preferredHeight)
        {
            var workArea = DisplayArea.GetFromWindowId(AppWindow.Id, DisplayAreaFallback.Nearest).WorkArea;
            double scale = (Content?.XamlRoot?.RasterizationScale) ?? GetDpiScale();

            int width = Math.Min((int)(preferredWidth * scale), (int)(workArea.Width * 0.92));
            int height = Math.Min((int)(preferredHeight * scale), (int)(workArea.Height * 0.92));

            AppWindow.MoveAndResize(new Windows.Graphics.RectInt32(
                workArea.X + (workArea.Width - width) / 2,
                workArea.Y + (workArea.Height - height) / 2,
                width,
                height));
        }

        private double GetDpiScale()
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            return GetDpiForWindow(hwnd) / 96.0;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern uint GetDpiForWindow(IntPtr hwnd);

        private void AppTitleBar_PaneToggleRequested(TitleBar sender, object args)
        {
            NavView.IsPaneOpen = !NavView.IsPaneOpen;
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Type? pageType = args.IsSettingsSelected
                ? typeof(SettingsPage)
                : args.SelectedItemContainer?.Tag is string tag && Pages.TryGetValue(tag, out var type) ? type : null;

            if (pageType != null && ContentFrame.CurrentSourcePageType != pageType)
            {
                ContentFrame.Navigate(pageType, null, new EntranceNavigationTransitionInfo());
            }
        }

        private void Settings_Changed(object? sender, EventArgs e) => ApplySettings();

        private async void ApplySettings()
        {
            var version = ++_applyVersion;
            var s = _settings.Current;

            // Theme
            RootGrid.RequestedTheme = s.Theme switch
            {
                AppTheme.Light => ElementTheme.Light,
                AppTheme.Dark => ElementTheme.Dark,
                _ => ElementTheme.Default
            };
            AppWindow.TitleBar.PreferredTheme = s.Theme switch
            {
                AppTheme.Light => TitleBarTheme.Light,
                AppTheme.Dark => TitleBarTheme.Dark,
                _ => TitleBarTheme.UseDefaultAppMode
            };

            // Window material: Acrylic blurs the desktop behind the window, Mica tints with the wallpaper.
            if (s.BackgroundMode == BackgroundMode.Acrylic)
            {
                if (SystemBackdrop is not DesktopAcrylicBackdrop) SystemBackdrop = new DesktopAcrylicBackdrop();
            }
            else if (SystemBackdrop is not MicaBackdrop)
            {
                SystemBackdrop = new MicaBackdrop { Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.Base };
            }

            // Picture
            if (s.BackgroundMode == BackgroundMode.Image && _settings.HasUsableImage)
            {
                if (!string.Equals(_loadedImagePath, s.BackgroundImagePath, StringComparison.OrdinalIgnoreCase))
                {
                    var bitmap = await ImageLoader.LoadAsync(s.BackgroundImagePath);
                    if (version != _applyVersion) return; // a newer change arrived while loading

                    BackgroundImage.Source = bitmap;
                    _loadedImagePath = bitmap != null ? s.BackgroundImagePath : null;
                }
            }
            else
            {
                BackgroundImage.Source = null;
                _loadedImagePath = null;
            }

            ApplyOverlay();
        }

        private void ApplyOverlay()
        {
            var s = _settings.Current;
            bool showPicture = s.BackgroundMode == BackgroundMode.Image && BackgroundImage.Source != null;

            BackgroundImage.Visibility = showPicture ? Visibility.Visible : Visibility.Collapsed;
            BackdropOverlay.Visibility = showPicture ? Visibility.Visible : Visibility.Collapsed;
            if (!showPicture) return;

            // Tint towards the theme's base colour so text on top stays readable.
            Color tint = RootGrid.ActualTheme == ElementTheme.Dark
                ? Color.FromArgb(255, 32, 32, 32)
                : Color.FromArgb(255, 243, 243, 243);

            BackdropOverlay.Background = s.BlurEnabled
                ? new AcrylicBrush
                {
                    TintColor = tint,
                    TintOpacity = s.OverlayOpacity,
                    TintLuminosityOpacity = Math.Max(0.1, s.OverlayOpacity),
                    FallbackColor = Color.FromArgb(230, tint.R, tint.G, tint.B)
                }
                : new SolidColorBrush(tint) { Opacity = s.OverlayOpacity };
        }
    }
}
