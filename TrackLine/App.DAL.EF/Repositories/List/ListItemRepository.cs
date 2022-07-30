using App.Contracts.DAL.Repositories.List;
using App.DAL.DTO.List;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.List;

public class ListItemRepository: BaseEntityRepository<App.DAL.DTO.List.ListItemDTO,
    App.Domain.List.ListItem, AppDbContext>, IListItemRepository
{
    public ListItemRepository(AppDbContext dbContext,
        IMapper<App.DAL.DTO.List.ListItemDTO, App.Domain.List.ListItem> mapper) : base(dbContext, mapper)
    {
    }

    public Task<bool> Exists(string code, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        return Task.FromResult(query.Any(a => a.Code == code));
    }

    public async Task<ListItemDTO?> GetByCode(string code, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        return (Mapper.Map(await query.FirstAsync(a => a.Code == code)));
    }

    public async Task<ListItemDTO?> GetById(string id, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        return (Mapper.Map(await query.FirstAsync(x => x.Id.ToString().ToUpper() == id.ToUpper())));
    }
}