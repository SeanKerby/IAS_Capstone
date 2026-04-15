using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SecurityMonitoringSystem.Views
{
    public partial class EncryptionView : UserControl
    {
        public EncryptionView()
        {
            InitializeComponent();
        }

        private void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInput.Text)) return;
            try
            {
                var bytes = Encoding.UTF8.GetBytes(txtInput.Text);
                txtOutput.Text = Convert.ToBase64String(bytes);
                txtInput.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Encryption Error: {0}", ex.Message));
            }
        }

        private void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOutput.Text)) return;
            try
            {
                var bytes = Convert.FromBase64String(txtOutput.Text);
                txtInput.Text = Encoding.UTF8.GetString(bytes);
                txtOutput.Clear();
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid Cipher Text to decrypt.");
            }
        }
    }
}
