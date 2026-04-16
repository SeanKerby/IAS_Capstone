using System.Windows.Controls;

namespace SecurityMonitoringSystem.Views
{
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();
            LoadDashboardData();
        }

        private async void LoadDashboardData()
        {
            try 
            {
                var netService = new Services.NetworkSimulationService();
                var threatService = new Services.ThreatDetectionService();
                var accessService = new Services.AccessControlService();

                var devices = await netService.GetAllDevicesAsync();
                int activeCount = 0;
                foreach (var d in devices) 
                {
                    if (d.Status != "Offline" && d.UplinkSpeed != "Offline") 
                        activeCount++;
                }

                var threats = await threatService.GetAlertsAsync();
                var users = await accessService.GetAllUsersAsync();

                Dispatcher.Invoke(() => {
                    txtActiveConnections.Text = activeCount.ToString();
                    txtDetectedThreats.Text = threats.Count.ToString();
                    txtTotalUsers.Text = users.Count.ToString();
                });
            }
            catch { }
        }
    }
}
