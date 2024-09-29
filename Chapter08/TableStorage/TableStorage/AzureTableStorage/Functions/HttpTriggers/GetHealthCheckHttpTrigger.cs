using System.Net;
using AzureFunc.Functions.HttpTriggers.Base;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace MasterFile.AzureFunc.Functions.HttpTriggers
{
    public class GetHealthCheckHttpTrigger : HttpTriggerBase
    {
        [Function(nameof(GetHealthCheckHttpTrigger))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "healthz")] HttpRequestData request)
        {
            try
            {
                var httpResponseData = await CreateResponseAsync(request, HttpStatusCode.OK, "Healthy");

                return httpResponseData;
            }
            catch (Exception)
            {
                var httpResponseData = await CreateResponseAsync(request, HttpStatusCode.ServiceUnavailable, "Unhealthy");

                return httpResponseData;
            }
        }
    }
}
