using Azure.Identity;
using System.Net.Http.Headers;

namespace ClientApp
{
    internal class Program
    {
        private static readonly string clientId = "<your client id>";
        private static readonly string tenantId = "<your tenantId>";
        static async Task Main(string[] args)
        {
            DefaultAzureCredential cred = new DefaultAzureCredential(new DefaultAzureCredentialOptions
            {
                TenantId = tenantId
            });
            //ClientSecretCredential cred = new ClientSecretCredential(tenantId, clientId, "<your secret>");
            Azure.Core.AccessToken token = await cred.GetTokenAsync(new Azure.Core.TokenRequestContext(new string[]
            {
                clientId
            }), CancellationToken.None);

            var message = new HttpRequestMessage(HttpMethod.Get, "https://<your function resource>.azurewebsites.net/api/Function1");
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
            HttpResponseMessage resp = await new HttpClient().SendAsync(message);//inject the HttpClient in a production app
            resp.EnsureSuccessStatusCode();
            string content = await resp.Content.ReadAsStringAsync();
            Console.WriteLine(content);
        }
    }
}
