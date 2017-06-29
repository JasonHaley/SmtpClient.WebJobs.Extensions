using System;

namespace Smtp.WebJobs.Extensions
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class SmtpAttribute : Attribute
    {
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
    }
}
