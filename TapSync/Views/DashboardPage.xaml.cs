using Microsoft.UI.Xaml.Controls;
using TapSync.Services;

namespace TapSync.Views
{
    public sealed partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            this.InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            var mockData = MockDataService.GetInstance();
            var metrics = mockData.GetDashboardMetrics();

            CheckInProgressText.Text = $"{metrics.CheckedInCount}/{metrics.TotalAttendees}";
            VIPCapacityBar.Value = metrics.VIPCapacityPercentage;
            VIPCapacityText.Text = $"{metrics.VIPCapacityPercentage}%";
            GeneralCapacityBar.Value = metrics.GeneralCapacityPercentage;
            GeneralCapacityText.Text = $"{metrics.GeneralCapacityPercentage}%";
            StaffCapacityBar.Value = metrics.StaffCapacityPercentage;
            StaffCapacityText.Text = $"{metrics.StaffCapacityPercentage}%";
        }
    }
}
