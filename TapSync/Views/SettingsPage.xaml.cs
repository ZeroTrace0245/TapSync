using System;
using System.IO;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.Windows.Storage.Pickers;
using TapSync.Services;

namespace TapSync.Views
{
    public sealed partial class SettingsPage : Page
    {
        private readonly AppSettingsService _settings = AppSettingsService.Instance;

        // True while controls are being filled from saved settings, so their change events don't save.
        private bool _loading = true;

        public SettingsPage()
        {
            InitializeComponent();
            VersionText.Text = $"Version {typeof(App).Assembly.GetName().Version?.ToString(3) ?? "1.0.0"}";
            Loaded += (_, _) => LoadFromSettings();
        }

        private void LoadFromSettings()
        {
            _loading = true;
            var s = _settings.Current;

            SelectByTag(ThemeComboBox, s.Theme.ToString());
            SelectByTag(BackgroundModeComboBox, s.BackgroundMode.ToString());
            BlurToggle.IsOn = s.BlurEnabled;
            TintSlider.Value = Math.Round(s.OverlayOpacity * 100);
            TintValueText.Text = $"{TintSlider.Value:0}%";

            UpdatePictureOptions();
            _ = RefreshPreviewAsync();
            _loading = false;
        }

        private static void SelectByTag(ComboBox comboBox, string tag)
        {
            comboBox.SelectedItem = comboBox.Items
                .OfType<ComboBoxItem>()
                .FirstOrDefault(i => (string)i.Tag == tag) ?? comboBox.Items.FirstOrDefault();
        }

        private static string? SelectedTag(ComboBox comboBox) => (comboBox.SelectedItem as ComboBoxItem)?.Tag as string;

        private void UpdatePictureOptions()
        {
            PictureOptionsPanel.Visibility = _settings.Current.BackgroundMode == BackgroundMode.Image
                ? Visibility.Visible
                : Visibility.Collapsed;

            bool hasImage = _settings.HasUsableImage;
            RemoveImageButton.IsEnabled = hasImage;
            BlurToggle.IsEnabled = hasImage;
            TintSlider.IsEnabled = hasImage;
            ImageNameText.Text = hasImage
                ? Path.GetFileName(_settings.Current.BackgroundImagePath)
                : "No picture selected. Choose one to use it as the background.";
        }

        private async System.Threading.Tasks.Task RefreshPreviewAsync()
        {
            var bitmap = _settings.HasUsableImage
                ? await ImageLoader.LoadAsync(_settings.Current.BackgroundImagePath, decodePixelWidth: 320)
                : null;

            PreviewImage.Source = bitmap;
            PreviewPlaceholderIcon.Visibility = bitmap == null ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_loading || !Enum.TryParse(SelectedTag(ThemeComboBox), out AppTheme theme)) return;
            _settings.Update(s => s.Theme = theme);
        }

        private void BackgroundModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_loading || !Enum.TryParse(SelectedTag(BackgroundModeComboBox), out BackgroundMode mode)) return;
            _settings.Update(s => s.BackgroundMode = mode);
            UpdatePictureOptions();
        }

        private async void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorInfoBar.IsOpen = false;

            try
            {
                var picker = new FileOpenPicker(XamlRoot.ContentIslandEnvironment.AppWindowId)
                {
                    SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                    ViewMode = PickerViewMode.Thumbnail,
                    CommitButtonText = "Use as background"
                };
                foreach (var type in AppSettingsService.SupportedImageTypes)
                {
                    picker.FileTypeFilter.Add(type);
                }

                var result = await picker.PickSingleFileAsync();
                if (result == null) return; // cancelled

                // Make sure it is really a picture before saving it.
                if (await ImageLoader.LoadAsync(result.Path, decodePixelWidth: 64) == null)
                {
                    ShowError("That file couldn't be opened as a picture. Try a JPG or PNG.");
                    return;
                }

                var copy = _settings.ImportBackgroundImage(result.Path);
                _settings.Update(s =>
                {
                    s.BackgroundImagePath = copy;
                    s.BackgroundMode = BackgroundMode.Image;
                });

                UpdatePictureOptions();
                await RefreshPreviewAsync();
            }
            catch (Exception ex)
            {
                ShowError($"Couldn't set the background picture: {ex.Message}");
            }
        }

        private async void RemoveImageButton_Click(object sender, RoutedEventArgs e)
        {
            _settings.RemoveBackgroundImage();
            LoadFromSettings();
            await RefreshPreviewAsync();
        }

        private void BlurToggle_Toggled(object sender, RoutedEventArgs e)
        {
            if (_loading) return;
            _settings.Update(s => s.BlurEnabled = BlurToggle.IsOn);
        }

        private void TintSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (TintValueText != null)
            {
                TintValueText.Text = $"{e.NewValue:0}%";
            }

            if (_loading) return;
            _settings.Update(s => s.OverlayOpacity = e.NewValue / 100.0);
        }

        private void ShowError(string message)
        {
            ErrorInfoBar.Message = message;
            ErrorInfoBar.IsOpen = true;
        }
    }
}
