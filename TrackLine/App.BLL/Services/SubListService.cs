using App.BLL.DTO.List;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories.List;
using Base.BLL;
using Base.Common;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class SubListService : BaseEntityService<App.BLL.DTO.List.SubListDTO, App.DAL.DTO.List.SubListDTO, ISubListRepository>,
    ISubListService
{
    public SubListService(ISubListRepository repository, IMapper<SubListDTO, DAL.DTO.List.SubListDTO> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<SubListDTO>> GetAllByHeadListId(Guid headListId, bool noTracking = true)
    {
        // TODO handle null
        return (await Repository.GetAllByHeadListId(headListId, noTracking)).Select(x => Mapper.Map(x));
    }

    public async Task<IEnumerable<SubListDTO>> GenerateDefaultSubLists(Guid headListId, bool noTracking = true)
    {
        var res = new List<SubListDTO>();
        foreach (var title in DefaultTitles.SubListTitles)
        {
            res.Add(Mapper.Map(Repository.Add(new DAL.DTO.List.SubListDTO
            {
                HeadListId = headListId,
                DefaultTitle = title,
                IsPublic = false
            }))!);
        }
        return res;
    }

}