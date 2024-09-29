using FunctionChapter6;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FunctionsChapter6
{
    public class Products
    {
        private readonly IOptions<MyOptions> _options;
        private readonly ILogger<Products> _logger;

        public Products(IOptions<MyOptions> options, ILogger<Products> logger)
        {
            _options = options;
            _logger = logger;
        }

        [Function(nameof(Products))]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products/{category:alpha}/{id:int?}")] HttpRequest req,
            string category, int id = 0)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult(new
            {
                category,
                id
            });
        }
    }
}
