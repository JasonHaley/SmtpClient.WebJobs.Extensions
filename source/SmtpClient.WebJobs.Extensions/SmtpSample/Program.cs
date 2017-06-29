using Microsoft.Azure.WebJobs;
using Smtp.WebJobs.Extensions.Config;

namespace SmtpSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new JobHostConfiguration();

            config.UseDevelopmentSettings();

            config.UseSmtp();

            JobHost host = new JobHost(config);

            host.RunAndBlock();
        }
    }
}
