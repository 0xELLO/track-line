using Base.Contracts.DAL;
using Base.Contracts.Domain;

namespace Base.Contracts.BLL;

public interface IEntityService<TEntity> : IEntityRepository<TEntity>, IEntityService<TEntity, Guid>
    where TEntity: class, IDomainEntityId
{
}


public interface IEntityService<TEntity, TKey> : IEntityRepository<TEntity, TKey>
    where TEntity: class, IDomainEntityId, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
{
}