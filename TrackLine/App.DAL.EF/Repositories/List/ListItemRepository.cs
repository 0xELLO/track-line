using App.Contracts.DAL.Repositories.List;
using Base.Contracts.Base;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories.List;

public class ListItemRepository: BaseEntityRepository<App.DAL.DTO.List.ListItemDTO,
    App.Domain.List.ListItem, AppDbContext>, IListItemRepository
{
    public ListItemRepository(AppDbContext dbContext,
        IMapper<App.DAL.DTO.List.ListItemDTO, App.Domain.List.ListItem> mapper) : base(dbContext, mapper)
    {
    }
}