using App.BLL.DTO.List;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using App.Contracts.DAL.Repositories.List;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class HeadListService : BaseEntityService<App.BLL.DTO.List.HeadListDTO, App.DAL.DTO.List.HeadListDTO, IHeadListRepository>,
    IHeadListService
{
    public HeadListService(IHeadListRepository repository, IMapper<HeadListDTO, DAL.DTO.List.HeadListDTO> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<HeadListDTO>> getHeadListsByUserId(string userId, bool noTracking = true)
    {
        // TODO handle null
        Console.WriteLine(userId);
        return (await Repository.getHeadListsByUserId(userId, noTracking)).Select(x => Mapper.Map(x));
    }
}