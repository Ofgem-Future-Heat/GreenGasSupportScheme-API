using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Ofgem.API.GGSS.Persistence;
using Ofgem.API.GGSS.Persistence.Contracts;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOfgemCloudDbContext<ApplicationDbContext>(configuration, "ConnectionStrings:ApplicationApi");
      
            services.AddScoped<IDbContextFactory, DbContextFactory>();

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOrganisationRepository, OrganisationRepository>();
            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<IResponsiblePersonRepository, ResponsiblePersonRepository>();
            services.AddScoped<IUserOrganisationRepository, UserOrganisationRepository>();

            return services;    
        }
    }
}
