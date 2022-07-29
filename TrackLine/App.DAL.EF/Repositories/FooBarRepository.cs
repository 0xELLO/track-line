using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class FooBarRepository : BaseEntityRepository<App.DAL.DTO.FooBar, App.Domain.FooBar, AppDbContext>, IFooBarRepository
{
    public FooBarRepository(AppDbContext dbContext, IMapper<App.DAL.DTO.FooBar, App.Domain.FooBar> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<FooBar?>> GetAllByNameAsync(string? name, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        return (await query.Where(a => a.Name.ToUpper() == name)
            .ToListAsync()).Select(x => Mapper.Map(x));
    }

    public async Task<FooBar?> GetByNameAsync(string name, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        var a = await query.FirstAsync(a => a.Name == "a");
        return Mapper.Map(await query.Where(a => a.Name == name).FirstOrDefaultAsync());
    }
}