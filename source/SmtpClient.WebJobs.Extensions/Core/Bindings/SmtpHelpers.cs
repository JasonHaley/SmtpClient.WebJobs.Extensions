using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Smtp.WebJobs.Extensions.Config;
using System;

namespace Smtp.WebJobs.Extensions
{
    internal class SmtpHelpers
    {
        internal static Message CreateMessage(string input)
        {
            JObject json = JObject.Parse(input);
            return CreateMessage(json);
        }

        internal static Message CreateMessage(JObject input)
        {
            return input.ToObject<Message>();
        }

        internal static string CreateString(Message input)
        {
            return CreateString(JObject.FromObject(input));
        }

        internal static string CreateString(JObject input)
        {
            return input.ToString(Formatting.None);
        }

        internal static SmtpConfiguration CreateConfiguration(JObject metadata)
        {
            SmtpConfiguration config = new SmtpConfiguration();

            JObject configSection = (JObject)metadata.GetValue("smtp", StringComparison.OrdinalIgnoreCase);
            JToken value = null;
            if (configSection != null)
            {
                //Message mailAddress = null;
                //if (configSection.TryGetValue("from", StringComparison.OrdinalIgnoreCase, out value) &&
                //    TryParseAddress((string)value, out mailAddress))
                //{
                //    config.FromAddress = mailAddress;
                //}

                //if (configSection.TryGetValue("to", StringComparison.OrdinalIgnoreCase, out value) &&
                //    TryParseAddress((string)value, out mailAddress))
                //{
                //    sendGridConfig.ToAddress = mailAddress;
                //}
            }

            return config;
        }
    }
}
