using App.BLL.DTO.Identity;
using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IRefreshTokenService  : IEntityService<App.BLL.DTO.Identity.RefreshToken>,
    IRefreshTokenRepositoryCustom<App.BLL.DTO.Identity.RefreshToken>,
    IRefreshTokenServiceCustom<App.BLL.DTO.Identity.RefreshToken>

{
    
}

public interface IRefreshTokenServiceCustom<TEntity>
{
    public Task<IEnumerable<RefreshToken>> GetValidRefreshTokensByUserIdAsync(string appUserId,
        bool noTracking = true);
}