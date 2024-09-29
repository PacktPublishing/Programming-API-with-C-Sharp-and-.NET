using AzureTableStorage.Models;
using AzureTableStorage.Extensions;
using AzureTableStorage.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Azure;

namespace AzureTableStorageTests
{
    public class AzureTableStorageIntegrationTests
    {
        private readonly IServiceProvider _provider;

        public AzureTableStorageIntegrationTests()
        {
            var serviceCollection = new ServiceCollection();
            var config = new ConfigurationRoot(new List<IConfigurationProvider>
            {
                new MemoryConfigurationProvider(new MemoryConfigurationSource
                {
                    InitialData = new Dictionary<string, string>
                    {
                        {"Azure:TenantId", ""},
                        {"MasterFileUri", ""},
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
                FileName = "outside.zip",
                FileSize = 3141592,
                SasUrl = ""
            };

            Response response = await _provider.GetService<IStorageTableService>().UpsertEntityAsync(entity);

            Assert.Equal(204, response.Status);
        }
    }
}