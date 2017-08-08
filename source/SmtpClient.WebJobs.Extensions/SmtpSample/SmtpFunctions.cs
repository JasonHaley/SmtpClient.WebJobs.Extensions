using Microsoft.Azure.WebJobs;
using Smtp.WebJobs.Extensions;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SmtpSample
{
    public static class SmtpFunctions
    {
        public static async Task SendEmail_Declarative(
            [QueueTrigger(@"samples-orders")] string order,
            [Smtp] IAsyncCollector<Message> messages)
        {
            var message = new Message()
            {
                To = "jason@jasonhaley.com",
                From = "hello@jasonhaley.com",
                Subject = "testing",
                Body = "this is a test",
                IsHtml = true
            };
            await messages.AddAsync(message);
        }

        //public static void SendEmail_Declarative(
        //    [QueueTrigger(@"samples-orders")] string order,
        //    [Smtp(To ="jason@jasonhaley.com", From ="haleyjason@gmail.com", Subject ="testing", Body ="this is a test")] Message message)
        //{
        //    // You can set additional message properties here
        //}
    }
}
