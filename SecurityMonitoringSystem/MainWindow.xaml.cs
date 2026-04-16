using System.Windows;

namespace SecurityMonitoringSystem
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try {
                Services.DatabaseInitializer.Initialize();
                Services.NetworkSimulationService.StartSimulation();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("DB Init Error: " + ex.Message);
            }
        }
    }
}
