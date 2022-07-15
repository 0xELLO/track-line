using App.Contracts.DAL.Repositories.List;
using Base.Contracts.Base;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories.List;

public class ListItemInSubListRepository: BaseEntityRepository<App.DAL.DTO.List.ListItemInSubListDTO,
        App.Domain.List.ListItemInSubList, AppDbContext>, IListItemInSubListRepository
{
    public ListItemInSubListRepository(AppDbContext dbContext,
        IMapper<App.DAL.DTO.List.ListItemInSubListDTO, App.Domain.List.ListItemInSubList> mapper) : base(
        dbContext, mapper)
    {
    }
}