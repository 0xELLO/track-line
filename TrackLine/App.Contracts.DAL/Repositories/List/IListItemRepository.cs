using App.DAL.DTO.List;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories.List;

public interface IListItemRepository: IEntityRepository<App.DAL.DTO.List.ListItemDTO>, IListItemRepositoryCustom<ListItemDTO>
{
    
}

public interface IListItemRepositoryCustom<TEntity>
{
}