using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.Dynamic;

namespace FunctionsChapter6
{
    public static class MyOrchestrator
    {
        [Function(nameof(MyOrchestrator))]
        public static async Task<object> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            await context.CallActivityAsync(nameof(SayHello));
            dynamic eventValue = await context.WaitForExternalEvent<ExpandoObject>("MyEvent");
            await context.CallActivityAsync(nameof(SayHello), eventValue.value);
            return default;
        }

        [Function(nameof(SayHello))]
        public static async Task<string> SayHello([ActivityTrigger] string contents, FunctionContext executionContext)
        {
            await File.WriteAllTextAsync("myfile.txt", contents);
            return default;
        }

        [Function("MyOrchestrator_HttpStart")]
        public static async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("MyOrchestrator_HttpStart");

            // Function input comes from the request content.
            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                nameof(MyOrchestrator));

            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            // Returns an HTTP 202 response with an instance management payload.
            // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
            return await client.CreateCheckStatusResponseAsync(req, instanceId);
        }
    }
}
