using Microsoft.Azure.WebJobs;
using Smtp.WebJobs.Extensions;
using System.Net.Mail;

namespace SmtpSample
{
    public static class SmtpFunctions
    {
        public static void SendEmail_Declarative(
            [QueueTrigger(@"samples-orders")] string order,
            [Smtp(To ="jason@jasonhaley.com", From ="jason@jasonhaley.com", Subject ="testing", Body ="this is a test")] MailMessage message)
        {
            // You can set additional message properties here
        }
    }
}
