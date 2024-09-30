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

            services.AddSingleton(
                new TableServiceClient("DefaultEndpointsProtocol=https;AccountName=<>;AccountKey=<>;EndpointSuffix=core.windows.net"));
            return services;
        }
    }
}