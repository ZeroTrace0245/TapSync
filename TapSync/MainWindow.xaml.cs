using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TapSync.Views;

namespace TapSync
{
    public sealed partial class MainWindow : Window
    {
        private Frame _contentFrame;
        private NavigationView _navView;

        public MainWindow()
        {
            // Initialize the basic window
            this.Content = new Grid();
            ExtendsContentIntoTitleBar = true;
            AppWindow.SetIcon("AppIcon.ico");

            // Create UI manually
            SetupUI();
        }

        private void SetupUI()
        {
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Title Bar
            var titleBar = new TitleBar();
            grid.Children.Add(titleBar);

            // Navigation View
            _navView = new NavigationView
            {
                IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed,
                IsPaneOpen = true,
                PaneDisplayMode = NavigationViewPaneDisplayMode.Left
            };

            _navView.MenuItems.Add(new NavigationViewItem 
            { 
                Icon = new SymbolIcon(Symbol.Home), 
                Content = "Dashboard", 
                Tag = "Dashboard" 
            });
            _navView.MenuItems.Add(new NavigationViewItem 
            { 
                Icon = new SymbolIcon(Symbol.MapPin), 
                Content = "Live Heatmap", 
                Tag = "Map" 
            });
            _navView.MenuItems.Add(new NavigationViewItem 
            { 
                Icon = new SymbolIcon(Symbol.Admin), 
                Content = "Event Administration", 
                Tag = "EventAdmin" 
            });
            _navView.MenuItems.Add(new NavigationViewItem 
            { 
                Icon = new SymbolIcon(Symbol.People), 
                Content = "Registration", 
                Tag = "Registration" 
            });
            _navView.MenuItems.Add(new NavigationViewItem 
            { 
                Icon = new SymbolIcon(Symbol.ContactInfo), 
                Content = "NFC Credentials", 
                Tag = "NFCCredentials" 
            });
            _navView.MenuItems.Add(new NavigationViewItem 
            { 
                Icon = new SymbolIcon(Symbol.BrowsePhotos), 
                Content = "Access Rules", 
                Tag = "AccessRules" 
            });
            _navView.MenuItems.Add(new NavigationViewItem 
            { 
                Icon = new SymbolIcon(Symbol.Emoji), 
                Content = "Emergency Roll Call", 
                Tag = "RollCall" 
            });

            // Content Frame
            _contentFrame = new Frame();
            _navView.Content = _contentFrame;

            Grid.SetRow(_navView, 1);
            grid.Children.Add(_navView);

            this.Content = grid;
            SetTitleBar(titleBar);

            _navView.SelectionChanged += NavView_SelectionChanged;
            _contentFrame.Navigate(typeof(DashboardPage));
            _navView.SelectedItem = _navView.MenuItems[0];
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer is NavigationViewItem item)
            {
                string tag = (string)item.Tag;

                switch (tag)
                {
                    case "Dashboard":
                        _contentFrame.Navigate(typeof(DashboardPage));
                        break;
                    case "Map":
                        _contentFrame.Navigate(typeof(MapPage));
                        break;
                    case "EventAdmin":
                        _contentFrame.Navigate(typeof(EventAdminPage));
                        break;
                    case "Registration":
                        _contentFrame.Navigate(typeof(RegistrationPage));
                        break;
                    case "NFCCredentials":
                        _contentFrame.Navigate(typeof(NFCCredentialsPage));
                        break;
                    case "AccessRules":
                        _contentFrame.Navigate(typeof(AccessRulesPage));
                        break;
                    case "RollCall":
                        _contentFrame.Navigate(typeof(RollCallPage));
                        break;
                }
            }
        }
    }
}

