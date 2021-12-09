using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Ofgem.API.GGSS.Persistence.Contracts;
using Ofgem.Azure.SecureDbContext.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ofgem.API.GGSS.Persistence
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly IOfgemDbContext<ApplicationDbContext> _applicationContextFactory;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostEnvironment;

        public string TennantId => _configuration.GetSection("OfgemCloud:TennantId").Get<string>();

        public DbContextFactory(
            IOfgemDbContext<ApplicationDbContext> applicationContextFactory,
            IConfiguration configuration, 
            IHostEnvironment hostEnvironment)
        {
            _applicationContextFactory = applicationContextFactory ?? throw new ArgumentNullException(nameof(applicationContextFactory));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        public async Task<ApplicationDbContext> CreateApplicationContextAsync(CancellationToken cancellationToken = default)
        {
            return _hostEnvironment.IsDevelopment()
                ? await _applicationContextFactory.CreateDbContext(cancellationToken)
                : await _applicationContextFactory.CreateSecureDbContext(this.TennantId, cancellationToken);
        }
    }
}
