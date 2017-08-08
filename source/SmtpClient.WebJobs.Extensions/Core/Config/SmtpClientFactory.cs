using Smtp.WebJobs.Extensions.Client;

namespace Smtp.WebJobs.Extensions.Config
{
    public class SmtpClientFactory : ISmtpClientFactory
    {
        public ISmtpClient Create(string host, int port, bool enableSsl, string userName, string password)
        {
            return new SmtpClient(host, port, enableSsl, userName, password);
        }
    }
}
