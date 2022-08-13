using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class FooBarService: BaseEntityService<App.BLL.DTO.FooBar, App.DAL.DTO.FooBar, IFooBarRepository>,
IFooBarService
{
    public FooBarService(IFooBarRepository repository, IMapper<App.BLL.DTO.FooBar, App.DAL.DTO.FooBar> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<FooBar?>> GetAllByNameAsync(string name, bool noTracking = true)
    {
        return (await Repository.GetAllByNameAsync(name, noTracking)).Select(x => Mapper.Map(x));

    }

    public async Task<FooBar?> GetByNameAsync(string name, bool noTracking = true)
    {
        return Mapper.Map(await Repository.GetByNameAsync(name, noTracking));
    }
}