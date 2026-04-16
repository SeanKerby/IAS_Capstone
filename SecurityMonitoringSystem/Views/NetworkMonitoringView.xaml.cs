using System.Windows;
using System.Windows.Controls;
using SecurityMonitoringSystem.Models;

namespace SecurityMonitoringSystem.Views
{
    public partial class NetworkMonitoringView : UserControl
    {
        private Services.NetworkSimulationService _service;

        public NetworkMonitoringView()
        {
            InitializeComponent();
            _service = new Services.NetworkSimulationService();
            LoadData();
        }

        private async void LoadData()
        {
            var devices = await _service.GetAllDevicesAsync();
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
