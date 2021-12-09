using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Domain.Commands.Users;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ofgem.API.GGSS.Application.Contracts.Persistence
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        Task<User> GetByProviderIdAsync(string providerId, CancellationToken token = default);
        Task<bool> UserIsRegisteredAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
