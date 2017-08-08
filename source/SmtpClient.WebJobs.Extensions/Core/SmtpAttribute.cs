using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using System;

namespace Smtp.WebJobs.Extensions
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter)]
    public class SmtpAttribute : Attribute
    {
        [AutoResolve]
        public string To { get; set; }
        [AutoResolve]
        public string Cc { get; set; }
        [AutoResolve]
        public string Bcc { get; set; }
        [AutoResolve]
        public string From { get; set; }
        [AutoResolve]
        public string Subject { get; set; }
        [AutoResolve]
        public string Body { get; set; }
        
        public bool IsHtml { get; set; }
    }
}
