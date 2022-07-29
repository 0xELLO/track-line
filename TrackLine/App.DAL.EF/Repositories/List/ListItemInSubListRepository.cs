using App.Contracts.DAL.Repositories.List;
using App.DAL.DTO.List;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.List;

public class ListItemInSubListRepository: BaseEntityRepository<App.DAL.DTO.List.ListItemInSubListDTO,
        App.Domain.List.ListItemInSubList, AppDbContext>, IListItemInSubListRepository
{
    public ListItemInSubListRepository(AppDbContext dbContext,
        IMapper<App.DAL.DTO.List.ListItemInSubListDTO, App.Domain.List.ListItemInSubList> mapper) : base(
        dbContext, mapper)
    {
    }

    // TODO null
    public async Task<IEnumerable<ListItemInSubListDTO>> GetAllBySubListId(string subListId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        return (await query.Where(a => a.SubListId.ToString() == subListId)
            .ToListAsync()).Select(x => Mapper.Map(x));
    }
}