using Ofgem.API.GGSS.Application.Entities;
using System;

namespace Ofgem.API.GGSS.Persistence.Extensions
{
    public static class EntityExtensions
    {
        public static IDbEntity ToDbEntity<TEntity>(this TEntity entity)
            where TEntity : class, new()
        {
            var type = Type.GetType($"Entities.{entity.GetType().Name}, Ofgem.API.GGSS.Persistence");
            var dbEntity = Activator.CreateInstance(type);
            return dbEntity as IDbEntity;
        }
    }
}
