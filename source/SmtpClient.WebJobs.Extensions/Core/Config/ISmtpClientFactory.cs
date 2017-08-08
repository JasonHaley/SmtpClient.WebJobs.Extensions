
using Smtp.WebJobs.Extensions.Client;

namespace Smtp.WebJobs.Extensions.Config
{
    internal interface ISmtpClientFactory
    {
        ISmtpClient Create(string host, int port, bool enableSsl, string userName, string password);
    }
}
