using net = System.Net.Mail;
using System.Threading.Tasks;
using System.Net;

namespace Smtp.WebJobs.Extensions.Client
{
    internal class SmtpClient : ISmtpClient
    {
        private readonly net.SmtpClient _smtpClient;

        public SmtpClient(string host, int port, bool enableSsl, string userName, string password)
        {
            _smtpClient = new net.SmtpClient();
            _smtpClient.Host = host;
            _smtpClient.Port = port;
            _smtpClient.EnableSsl = enableSsl;
            _smtpClient.Credentials = new NetworkCredential(userName, password);
        }

        public Task SendMessageAsync(string from, string recipients, string subject, string body)
        {
            return _smtpClient.SendMailAsync(from, recipients, subject, body);
        }
    }
}
