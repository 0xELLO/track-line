using App.Contracts.DAL.Repositories;
using App.Contracts.DAL.Repositories.List;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IHeadListService: IEntityService<App.BLL.DTO.List.HeadListDTO>, IHeadListRepositoryCustom<App.BLL.DTO.List.HeadListDTO>
{
}