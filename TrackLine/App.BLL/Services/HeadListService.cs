using App.BLL.DTO.List;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using App.Contracts.DAL.Repositories.List;
using Base.BLL;
using Base.Common;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class HeadListService : BaseEntityService<App.BLL.DTO.List.HeadListDTO, App.DAL.DTO.List.HeadListDTO, IHeadListRepository>,
    IHeadListService
{
    public HeadListService(IHeadListRepository repository, IMapper<HeadListDTO, DAL.DTO.List.HeadListDTO> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<HeadListDTO>> GetAllByUserId(Guid appUserId, bool noTracking = true)
    {
        // TODO handle null
        return (await Repository.GetAllByUserId(appUserId, noTracking)).Select(x => Mapper.Map(x));
    }
    
    /// <summary>
    /// Generates default headLists for new AppUser
    /// </summary>
    /// <param name="appUserId"></param>
    /// <param name="noTracking"></param>
    /// <returns></returns>
    public async Task<IEnumerable<HeadListDTO>> GenerateDefaultHeadLists(Guid appUserId, bool noTracking = true)
    {
        var result = new List<HeadListDTO>();
        foreach (var title in DefaultTitles.HeadListTitles)
        {
            result.Add(Mapper.Map(Repository.Add(new DAL.DTO.List.HeadListDTO
            {
                AppUserId = appUserId,
                DefaultTitle = title
            }))!);
        }

        return result;
    }
}