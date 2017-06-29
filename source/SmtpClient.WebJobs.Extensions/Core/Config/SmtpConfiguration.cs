using System;
using System.Configuration;

namespace Smtp.WebJobs.Extensions.Config
{
    public class SmtpConfiguration 
    {
        internal const string AzureWebJobsSmtpUser = "AzureWebJobsSmtpUser";
        internal const string AzureWebJobsSmtpPassword = "AzureWebJobsSmtpPassword";
        internal const string AzureWebJobsSmtpPort = "AzureWebJobsSmtpPort";
        internal const string AzureWebJobsSmtpHost = "AzureWebJobsSmtpHost";
        internal const string AzureWebJobsSmtpEnableSsl = "AzureWebJobsSmtpEnableSsl";

        internal const int DefaultPort = 587;

        public SmtpConfiguration()
        {
            Initialize();
        }

        private void Initialize()
        {
            User = GetValue(AzureWebJobsSmtpUser);
            Password = GetValue(AzureWebJobsSmtpPassword);
            Port = GetIntValue(AzureWebJobsSmtpPort);
            Host = GetValue(AzureWebJobsSmtpHost);
            EnableSsl = GetBoolValue(AzureWebJobsSmtpEnableSsl);
        }

        private string GetValue(string key)
        {
            var value = ConfigurationManager.AppSettings.Get(key);
            if (string.IsNullOrEmpty(value))
            {
                value = Environment.GetEnvironmentVariable(key);
            }
            return value;
        }

        private int GetIntValue(string key)
        {
            var value = GetValue(key);
            int val = 0;
            if (int.TryParse(value, out val))
            {
                return val;
            }
            else
            {
                return DefaultPort;
            }
        }

        private bool GetBoolValue(string key)
        {
            var value = GetValue(key);
            bool val = false;
            if (bool.TryParse(value, out val))
            {
                return val;
            }
            else
            {
                return false;
            }
        }
               

        public string User { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string Host { get; set; }
        public bool EnableSsl { get; set; }
    }
}
