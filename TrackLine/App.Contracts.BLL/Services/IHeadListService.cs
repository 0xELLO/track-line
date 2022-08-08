using App.BLL.DTO.List;
using App.Contracts.DAL.Repositories;
using App.Contracts.DAL.Repositories.List;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IHeadListService: IEntityService<App.BLL.DTO.List.HeadListDTO>, IHeadListRepositoryCustom<App.BLL.DTO.List.HeadListDTO>
{
    public Task<IEnumerable<HeadListDTO>> GenerateDefaultHeadLists(Guid appUserId, bool noTracking = true);
}