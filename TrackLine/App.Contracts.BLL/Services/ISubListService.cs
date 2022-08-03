using App.BLL.DTO.List;
using App.Contracts.DAL.Repositories.List;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface ISubListService: IEntityService<App.BLL.DTO.List.SubListDTO>, ISubListRepositoryCustom<App.BLL.DTO.List.SubListDTO>
{
    public Task<IEnumerable<SubListDTO>> GenerateDefaultSubLists (Guid headListId, bool noTracking = true);
}