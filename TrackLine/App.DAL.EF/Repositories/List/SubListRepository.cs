using App.Contracts.DAL.Repositories.Identity;
using App.Contracts.DAL.Repositories.List;
using App.DAL.DTO.List;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.List;

public class SubListRepository : BaseEntityRepository<App.DAL.DTO.List.SubListDTO,
        App.Domain.List.SubList, AppDbContext>, ISubListRepository
{
    public SubListRepository(AppDbContext dbContext,
        IMapper<App.DAL.DTO.List.SubListDTO, App.Domain.List.SubList> mapper) : base(
        dbContext, mapper)
    {
    }

    public async Task<IEnumerable<SubListDTO>> getSubListsByHeadListId(string headListId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        return (await query.Where(a => a.HeadListId.ToString().ToUpper() == headListId.ToUpper())
            .ToListAsync()).Select(x => Mapper.Map(x));
    }
}