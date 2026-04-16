using System.Windows;
using System.Windows.Controls;
using SecurityMonitoringSystem.Models;

namespace SecurityMonitoringSystem.Views
{
    public partial class ActivityLogView : UserControl
    {
        private Services.ActivityLogService _service;

        public ActivityLogView()
        {
            InitializeComponent();
            _service = new Services.ActivityLogService();
            LoadData();
        }

        private async void LoadData(string search = "", string module = "All Modules")
        {
            LogsDataGrid.ItemsSource = null;
            LogsDataGrid.ItemsSource = await _service.GetLogsAsync(search, module);
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (LogsDataGrid.ItemsSource is System.Collections.Generic.IEnumerable<SystemLog> logs)
                {
                    var lines = new System.Collections.Generic.List<string> { "Timestamp,User,Module,Action" };
                    foreach(var log in logs) {
                        lines.Add($"{log.Timestamp},{log.User},{log.Module},\"{log.Action.Replace("\"", "\"\"")}\"");
                    }
                    System.IO.File.WriteAllLines("AuditLogs.csv", lines);
                    MessageBox.Show("Audit logs successfully exported to CSV (AuditLogs.csv).", "Export Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            } catch (System.Exception ex) {
                 MessageBox.Show("Export failed: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (cmbModule.SelectedItem is ComboBoxItem item)
            {
                LoadData(txtSearch.Text, item.Content.ToString());
            }
        }
    }
}
