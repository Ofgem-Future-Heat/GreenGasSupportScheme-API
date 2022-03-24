using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ofgem.API.GGSS.Application.Contracts.Persistence
{
    public interface IApplicationRepository : IAsyncRepository<Entities.Application>
    {
        Task<IReadOnlyList<Entities.Application>> ListAllForUserAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Entities.Application>> ListAllForUserWithDocumentsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Entities.Application>> ListAll(CancellationToken cancellationToken = default);
        Task<Entities.Application> GetById(Guid applicationId, CancellationToken cancellationToken = default);
    }
}
