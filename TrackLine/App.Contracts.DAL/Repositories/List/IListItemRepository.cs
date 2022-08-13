using App.DAL.DTO.List;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories.List;

public interface IListItemRepository: IEntityRepository<App.DAL.DTO.List.ListItemDTO>, IListItemRepositoryCustom<ListItemDTO>
{
    
}

public interface IListItemRepositoryCustom<TEntity>
{
    // True if exists
    public Task<bool> Exists(string code, bool noTracking = true);

    public Task<TEntity?> GetByCode(string code, bool noTracking = true);

    public Task<TEntity?> GetById(Guid id, bool noTracking = true);

    public Task<IEnumerable<TEntity>> GetAllPublic(bool noTracking = true);
}