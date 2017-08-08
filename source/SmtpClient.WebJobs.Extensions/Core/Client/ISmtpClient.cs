
using System.Threading.Tasks;

namespace Smtp.WebJobs.Extensions.Client
{
    public interface ISmtpClient
    {
        Task SendMessageAsync(string from, string recipients, string subject, string body);
    }
}
