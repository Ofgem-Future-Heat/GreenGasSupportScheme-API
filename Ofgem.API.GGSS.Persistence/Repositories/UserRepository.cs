using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Domain.Commands.Users;
using Ofgem.API.GGSS.Persistence.Contracts;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ofgem.API.GGSS.Persistence.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IDbContextFactory factory) : base(factory) { }

        public async Task<User> GetByProviderIdAsync(string providerId, CancellationToken token = default)
        {
            var resultAsync = await _factory.CreateApplicationContextAsync(token)
                .ContinueWith(async ctxAsync =>
                {
                    var ctx = await ctxAsync;
                    return ctx.Users.FirstOrDefault(u => u.ProviderId == providerId);
                },
                token);

            return await resultAsync;
        }

        public async Task<bool> UserIsRegisteredAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var resultAsync = await _factory.CreateApplicationContextAsync(cancellationToken)
                .ContinueWith(async ctxAsync =>
                {
                    var ctx = await ctxAsync;
                    return ctx.Users.Any(u => u.Id == userId && u.ProviderId != null);
                }, 
                cancellationToken);

            return await resultAsync;
        }


    }
}
