using App.Contracts.DAL.Repositories.List;
using Base.Contracts.Base;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories.List;

public class HeadListRepository : BaseEntityRepository<App.DAL.DTO.List.HeadListDTO,
        App.Domain.List.HeadList, AppDbContext>, IHeadListRepository
{
    public HeadListRepository(AppDbContext dbContext,
        IMapper<App.DAL.DTO.List.HeadListDTO, App.Domain.List.HeadList> mapper) : base(dbContext, mapper)
    {
    }
}