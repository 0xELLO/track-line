using App.Contracts.DAL.Repositories.Identity;
using App.DAL.DTO.Identity;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.Identity;

public class RefreshTokenRepository : BaseEntityRepository<App.DAL.DTO.Identity.RefreshTokenDTO, App.Domain.Identity.RefreshToken, AppDbContext>,
    IRefreshTokenRepository
{
    public RefreshTokenRepository(AppDbContext dbContext, IMapper<App.DAL.DTO.Identity.RefreshTokenDTO, App.Domain.Identity.RefreshToken> mapper) : base(
        dbContext, mapper)
    {
    }

    public async Task<IEnumerable<RefreshTokenDTO?>> GetRefreshTokensByUserIdAsync(string appUserId,
        bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        return (await query.Where(a => a.AppUserId.ToString() == appUserId)
            .ToListAsync()).Select(x => Mapper.Map(x));
        
        
        // List Refresh => refresh appUserId == appUserId
    }
}