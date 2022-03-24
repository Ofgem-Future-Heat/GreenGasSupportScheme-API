using Microsoft.EntityFrameworkCore;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ofgem.API.GGSS.Persistence.Repositories
{
    public class ApplicationRepository : BaseRepository<Application.Entities.Application>, IApplicationRepository
    {
        public ApplicationRepository(IDbContextFactory factory) : base(factory){ }

        public async Task<IReadOnlyList<Application.Entities.Application>> ListAllForUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var dataAsync = await _factory.CreateApplicationContextAsync(cancellationToken)
                .ContinueWith(async ctxAsync =>
                {
                    var ctx = await ctxAsync;
                    return await ctx.Applications.Include(o => o.Organisation).Where(a => a.Organisation.ResponsiblePeople.Any(p => p.User.ProviderId == userId.ToString())).ToListAsync(cancellationToken);
                },
                cancellationToken);

            return await dataAsync;
        }

        public async Task<IReadOnlyList<Application.Entities.Application>> ListAllForUserWithDocumentsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var dataAsync = await _factory.CreateApplicationContextAsync(cancellationToken)
                .ContinueWith(async ctxAsync =>
                {
                    var ctx = await ctxAsync;
                    return await ctx.Applications.Include(a => a.Organisation).Include(a => a.Documents).Where(a => a.Organisation.ResponsiblePeople.Any(p => p.User.ProviderId == userId.ToString())).ToListAsync(cancellationToken);
                },
                cancellationToken);

            return await dataAsync;
        }

        public async Task<IReadOnlyList<Application.Entities.Application>> ListAll(CancellationToken cancellationToken = default)
        {
            var dataAsync = await _factory.CreateApplicationContextAsync(cancellationToken)
                .ContinueWith(async ctxAsync =>
                {
                    var ctx = await ctxAsync;
                    return await ctx.Applications.Include(a => a.Organisation).ToListAsync(cancellationToken);
                },
                cancellationToken);

            return await dataAsync;
        }

        public async Task<Application.Entities.Application> GetById(Guid applicationId, CancellationToken cancellationToken = default)
        {
            var context = await _factory.CreateApplicationContextAsync(cancellationToken);
            return await context.Applications.Include(a => a.Organisation)
                .ThenInclude(o => o.ResponsiblePeople)
                .SingleOrDefaultAsync(a => a.Id == applicationId);
        }
    }
}
