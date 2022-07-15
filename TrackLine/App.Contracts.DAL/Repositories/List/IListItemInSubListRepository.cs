using App.DAL.DTO.List;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories.List;

public interface IListItemInSubListRepository: IEntityRepository<App.DAL.DTO.List.ListItemInSubListDTO>, IListItemInSubListRepositoryCustom<ListItemInSubListDTO>
{
    
}

public interface IListItemInSubListRepositoryCustom<TEntity>
{
}