

using Microsoft.Azure.WebJobs;
using Smtp.WebJobs.Extensions;

namespace SmtpSample
{
    public static class SmtpFunctions
    {
        public static void SendEmail_Declarative(
            [QueueTrigger(@"samples-orders")] string order,
            [Smtp(To ="jason@jasonhaley.com", From ="jason@jasonhaley.com", Subject ="testing", Body ="this is a test")] string message)
        {
            // You can set additional message properties here
        }
    }
}
