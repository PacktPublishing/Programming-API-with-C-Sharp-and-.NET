using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TableStorageConsoleApp.Abstractions;
using TableStorageConsoleApp.Services;

namespace TableStorageConsoleApp.Extensions
{
    public static class RegistrationExtensions
    {
        public static IServiceCollection RegisterStorageTableServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IStorageTableService, StorageTableService>();
            return services;
        }
    }
}
