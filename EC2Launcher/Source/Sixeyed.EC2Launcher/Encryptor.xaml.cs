using System.Collections.Generic;
using System.Windows;
using Sixeyed.EC2Launcher.Configuration;

namespace Sixeyed.EC2Launcher
{
    /// <summary>
    /// Window for encrypting <see cref="AwsConfiguration"/> settings
    /// </summary>
    public partial class Encryptor : Window
    {        
        public Encryptor()
        {
            InitializeComponent();
            LoadConfiguredValues();
        }

        private void LoadConfiguredValues()
        {
            txtAccessKey.Text = string.Empty;
            txtSecretKey.Text = string.Empty;
            txtInstanceId.Text = AwsConfiguration.InstanceId;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                AwsConfiguration.AccessKey = txtAccessKey.Text;
                AwsConfiguration.SecretKey = txtSecretKey.Text;
                AwsConfiguration.InstanceId = txtInstanceId.Text;
                DialogResult = true;
                Close();
            }
        }

        private bool IsValid()
        {
            var missingFields = new List<string>();
            if (string.IsNullOrWhiteSpace(txtAccessKey.Text))
            {
                missingFields.Add("Access Key");
            }
            if (string.IsNullOrWhiteSpace(txtSecretKey.Text))
            {
                missingFields.Add("Secret Key");
            }
            if (string.IsNullOrWhiteSpace(txtInstanceId.Text))
            {
                missingFields.Add("Instance Id");
            }

            if (missingFields.Count == 0)
                return true;

            MessageBox.Show(string.Format("Required fields not completed: {0}", string.Join(", ", missingFields.ToArray())), "Validation Faliure");
            return false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
