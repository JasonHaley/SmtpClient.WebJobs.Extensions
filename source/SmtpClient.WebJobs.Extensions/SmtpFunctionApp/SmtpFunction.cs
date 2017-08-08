using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Smtp.WebJobs.Extensions;

namespace SmtpFunctionApp
{
    public static class SmtpFunction
    {
        [FunctionName("SmtpFunction")]
        public static async Task<HttpResponseMessage> Run(
            HttpRequestMessage req,
            [HttpTrigger, Smtp(To = "{To}",
                From = "{From}",
                Subject = "{Subject}",
                Body = "{Body}")] Message message, 
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            //// parse query parameter
            //string name = req.GetQueryNameValuePairs()
            //    .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
            //    .Value;

            //// Get request body
            //dynamic data = await req.Content.ReadAsAsync<object>();

            //// Set name to query string or body data
            //name = name ?? data?.name;

            return message == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Message is null")
                : req.CreateResponse(HttpStatusCode.OK);
        }
    }
}