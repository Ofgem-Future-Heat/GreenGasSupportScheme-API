using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ofgem.API.GGSS.Persistence.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ofgem.API.GGSS.WebApi.Helpers
{
    public class DatabaseHelper
    {
        internal static Task RunMigrationsTask(IServiceProvider provider)
        {
            return Task.Factory.StartNew(async () =>
            {
                using (var serviceScope = provider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var dbf = serviceScope.ServiceProvider.GetService<IDbContextFactory>();

                    var actx = await dbf.CreateApplicationContextAsync();
                    await actx.Database.MigrateAsync();
                }
            },
            CancellationToken.None);
        }
    }
}
