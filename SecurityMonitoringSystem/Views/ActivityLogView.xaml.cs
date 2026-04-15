using System.Windows;
using System.Windows.Controls;
using SecurityMonitoringSystem.Models;

namespace SecurityMonitoringSystem.Views
{
    public partial class ActivityLogView : UserControl
    {
        public ActivityLogView()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            LogsDataGrid.ItemsSource = SampleData.GetLogs();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Audit logs successfully exported to CSV.", "Export Complete", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
