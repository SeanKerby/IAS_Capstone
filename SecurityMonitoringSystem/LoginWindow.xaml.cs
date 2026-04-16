using System.Windows;

namespace SecurityMonitoringSystem
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            try {
                Services.DatabaseInitializer.Initialize();
                Services.NetworkSimulationService.StartSimulation();
            } catch { }
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            var service = new Services.AccessControlService();
            string role = await service.AuthenticateAsync(txtUsername.Text, pwdBox.Password);

            if (!string.IsNullOrEmpty(role))
            {
                MainWindow main = new MainWindow();
                main.Show();
                this.Close(); // Close the login window safely
            }
            else
            {
                MessageBox.Show("Invalid Username or Password", "Authentication Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
