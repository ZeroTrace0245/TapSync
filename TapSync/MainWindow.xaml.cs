using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using TapSync.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

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

            // Load dashboard on startup
            ContentFrame.Navigate(typeof(DashboardPage));
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string tag = button.Name;

                if (tag == "DashboardButton")
                    ContentFrame.Navigate(typeof(DashboardPage));
                else if (tag == "AttendeesButton")
                    ContentFrame.Navigate(typeof(AttendeesPage));
                else if (tag == "AccessButton")
                    ContentFrame.Navigate(typeof(AccessPage));
            }
        }
    }
}

