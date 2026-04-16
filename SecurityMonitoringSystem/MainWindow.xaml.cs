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
    }
}
