using System.Windows;
using System.Windows.Controls;
using SecurityMonitoringSystem.Models;

namespace SecurityMonitoringSystem.Views
{
    public partial class ThreatDetectionView : UserControl
    {
        public ThreatDetectionView()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            ThreatsDataGrid.ItemsSource = SampleData.GetThreats();
        }

        private void Action_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Command dispatched. Network segment isolated.", "System Action", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
