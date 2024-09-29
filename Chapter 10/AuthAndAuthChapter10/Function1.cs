using FunctionChapter6;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FunctionsChapter6
{
    public class Function1
    {
        private readonly IOptions<MyOptions> _options;
        private readonly ILogger<Function1> _logger;

        public Function1(IOptions<MyOptions> options, ILogger<Function1> logger)
        {
            _options = options;
            _logger = logger;
        }

        [Function("Function1")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult(_options.Value.MyReturnProperty);
        }
    }
}
