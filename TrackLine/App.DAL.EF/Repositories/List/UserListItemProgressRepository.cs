using App.Contracts.DAL.Repositories.List;
using Base.Contracts.Base;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories.List;

public class UserListItemProgressRepository: BaseEntityRepository<App.DAL.DTO.List.UserListItemProgressDTO,
    App.Domain.List.UserListItemProgress, AppDbContext>, IUserListItemProgressRepository
{
    public UserListItemProgressRepository(AppDbContext dbContext,
        IMapper<App.DAL.DTO.List.UserListItemProgressDTO, App.Domain.List.UserListItemProgress> mapper) : base(
        dbContext, mapper)
    {
    }
}