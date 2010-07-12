using System;
using System.Configuration;
using Sixeyed.EC2Launcher.Cryptography;

namespace Sixeyed.EC2Launcher.Configuration
{
    /// <summary>
    /// Wrapper class for accessing AWS configuration
    /// </summary>
    public static class AwsConfiguration
    {
        /// <summary>
        /// Gets the friendly name of the EC2 instance to launch
        /// </summary>
        public static string PCName
        {
            get 
            {
                var pcName = ConfigurationManager.AppSettings["PCName"];
                if (string.IsNullOrEmpty(pcName))
                {
                    pcName = string.Format("Amazon EC2 Instance: {0}", InstanceId);
                }
                return pcName;
            }
        }

        /// <summary>
        /// Gets the URL for Amazon Web Services
        /// </summary>
        public static string ServiceUrl
        {
            get { return ConfigurationManager.AppSettings["ServiceUrl"]; }
        }

        /// <summary>
        /// Gets the path to the RDP app
        /// </summary>
        public static string RdpPath
        {
            get { return ConfigurationManager.AppSettings["RdpPath"]; }
        }

        /// <summary>
        /// Gets the argument format for passing the EC2 DNS name to the RDP app
        /// </summary>
        public static string RdpDnsArgumentFormat
        {
            get { return ConfigurationManager.AppSettings["RdpDnsArgumentFormat"]; }
        }

        /// <summary>
        /// Gets the number of milliseconds to wait between polling AWS for instance status
        /// </summary>
        public static int UpdateWaitMilliseconds
        {
            get { return int.Parse(ConfigurationManager.AppSettings["UpdateWaitMilliseconds"]); }
        }

        /// <summary>
        /// Gets the number of minutes to wait before automatically stopping the instance
        /// </summary>
        public static int StopTimeoutMinutes
        {
            get { return int.Parse(ConfigurationManager.AppSettings["StopTimeoutMinutes"]); }
        }

        /// <summary>
        /// Gets/sets the encrypted AWS Access Key
        /// </summary>
        public static string AccessKey
        {
            get { return DecryptAppSetting("AwsAccessKey"); }
            set { EncryptAppSetting("AwsAccessKey", value); }
        }

        /// <summary>
        /// Gets/sets the encrypted AWS Secret Key
        /// </summary>
        public static string SecretKey
        {
            get { return DecryptAppSetting("AwsSecretKey"); }
            set { EncryptAppSetting("AwsSecretKey", value); }
        }

        /// <summary>
        /// Gets/sets the ID of the EC2 instance to launch
        /// </summary>
        public static string InstanceId
        {
            get { return ConfigurationManager.AppSettings["InstanceId"]; }
            set { SaveAppSetting("InstanceId", value); }
        }

        private static void EncryptAppSetting(string settingName, string value)
        {
            SaveAppSetting(settingName, CryptoProvider.Encrypt(value));
        }

        private static void SaveAppSetting(string settingName, string value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[settingName].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private static string DecryptAppSetting(string settingName)
        {
            var plainText = string.Empty;
            var cipherText = ConfigurationManager.AppSettings[settingName];
            if (!string.IsNullOrWhiteSpace(cipherText))
            {
                try
                {
                    plainText = CryptoProvider.Decrypt(cipherText);
                }
                catch(Exception ex)
                {
                    //ignore
                }
            }
            return plainText;
        }
    }
}
