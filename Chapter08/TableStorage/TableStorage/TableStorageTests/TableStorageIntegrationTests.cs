using Azure;
using Azure.Data.Tables;
using AzureTableStorage.Abstractions;
using AzureTableStorage.Extensions;
using AzureTableStorage.Models;
using AzureTableStorage.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TableStorageTests
{
    public class TableStorageIntegrationTests
    {
        private readonly IServiceProvider _provider;

        public TableStorageIntegrationTests()
        {
            var serviceCollection = new ServiceCollection();
            var config = new ConfigurationRoot(new List<IConfigurationProvider>
            {
                new MemoryConfigurationProvider(new MemoryConfigurationSource
                {
                    InitialData = new Dictionary<string, string>
                    {
                        {"Azure:TenantId", ""},
                        {"MasterFileUri", "https://table.core.windows.net"},
                        {nameof(MasterFileOptions.TableName), "TestFile"}
                    }
                })
            });

            serviceCollection.AddSingleton<IConfiguration>(config);
            serviceCollection.AddOptions<MasterFileOptions>()
                            .Configure<IConfiguration>((settings, configuration) =>
                            {
                                configuration.Bind(settings);
                            });
            serviceCollection.RegisterStorageTableServices(config);
            _provider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public async Task KickOff()
        {
            var entity = new MasterFileTableModel
            {
                PartitionKey = Guid.NewGuid().ToString(),
                RowKey = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow,
                VehicleId = Guid.NewGuid().ToString(),
                FileName = "newTest.zip",
                FileSize = 3141592,
                SasUrl = ""
            };

            Response response = await _provider.GetService<IStorageTableService>().UpsertEntityAsync(entity);

            Assert.Equal(204, response.Status);
        }
    }
}