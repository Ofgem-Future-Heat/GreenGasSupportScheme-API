using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Ofgem.API.GGSS.Persistence.Contracts;
using Ofgem.API.GGSS.Application.Entities;

namespace Ofgem.API.GGSS.Persistence.Repositories
{
    public class BaseRepository<TEntity> : IAsyncRepository<TEntity> where TEntity : class, IDbEntity, new()
    {
        protected readonly IDbContextFactory _factory;

        public BaseRepository(IDbContextFactory factory)
        {
            _factory = factory;               
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            var context = await _factory.CreateApplicationContextAsync(token);

            return await context.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken: token);   
        }

        public async Task<IReadOnlyList<TEntity>> ListAllAsync(CancellationToken token = default)
        {
            var context = await _factory.CreateApplicationContextAsync(token);

            return await context.Set<TEntity>().ToListAsync(token);
        }

        public async Task<TEntity> AddAsync(TEntity entity, Guid userId = default, CancellationToken token = default)
        {
            var context = await _factory.CreateApplicationContextAsync(token);
            await context.Set<TEntity>().AddAsync(entity, token);
            await context.SaveChangesAsync(userId, token);

            return entity;
        }

        public async Task UpdateAsync(TEntity entity, Guid userId = default, CancellationToken token = default)
        {
            var context = await _factory.CreateApplicationContextAsync(token);
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync(userId, token);
        }

        public async Task DeleteAsync(TEntity entity, Guid userId = default, CancellationToken token = default)
        {
            var context = await _factory.CreateApplicationContextAsync(token);
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync(userId, token);
        }
    }
}
