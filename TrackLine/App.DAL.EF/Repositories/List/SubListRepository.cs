using App.Contracts.DAL.Repositories.Identity;
using App.Contracts.DAL.Repositories.List;
using Base.Contracts.Base;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories.List;

public class SubListRepository : BaseEntityRepository<App.DAL.DTO.List.SubListDTO,
        App.Domain.List.SubList, AppDbContext>, ISubListRepository
{
    public SubListRepository(AppDbContext dbContext,
        IMapper<App.DAL.DTO.List.SubListDTO, App.Domain.List.SubList> mapper) : base(
        dbContext, mapper)
    {
    }
}