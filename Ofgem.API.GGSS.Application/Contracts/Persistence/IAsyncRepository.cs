using Ofgem.API.GGSS.Application.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ofgem.API.GGSS.Application.Contracts.Persistence
{
    public interface IAsyncRepository<T> where T : class, IDbEntity, new()
    {
        Task<T> GetByIdAsync(Guid id, CancellationToken token = default);
        Task<IReadOnlyList<T>> ListAllAsync(CancellationToken token = default);
        Task<T> AddAsync(T entity, Guid userId = default, CancellationToken token = default);
        Task UpdateAsync(T entity, Guid userId = default, CancellationToken token = default);
        Task DeleteAsync(T entity, Guid userId = default, CancellationToken token = default);
    }
}
