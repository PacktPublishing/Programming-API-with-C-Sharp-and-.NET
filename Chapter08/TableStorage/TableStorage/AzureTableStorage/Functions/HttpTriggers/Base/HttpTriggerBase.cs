using System.Net;
using Microsoft.Azure.Functions.Worker.Http;

namespace AzureFunc.Functions.HttpTriggers.Base
{
    public class HttpTriggerBase
    {
        protected async Task<HttpResponseData> CreateResponseAsync(HttpRequestData request, HttpStatusCode statusCode, string message)
        {
            var httpResponseData = request.CreateResponse(statusCode);

            await httpResponseData.WriteStringAsync(message);

            return httpResponseData;
        }
    }
}