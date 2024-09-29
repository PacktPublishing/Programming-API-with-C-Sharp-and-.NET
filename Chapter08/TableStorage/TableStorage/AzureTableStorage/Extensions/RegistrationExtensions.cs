using Azure.Data.Tables;
using Azure.Identity;
using AzureTableStorage.Abstractions;
using AzureTableStorage.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AzureTableStorage.Extensions
{
    public static class RegistrationExtensions
    {
        public static IServiceCollection RegisterStorageTableServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IStorageTableService, StorageTableService>();

            var azureCredential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
            {
                TenantId = configuration["Azure:TenantId"]
            });

            services.AddSingleton(
            new TableServiceClient(new Uri(configuration["MasterFileUri"]), azureCredential));
            new TableServiceClient("DefaultEndpointsProtocol=https;AccountName=AccountName;AccountKey=;EndpointSuffix=core.windows.net");
            return services;
        }
    }
}