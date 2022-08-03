using App.DAL.DTO.List;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories.List;

public interface ISubListRepository: IEntityRepository<App.DAL.DTO.List.SubListDTO>, ISubListRepositoryCustom<SubListDTO>
{
    
}

public interface ISubListRepositoryCustom<TEntity>
{
    public Task<IEnumerable<TEntity>> GetAllByHeadListId(Guid headListId, bool noTracking = true);

}