using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TapSync.Views;

namespace TapSync
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
            AppWindow.SetIcon("AppIcon.ico");

            ContentFrame.Navigate(typeof(DashboardPage));
            NavView.SelectedItem = NavView.MenuItems[0];
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer is NavigationViewItem item)
            {
                string tag = (string)item.Tag;

                switch (tag)
                {
                    case "Dashboard":
                        ContentFrame.Navigate(typeof(DashboardPage));
                        break;
                    case "EventAdmin":
                        ContentFrame.Navigate(typeof(EventAdminPage));
                        break;
                    case "Registration":
                        ContentFrame.Navigate(typeof(RegistrationPage));
                        break;
                    case "NFCCredentials":
                        ContentFrame.Navigate(typeof(NFCCredentialsPage));
                        break;
                    case "AccessRules":
                        ContentFrame.Navigate(typeof(AccessRulesPage));
                        break;
                    case "RollCall":
                        ContentFrame.Navigate(typeof(RollCallPage));
                        break;
                }
            }
        }
    }
}

