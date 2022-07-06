using Base.Contracts.Domain;

namespace Base.Contracts.DAL;

/// <summary>
/// Default Guid based Repository interface
/// </summary>
/// <typeparam name="TEntity">Domain entity</typeparam>
public interface IEntityRepository<TEntity>: IEntityRepository<TEntity, Guid>
where TEntity: class, IDomainEntityId
{
}

/// <summary>
/// Describes repository implementation. All methods blow must be implemented in every repository.
/// </summary>
/// <typeparam name="TEntity">Domain entity</typeparam>
/// <typeparam name="TKey">Generic PK type</typeparam>
// TODO check for user ownership on read and update methods except (create?)
public interface IEntityRepository<TEntity, TKey>
where TEntity: class, IDomainEntityId<TKey>
where TKey: IEquatable<TKey>
{
    // sync
    TEntity Add(TEntity entity);
    TEntity Update(TEntity entity);
    TEntity Remove(TEntity entity);
    TEntity Remove(TKey id);

    TEntity? FirstOrDefault(TKey id, bool noTracking = true);
    IEnumerable<TEntity> GetAll(bool noTracking = true);
    bool Exists(TKey id);
    
    // async
    Task<TEntity?> FirstOrDefaultAsync(TKey id, bool noTracking = true);
    Task<IEnumerable<TEntity>> GetAllAsync(bool noTracking = true);
    Task<bool> ExistsAsync(TKey id);
    Task<TEntity> RemoveAsync(TKey id);
}