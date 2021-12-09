using System.Threading;
using System.Threading.Tasks;

namespace Ofgem.API.GGSS.Persistence.Contracts
{
    public interface IDbContextFactory
    {
        string TennantId { get; }
        Task<ApplicationDbContext> CreateApplicationContextAsync(CancellationToken cancellationToken = default);
    }
}
