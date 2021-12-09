namespace Ofgem.API.GGSS.Domain.Contracts.Entities
{
    public interface ISerializableEntity<TEntity> where TEntity : class, new()
    {
        TEntity Value { get; set; }
    }
}
