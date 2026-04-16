using System.Windows;
using System.Windows.Controls;
using SecurityMonitoringSystem.Models;

namespace SecurityMonitoringSystem.Views
{
    public partial class ThreatDetectionView : UserControl
    {
        private Services.ThreatDetectionService _service;

        public ThreatDetectionView()
        {
            InitializeComponent();
            _service = new Services.ThreatDetectionService();
            LoadData();

            Services.ThreatDetectionService.OnThreatDetected += (desc, lvl) => {
                Dispatcher.Invoke(() => {
                    LoadData();
                });
            };
        }

        private async void LoadData()
        {
            ThreatsDataGrid.ItemsSource = null;
            ThreatsDataGrid.ItemsSource = await _service.GetAlertsAsync();
        }

        private async void Isolate_Click(object sender, RoutedEventArgs e)
        {
            try {
                await Services.ActivityLogService.LogActionAsync("Admin", "Threat Detection", "Admin dispatched command to isolate segment.");
                MessageBox.Show("Command dispatched. Network segment isolated.", "System Action", MessageBoxButton.OK, MessageBoxImage.Warning);
            } catch { }
        }

        private async void Counter_Click(object sender, RoutedEventArgs e)
        {
            try {
                await Services.ActivityLogService.LogActionAsync("Admin", "Threat Detection", "Admin initialized AI-driven countermeasures.");
                MessageBox.Show("Countermeasures active. Securing perimeter.", "System Action", MessageBoxButton.OK, MessageBoxImage.Information);
            } catch { }
        }
    }
}
