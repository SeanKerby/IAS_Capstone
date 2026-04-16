using System.Windows;

namespace SecurityMonitoringSystem
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            txtHeaderRole.Text = $"Role: {Services.SessionProvider.CurrentRole} | User: {Services.SessionProvider.CurrentUsername}";
            
            if (!Services.SessionProvider.IsAdmin)
            {
                btnAccessControl.Visibility = Visibility.Collapsed;
                btnThreatDetection.Visibility = Visibility.Collapsed;
            }
        }

        private async void Logout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await Services.ActivityLogService.LogActionAsync(Services.SessionProvider.CurrentUsername, "Access Control", "User logged out.");
                Services.SessionProvider.CurrentUsername = string.Empty;
                Services.SessionProvider.CurrentRole = string.Empty;

                LoginWindow login = new LoginWindow();
                login.Show();
                this.Close();
            }
        }
    }
}
