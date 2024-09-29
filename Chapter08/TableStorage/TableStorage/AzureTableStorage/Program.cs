using Azure.Identity;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureServices((builder, services) =>
            {
                services.AddLogging();
                services.AddHttpClient();
                services.AddMemoryCache();

                services.AddAutoMapper(typeof(Program));
            })
            .ConfigureFunctionsWorkerDefaults(workerApplication =>
             {
                 workerApplication.UseNewtonsoftJson();
             })
            .ConfigureAppConfiguration((ctx, configuration) =>
            {
                var settings = configuration.Build();

                var keyVaultName = settings["Azure:KeyVaultName"];

                if (string.IsNullOrEmpty(keyVaultName))
                {
                    throw new Exception("Azure__KeyVaultName can not be empty");
                }

                var tenantId = settings["Azure:TenantId"];
                if (string.IsNullOrEmpty(tenantId))
                {
                    throw new Exception("Azure__TenantId can not be empty");
                }

                //configuration.AddAzureKeyVault(
                //    new Uri("https://" + keyVaultName + ".vault.azure.net/"),
                //    new DefaultAzureCredential(new DefaultAzureCredentialOptions
                //    {
                //        TenantId = tenantId
                //    }));
            })
            .Build();

        await host.RunAsync();
    }
}