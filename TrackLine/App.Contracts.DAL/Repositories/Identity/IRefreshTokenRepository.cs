using App.DAL.DTO.Identity;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories.Identity;

public interface IRefreshTokenRepository : IEntityRepository<App.DAL.DTO.Identity.RefreshTokenDTO>, IRefreshTokenRepositoryCustom<RefreshTokenDTO>
{
    
}

public interface IRefreshTokenRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity?>> GetRefreshTokensByUserIdAsync(string appUserId, bool noTracking = true);
}