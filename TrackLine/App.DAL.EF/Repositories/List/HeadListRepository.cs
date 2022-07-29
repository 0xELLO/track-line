using App.Contracts.DAL.Repositories.List;
using App.DAL.DTO.List;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.List;

public class HeadListRepository : BaseEntityRepository<App.DAL.DTO.List.HeadListDTO,
        App.Domain.List.HeadList, AppDbContext>, IHeadListRepository
{
    public HeadListRepository(AppDbContext dbContext,
        IMapper<App.DAL.DTO.List.HeadListDTO, App.Domain.List.HeadList> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<HeadListDTO>> getHeadListsByUserId(string userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        return (await query.Where(a => a.AppUserId.ToString().ToUpper() == userId.ToUpper())
            .ToListAsync()).Select(x => Mapper.Map(x));
    }
}