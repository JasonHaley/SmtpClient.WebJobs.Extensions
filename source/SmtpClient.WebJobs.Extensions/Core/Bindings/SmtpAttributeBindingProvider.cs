using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Smtp.WebJobs.Extensions.Config;
using System;
using System.Globalization;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;

namespace Smtp.WebJobs.Extensions.Bindings
{
    public class SmtpAttributeBindingProvider : IBindingProvider
    {
        private readonly SmtpConfiguration _config;
        private readonly INameResolver _nameResolver;
        private readonly TraceWriter _traceWriter;

        public SmtpAttributeBindingProvider(SmtpConfiguration config, INameResolver nameResolver, TraceWriter traceWriter)
        {
            _config = config;
            _nameResolver = nameResolver;
            _traceWriter = traceWriter;
        }

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            ParameterInfo parameter = context.Parameter;
            SmtpAttribute attribute = parameter.GetCustomAttribute<SmtpAttribute>(inherit: false);
            if (attribute == null)
            {
                return Task.FromResult<IBinding>(null);
            }

            //if (context.Parameter.ParameterType != typeof(MailMessage) &&
            //    context.Parameter.ParameterType != typeof(MailMessage).MakeByRefType())
            //{
            //    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
            //        "Can't bind SmtpAttribute to type '{0}'.", parameter.ParameterType));
            //}

            if (string.IsNullOrEmpty(_config.Host))
            {
                throw new InvalidOperationException(
                    string.Format("The Smtp Host must be set either via a '{0}' app setting, via a '{0}' environment variable, or directly in code via SmtpConfiguration.Host.",
                    SmtpConfiguration.AzureWebJobsSmtpHost));
            }

            if (_config.Port == 0)
            {
                throw new InvalidOperationException(
                    string.Format("The Smtp Port must be set either via a '{0}' app setting, via a '{0}' environment variable, or directly in code via SmtpConfiguration.Port.",
                    SmtpConfiguration.AzureWebJobsSmtpPort));
            }

            if (string.IsNullOrEmpty(_config.User))
            {
                throw new InvalidOperationException(
                    string.Format("The Smtp User must be set either via a '{0}' app setting, via a '{0}' environment variable, or directly in code via SmtpConfiguration.User.",
                    SmtpConfiguration.AzureWebJobsSmtpUser));
            }

            if (string.IsNullOrEmpty(_config.Password))
            {
                throw new InvalidOperationException(
                    string.Format("The Smtp Password must be set either via a '{0}' app setting, via a '{0}' environment variable, or directly in code via SmtpConfiguration.Password.",
                    SmtpConfiguration.AzureWebJobsSmtpPassword));
            }

            return Task.FromResult<IBinding>(new SmtpBinding(parameter, attribute, _config, _nameResolver, context));
        }
    }
}
