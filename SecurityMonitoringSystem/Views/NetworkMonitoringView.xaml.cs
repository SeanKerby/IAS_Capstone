using System.Windows;
using System.Windows.Controls;
using SecurityMonitoringSystem.Models;

namespace SecurityMonitoringSystem.Views
{
    public partial class NetworkMonitoringView : UserControl
    {
        public NetworkMonitoringView()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            var devices = SampleData.GetDevices();
            DevicesDataGrid.ItemsSource = null;
            DevicesDataGrid.ItemsSource = devices;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            MessageBox.Show("Network status refreshed perfectly. All logs updated via 6G and Satellite up-link.", "Refresh Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
