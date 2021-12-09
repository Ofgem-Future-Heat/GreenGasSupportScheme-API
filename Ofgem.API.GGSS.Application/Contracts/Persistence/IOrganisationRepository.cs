using Ofgem.API.GGSS.Application.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ofgem.API.GGSS.Application.Contracts.Persistence
{
    public interface IOrganisationRepository : IAsyncRepository<Organisation>
    {
        Task<IReadOnlyList<Organisation>> ListAllForUserAsync(string userId, CancellationToken cancellationToken = default);
        Task<string> AddWithResponsiblePersonAsync(Organisation organisation, CancellationToken cancellationToken = default);
        Task<Organisation> GetByIdWithApplications(string organisationId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Organisation>> GetAllOrganisations(CancellationToken cancellationToken = default);
        Task<Organisation> GetOrganisationDetailsByOrgId(string organisationId,
            CancellationToken cancellationToken = default);
    }
}
