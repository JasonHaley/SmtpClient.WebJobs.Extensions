//using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.WebJobs.Host;
//using Microsoft.Azure.WebJobs.Host.Bindings;
//using Smtp.WebJobs.Extensions.Config;
//using System;
//using Microsoft.Azure.WebJobs.Script.Extensibility;
//using System.Reflection;
//using System.Threading.Tasks;
//using Newtonsoft.Json.Linq;

//namespace Smtp.WebJobs.Extensions.Bindings
//{
//    public class SmtpAttributeBindingProvider : ScriptBindingProvider
//    {
        
//        public SmtpAttributeBindingProvider(JobHostConfiguration config, JObject hostMetadata, TraceWriter traceWriter)
//             : base(config, hostMetadata, traceWriter)
//        { }

//        public override bool TryCreate(ScriptBindingContext context, out ScriptBinding binding)
//        {
//            if (context == null)
//            {
//                throw new ArgumentNullException("context");
//            }

//            binding = null;

//            if (string.Compare(context.Type, "smtp", StringComparison.OrdinalIgnoreCase) == 0)
//            {
//                binding = new SmtpBinding(context);
//            }

//            return binding != null;
//        }

//        /// <inheritdoc/>
//        public override void Initialize()
//        {
//            var config = SmtpHelpers.CreateConfiguration(Metadata);
//            Config.UseSmtp(config);
//        }

//        /// <inheritdoc/>
//        //public override bool TryResolveAssembly(string assemblyName, out Assembly assembly)
//        //{
//        //    assembly = null;

//        //    Assembly sendGridAssembly = typeof(SendGridAPIClient).Assembly;
//        //    if (string.Compare(assemblyName, sendGridAssembly.GetName().Name, StringComparison.OrdinalIgnoreCase) == 0)
//        //    {
//        //        assembly = sendGridAssembly;
//        //    }

//        //    return assembly != null;
//        //}


//        //public Task<IBinding> TryCreateAsync(BindingProviderContext context)
//        //{
//        //    if (context == null)
//        //    {
//        //        throw new ArgumentNullException(nameof(context));
//        //    }

//        //    ParameterInfo parameter = context.Parameter;
//        //    SmtpAttribute attribute = parameter.GetCustomAttribute<SmtpAttribute>(inherit: false);
//        //    if (attribute == null)
//        //    {
//        //        return Task.FromResult<IBinding>(null);
//        //    }

//        //    //if (context.Parameter.ParameterType != typeof(MailMessage) &&
//        //    //    context.Parameter.ParameterType != typeof(MailMessage).MakeByRefType())
//        //    //{
//        //    //    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
//        //    //        "Can't bind SmtpAttribute to type '{0}'.", parameter.ParameterType));
//        //    //}

//        //    if (string.IsNullOrEmpty(_config.Host))
//        //    {
//        //        throw new InvalidOperationException(
//        //            string.Format("The Smtp Host must be set either via a '{0}' app setting, via a '{0}' environment variable, or directly in code via SmtpConfiguration.Host.",
//        //            SmtpConfiguration.AzureWebJobsSmtpHost));
//        //    }

//        //    if (_config.Port == 0)
//        //    {
//        //        throw new InvalidOperationException(
//        //            string.Format("The Smtp Port must be set either via a '{0}' app setting, via a '{0}' environment variable, or directly in code via SmtpConfiguration.Port.",
//        //            SmtpConfiguration.AzureWebJobsSmtpPort));
//        //    }

//        //    if (string.IsNullOrEmpty(_config.User))
//        //    {
//        //        throw new InvalidOperationException(
//        //            string.Format("The Smtp User must be set either via a '{0}' app setting, via a '{0}' environment variable, or directly in code via SmtpConfiguration.User.",
//        //            SmtpConfiguration.AzureWebJobsSmtpUser));
//        //    }

//        //    if (string.IsNullOrEmpty(_config.Password))
//        //    {
//        //        throw new InvalidOperationException(
//        //            string.Format("The Smtp Password must be set either via a '{0}' app setting, via a '{0}' environment variable, or directly in code via SmtpConfiguration.Password.",
//        //            SmtpConfiguration.AzureWebJobsSmtpPassword));
//        //    }

//        //    return Task.FromResult<IBinding>(new SmtpBinding(parameter, attribute, _config, _nameResolver, context));
//        //}
//    }
//}
