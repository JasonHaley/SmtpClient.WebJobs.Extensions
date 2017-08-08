using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;
using Newtonsoft.Json.Linq;
using Smtp.WebJobs.Extensions.Bindings;
using Smtp.WebJobs.Extensions.Client;
using Smtp.WebJobs.Extensions.Utilities;
using System;
using System.Collections.Concurrent;


namespace Smtp.WebJobs.Extensions.Config
{
    public class SmtpConfiguration : IExtensionConfigProvider
    {
        internal const string AzureWebJobsSmtpUserKeyName = "AzureWebJobsSmtpUser";
        internal const string AzureWebJobsSmtpPasswordKeyName = "AzureWebJobsSmtpPassword";
        internal const string AzureWebJobsSmtpPortKeyName = "AzureWebJobsSmtpPort";
        internal const string AzureWebJobsSmtpHostKeyName = "AzureWebJobsSmtpHost";
        internal const string AzureWebJobsSmtpEnableSslKeyName = "AzureWebJobsSmtpEnableSsl";

        internal const int DefaultPort = 587;
        internal const bool DefaultEnableSsl = true;
        private ConcurrentDictionary<string, ISmtpClient> _smtpClientCache = new ConcurrentDictionary<string, ISmtpClient>();


        public SmtpConfiguration()
        {
            ClientFactory = new SmtpClientFactory();
        }
        
        internal ISmtpClientFactory ClientFactory { get; set; }
        
        public void Initialize(ExtensionConfigContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var nameResolver = context.Config.NameResolver;
            User = nameResolver.Resolve(AzureWebJobsSmtpUserKeyName);
            Password = nameResolver.Resolve(AzureWebJobsSmtpPasswordKeyName);
            Port = nameResolver.ResolveAsInt(AzureWebJobsSmtpPortKeyName, DefaultPort);
            Host = nameResolver.Resolve(AzureWebJobsSmtpHostKeyName);
            EnableSsl = nameResolver.ResolveAsBool(AzureWebJobsSmtpEnableSslKeyName, DefaultEnableSsl);
            
            context.AddConverter<Message, string>(SmtpHelpers.CreateString);
            context.AddConverter<string, Message>(SmtpHelpers.CreateMessage);
            context.AddConverter<JObject, Message>(SmtpHelpers.CreateMessage);

            var rule = context.AddBindingRule<SmtpAttribute>();

            rule.BindToInput<Message>(BuildItemFromAttr);
            rule.BindToCollector<Message>(BuildCollector);

            //var factory = new BindingFactory(nameResolver, converterManager);
            //var outputProvider = factory.BindToCollector<SmtpAttribute, Message>((attr) =>
            //{
            //    ISmtpClient client = _smtpClientCache.GetOrAdd(Host, ClientFactory.Create(Host, Port, EnableSsl, User, Password));
            //    return new MessageAsyncCollector(this, attr, client);
            //});

            //IExtensionRegistry extensions = context.Config.GetService<IExtensionRegistry>();
            //extensions.RegisterBindingRules<SmtpAttribute>(ValidateBinding, nameResolver, outputProvider);
        }
        private IAsyncCollector<Message> BuildCollector(SmtpAttribute attribute)
        {
            ISmtpClient client = _smtpClientCache.GetOrAdd(Host, ClientFactory.Create(Host, Port, EnableSsl, User, Password));
            return new MessageAsyncCollector(this, attribute, client);
        }

        // All {} and %% in the Attribute have been resolved by now. 
        private Message BuildItemFromAttr(SmtpAttribute attribute)
        {
            //var root = GetRoot(attribute);
            //var path = Path.Combine(root, attribute.FileName);
            //if (!File.Exists(path))
            //{
            //    return null;
            //}
            //var contents = File.ReadAllText(path);
            return new Message
            {
                To = attribute.To,
                Cc = attribute.Cc,
                Bcc = attribute.Bcc,
                From = attribute.From,
                Subject = attribute.Subject,
                Body = attribute.Body,
                IsHtml = attribute.IsHtml
            };
        }
        private void ValidateBinding(SmtpAttribute attribute, Type type)
        {
            //string apiKey = FirstOrDefault(attribute.ApiKey, ApiKey, _defaultApiKey);

            //if (string.IsNullOrEmpty(apiKey))
            //{
            //    throw new InvalidOperationException(
            //        $"The SendGrid ApiKey must be set either via an '{AzureWebJobsSendGridApiKeyName}' app setting, via an '{AzureWebJobsSendGridApiKeyName}' environment variable, or directly in code via {nameof(SendGridConfiguration)}.{nameof(SendGridConfiguration.ApiKey)} or {nameof(SendGridAttribute)}.{nameof(SendGridAttribute.ApiKey)}.");
            //}
        }

        //private static string FirstOrDefault(params string[] values)
        //{
        //    return values.FirstOrDefault(v => !string.IsNullOrEmpty(v));
        //}

        public string User { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string Host { get; set; }
        public bool EnableSsl { get; set; }
    }
}
