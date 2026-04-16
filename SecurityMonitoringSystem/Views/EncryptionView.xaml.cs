using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SecurityMonitoringSystem.Views
{
    public partial class EncryptionView : UserControl
    {
        private Services.EncryptionService _service;

        public EncryptionView()
        {
            InitializeComponent();
            _service = new Services.EncryptionService();
        }

        private async void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInput.Text)) return;
            try
            {
                txtOutput.Text = _service.Encrypt(txtInput.Text);
                await Services.ActivityLogService.LogActionAsync("User", "Encryption", "Data natively encrypted via AES-256.");
                txtInput.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Encryption Error: {0}", ex.Message));
            }
        }

        private async void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOutput.Text)) return;
            try
            {
                txtInput.Text = _service.Decrypt(txtOutput.Text);
                await Services.ActivityLogService.LogActionAsync("User", "Encryption", "Data properly decrypted returning to plaintext.");
                txtOutput.Clear();
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid Cipher Text to decrypt.");
            }
        }
    }
}
