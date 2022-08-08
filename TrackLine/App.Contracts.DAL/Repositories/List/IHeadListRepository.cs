using App.DAL.DTO.List;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories.List;

public interface IHeadListRepository: IEntityRepository<App.DAL.DTO.List.HeadListDTO>, IHeadListRepositoryCustom<HeadListDTO>
{
    
}

public interface IHeadListRepositoryCustom<TEntity>
{
    public Task<IEnumerable<TEntity>> GetAllByUserId(Guid appUserId, bool noTracking = true);
}