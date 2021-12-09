using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ofgem.API.GGSS.Persistence.Repositories
{
    public class OrganisationRepository : BaseRepository<Organisation>, IOrganisationRepository
    {
        public OrganisationRepository(IDbContextFactory factory) : base(factory){ }

        public async Task<string> AddWithResponsiblePersonAsync(Organisation organisation, CancellationToken cancellationToken = default)
        {
            CheckParameter(organisation);

            var result = await base.AddAsync(organisation, token: cancellationToken);

            return result.Id.ToString();
        }

        public async Task<IReadOnlyList<Organisation>> ListAllForUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            var context = await _factory.CreateApplicationContextAsync(cancellationToken);

            return await context.Organisations
                .Include(o => o.ResponsiblePeople)
                .Include(o => o.Applications)
                .Where(o => o.ResponsiblePeople.Any(r => r.UserId.ToString() == userId))
                .ToListAsync(cancellationToken);
        }

        public async Task<Organisation> GetByIdWithApplications(string organisationId, CancellationToken cancellationToken = default)
        {
            var organisationContext = await _factory.CreateApplicationContextAsync(cancellationToken);

            return await organisationContext.Organisations.Include(o => o.Applications).SingleOrDefaultAsync(o => o.Id.ToString() == organisationId, cancellationToken);
        }

        public async Task<IReadOnlyList<Organisation>> GetAllOrganisations(CancellationToken cancellationToken = default)
        {
            var organisationContext = await _factory.CreateApplicationContextAsync(cancellationToken);

            return await organisationContext.Organisations
                .Include(o => o.ResponsiblePeople)
                .ThenInclude(r => r.User)
                .Include(o => o.ResponsiblePeople)
                .ThenInclude(r=> r.Documents)
                .ToListAsync();
        }

        public async Task<Organisation> GetOrganisationDetailsByOrgId(string organisationId,
            CancellationToken cancellationToken = default)
        {
            var organisations = await GetAllOrganisations(cancellationToken);

            return organisations.SingleOrDefault(o => o.Id.ToString() == organisationId);
        }

        private void CheckParameter(Organisation organisation)
        {
            if (organisation == null)
            {
                throw new ArgumentNullException(nameof(organisation));
            }
        }
    }
}
