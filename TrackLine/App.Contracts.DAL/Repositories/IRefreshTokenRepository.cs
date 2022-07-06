using App.DAL.DTO;
using App.DAL.DTO.Identity;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IRefreshTokenRepository : IEntityRepository<App.DAL.DTO.Identity.RefreshToken>, IRefreshTokenRepositoryCustom<RefreshToken>
{
    
}

public interface IRefreshTokenRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity?>> GetRefreshTokensByUserIdAsync(string appUserId, bool noTracking = true);
}