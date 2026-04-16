using System.Windows;
using System.Windows.Controls;
using SecurityMonitoringSystem.Models;

namespace SecurityMonitoringSystem.Views
{
    public partial class AccessControlView : UserControl
    {
        private Services.AccessControlService _service;

        public AccessControlView()
        {
            InitializeComponent();
            _service = new Services.AccessControlService();
            LoadData();
        }

        private async void LoadData()
        {
            UsersDataGrid.ItemsSource = null;
            UsersDataGrid.ItemsSource = await _service.GetAllUsersAsync();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = pwdBox.Password;
            string role = (cmbRole.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "User";

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Username and Password are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool success = await _service.AddUserAsync(username, password, role);
            if (success)
            {
                MessageBox.Show("User successfully added.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                txtUsername.Clear();
                pwdBox.Clear();
                LoadData();
            }
            else
            {
                MessageBox.Show("Failed to add user. Username might already exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            Models.UserAccount selectedUser = UsersDataGrid.SelectedItem as Models.UserAccount;
            string targetUser = selectedUser?.Username ?? txtUsername.Text;

            if (string.IsNullOrWhiteSpace(targetUser))
            {
                MessageBox.Show("Select a user from the list or type a username to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Are you sure you want to delete {targetUser}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                bool success = await _service.DeleteUserAsync(targetUser);
                if (success)
                {
                    MessageBox.Show("User deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtUsername.Clear();
                    pwdBox.Clear();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Failed to delete user. The user might not exist or is protected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
