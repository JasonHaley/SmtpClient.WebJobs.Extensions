using Microsoft.Azure.WebJobs;
using System;

namespace Smtp.WebJobs.Extensions.Config
{
    public static class SmtpJobHostConfigurationExtensions
    {
        public static void UseSmtp(this JobHostConfiguration config, SmtpConfiguration smtpConfiguration = null)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (smtpConfiguration == null)
            {
                smtpConfiguration = new SmtpConfiguration();
            }

            config.RegisterExtensionConfigProvider(smtpConfiguration);
        }
        
    }
}
