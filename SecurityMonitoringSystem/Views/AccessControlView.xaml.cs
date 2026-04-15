using System.Windows;
using System.Windows.Controls;
using SecurityMonitoringSystem.Models;

namespace SecurityMonitoringSystem.Views
{
    public partial class AccessControlView : UserControl
    {
        public AccessControlView()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            UsersDataGrid.ItemsSource = SampleData.GetUsers();
        }

        private void Action_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("User action completed successfully. Demonstration only.", "Access Control", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
