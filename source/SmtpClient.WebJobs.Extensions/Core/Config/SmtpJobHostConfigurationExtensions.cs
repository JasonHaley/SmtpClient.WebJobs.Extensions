using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Config;
using Smtp.WebJobs.Extensions.Bindings;
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

            config.RegisterExtensionConfigProvider(new SmtpExtensionConfig(smtpConfiguration));
        }

        private class SmtpExtensionConfig : IExtensionConfigProvider
        {
            private SmtpConfiguration _smtpConfiguration;

            public SmtpExtensionConfig(SmtpConfiguration smtpConfiguration)
            {
                _smtpConfiguration = smtpConfiguration;
            }

            public void Initialize(ExtensionConfigContext context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                context.Config.RegisterBindingExtension(new SmtpAttributeBindingProvider(_smtpConfiguration, context.Config.NameResolver, context.Trace));
            }
        }
    }
}
