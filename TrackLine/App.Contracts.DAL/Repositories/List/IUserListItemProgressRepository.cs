using App.DAL.DTO.List;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories.List;

public interface IUserListItemProgressRepository: IEntityRepository<App.DAL.DTO.List.UserListItemProgressDTO>, IUserListItemProgressRepositoryCustom<UserListItemProgressDTO>
{
    
}

public interface IUserListItemProgressRepositoryCustom<TEntity>
{
    public Task<TEntity> GetByListItemId(Guid listItemId, bool noTracking = true);
}