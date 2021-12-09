using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Ofgem.API.GGSS.WebApi
{
    // Do not expose via via the API docs
#pragma warning disable CS1591
    public class Program
    {
        protected Program() { }

        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseStartup<Startup>()
                            .ConfigureAppConfiguration((ctx, builder) =>
                            {
                                //Build the config from sources we have
                                var config = builder.Build();

                                if (!ctx.HostingEnvironment.IsDevelopment())
                                {
                                    //Create Managed Service Identity token provider
                                    var tokenProvider = new AzureServiceTokenProvider();
                                    //Create the Key Vault client
                                    var kvClient = new KeyVaultClient((authority, resource, scope) => tokenProvider.KeyVaultTokenCallback(authority, resource, scope));
                                    //Add Key Vault to configuration pipeline
                                    builder.AddAzureKeyVault(config["OfgemCloud:KeyVaultBaseUri"], kvClient, new DefaultKeyVaultSecretManager());
                                }
                            });
                });
    }
}
#pragma warning restore CS1591
