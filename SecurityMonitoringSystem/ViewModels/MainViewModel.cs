using System.Windows.Controls;

namespace SecurityMonitoringSystem.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private UserControl _currentView;

        public UserControl CurrentView
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value; 
                OnPropertyChanged(); 
            }
        }

        public RelayCommand NavigateDashboardCmd { get; set; }
        public RelayCommand NavigateNetworkCmd { get; set; }
        public RelayCommand NavigateAccessCmd { get; set; }
        public RelayCommand NavigateEncryptionCmd { get; set; }
        public RelayCommand NavigateLogsCmd { get; set; }
        public RelayCommand NavigateThreatsCmd { get; set; }

        public MainViewModel()
        {
            NavigateDashboardCmd = new RelayCommand(o => CurrentView = new Views.DashboardView());
            NavigateNetworkCmd = new RelayCommand(o => CurrentView = new Views.NetworkMonitoringView());
            NavigateAccessCmd = new RelayCommand(o => CurrentView = new Views.AccessControlView());
            NavigateEncryptionCmd = new RelayCommand(o => CurrentView = new Views.EncryptionView());
            NavigateLogsCmd = new RelayCommand(o => CurrentView = new Views.ActivityLogView());
            NavigateThreatsCmd = new RelayCommand(o => CurrentView = new Views.ThreatDetectionView());

            // Default startup view
            CurrentView = new Views.DashboardView();
        }
    }
}
