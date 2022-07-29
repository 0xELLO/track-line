using App.BLL.DTO.List;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories.List;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class SubListService : BaseEntityService<App.BLL.DTO.List.SubListDTO, App.DAL.DTO.List.SubListDTO, ISubListRepository>,
    ISubListService
{
    public SubListService(ISubListRepository repository, IMapper<SubListDTO, DAL.DTO.List.SubListDTO> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<SubListDTO>> getSubListsByHeadListId(string headListId, bool noTracking = true)
    {
        // TODO handle null
        return (await Repository.getSubListsByHeadListId(headListId, noTracking)).Select(x => Mapper.Map(x));
    }
}