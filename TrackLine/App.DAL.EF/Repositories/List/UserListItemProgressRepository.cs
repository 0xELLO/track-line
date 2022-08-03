using App.Contracts.DAL.Repositories.List;
using App.DAL.DTO.List;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.List;

public class UserListItemProgressRepository: BaseEntityRepository<App.DAL.DTO.List.UserListItemProgressDTO,
    App.Domain.List.UserListItemProgress, AppDbContext>, IUserListItemProgressRepository
{
    public UserListItemProgressRepository(AppDbContext dbContext,
        IMapper<App.DAL.DTO.List.UserListItemProgressDTO, App.Domain.List.UserListItemProgress> mapper) : base(
        dbContext, mapper)
    {
    }
    
    public async Task<UserListItemProgressDTO> GetByListObjectId(Guid listObjectId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        //var a = await query.FirstAsync(a => a.ListObjectId.ToString() == listObjectId);
        return Mapper.Map(await query.FirstAsync(a => a.ListObjectId == listObjectId))!;
    }
}