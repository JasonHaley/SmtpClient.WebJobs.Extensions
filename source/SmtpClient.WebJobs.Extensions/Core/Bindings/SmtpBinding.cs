using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Bindings.Path;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Smtp.WebJobs.Extensions.Config;
using System.Net;

namespace Smtp.WebJobs.Extensions.Bindings
{
    public class SmtpBinding : IBinding
    {
        private readonly ParameterInfo _parameter;
        private readonly SmtpAttribute _attribute;
        private readonly SmtpConfiguration _config;
        private readonly INameResolver _nameResolver;
        private readonly SmtpClient _smtpClient;
        private readonly BindingTemplate _toFieldBindingTemplate;
        private readonly BindingTemplate _ccFieldBindingTemplate;
        private readonly BindingTemplate _bccFieldBindingTemplate;
        private readonly BindingTemplate _fromFieldBindingTemplate;
        private readonly BindingTemplate _subjectFieldBindingTemplate;
        private readonly BindingTemplate _bodyFieldBindingTemplate;
        
        public SmtpBinding(ParameterInfo parameter, SmtpAttribute attribute, SmtpConfiguration config, INameResolver nameResolver, BindingProviderContext context)
        {
            _parameter = parameter;
            _attribute = attribute;
            _config = config;
            _nameResolver = nameResolver;

            _smtpClient = new SmtpClient();

            _smtpClient.Host = _config.Host;
            _smtpClient.Port = _config.Port;
            _smtpClient.EnableSsl = _config.EnableSsl;
            _smtpClient.Credentials = new NetworkCredential(_config.User, _config.Password);

            if (!string.IsNullOrEmpty(_attribute.To))
            {
                _toFieldBindingTemplate = CreateBindingTemplate(_attribute.To, context.BindingDataContract);
            }

            if (!string.IsNullOrEmpty(_attribute.Cc))
            {
                _ccFieldBindingTemplate = CreateBindingTemplate(_attribute.Cc, context.BindingDataContract);
            }

            if (!string.IsNullOrEmpty(_attribute.Bcc))
            {
                _bccFieldBindingTemplate = CreateBindingTemplate(_attribute.Bcc, context.BindingDataContract);
            }

            if (!string.IsNullOrEmpty(_attribute.From))
            {
                _fromFieldBindingTemplate = CreateBindingTemplate(_attribute.From, context.BindingDataContract);
            }

            if (!string.IsNullOrEmpty(_attribute.Subject))
            {
                _subjectFieldBindingTemplate = CreateBindingTemplate(_attribute.Subject, context.BindingDataContract);
            }

            if (!string.IsNullOrEmpty(_attribute.Body))
            {
                _bodyFieldBindingTemplate = CreateBindingTemplate(_attribute.Body, context.BindingDataContract);
            }
        }
        public bool FromAttribute
        {
            get { return true; }
        }

        public async Task<IValueProvider> BindAsync(BindingContext context)
        {
            var message = CreateDefaultMessage(context.BindingData);

            return await BindAsync(message, context.ValueContext);
        }

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
        {
            var message = (MailMessage)value;

            return Task.FromResult<IValueProvider>(new SmtpValueBinder(_smtpClient, message));
        }

        public ParameterDescriptor ToParameterDescriptor()
        {
            return new ParameterDescriptor
            {
                Name = _parameter.Name
            };
        }

        internal MailMessage CreateDefaultMessage(IReadOnlyDictionary<string, object> bindingData)
        {
            MailMessage message = new MailMessage();

            if (_fromFieldBindingTemplate != null)
            {
                message.From = new MailAddress(_fromFieldBindingTemplate.Bind(bindingData));
            }

            if (_toFieldBindingTemplate != null)
            {
                message.To.Add(new MailAddress(_toFieldBindingTemplate.Bind(bindingData)));
            }

            if (_ccFieldBindingTemplate != null)
            {
                message.CC.Add(new MailAddress(_ccFieldBindingTemplate.Bind(bindingData)));
            }

            if (_bccFieldBindingTemplate != null)
            {
                message.Bcc.Add(new MailAddress(_toFieldBindingTemplate.Bind(bindingData)));
            }

            if (_subjectFieldBindingTemplate != null)
            {
                message.Subject = _subjectFieldBindingTemplate.Bind(bindingData);
            }

            if (_bodyFieldBindingTemplate != null)
            {
                message.Body = _bodyFieldBindingTemplate.Bind(bindingData);
            }
            message.IsBodyHtml = _attribute.IsHtml;

            return message;
        }

        private BindingTemplate CreateBindingTemplate(string pattern, IReadOnlyDictionary<string, Type> bindingDataContract)
        {
            if (_nameResolver != null)
            {
                pattern = _nameResolver.ResolveWholeString(pattern);
            }
            BindingTemplate bindingTemplate = BindingTemplate.FromString(pattern);
            bindingTemplate.ValidateContractCompatibility(bindingDataContract);

            return bindingTemplate;
        }

        internal class SmtpValueBinder : IValueBinder
        {
            private readonly MailMessage _message;
            private readonly SmtpClient _smtpClient;

            public SmtpValueBinder(SmtpClient smtpClient, MailMessage message)
            {
                _message = message;
                _smtpClient = smtpClient;
            }

            public Type Type
            {
                get
                {
                    return typeof(MailMessage);
                }
            }

            public object GetValue()
            {
                return _message;
            }

            public Task<object> GetValueAsync()
            {
                return Task.FromResult<object>(_message);
            }

            public async Task SetValueAsync(object value, CancellationToken cancellationToken)
            {
                if (value == null)
                {
                    // if this is a 'ref' binding and the user set the parameter to null, that
                    // signals that they don't want us to send the message
                    return;
                }

                if (_message.To == null)
                {
                    throw new InvalidOperationException("A 'To' sms number must be specified for the message.");
                }
                if (_message.From == null)
                {
                    throw new InvalidOperationException("A 'From' phone number must be specified for the message.");
                }

                await _smtpClient.SendMailAsync(_message);
                
                return;
            }

            public string ToInvokeString()
            {
                return null;
            }
        }
    }
}
