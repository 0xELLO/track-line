using App.BLL.DTO.Identity;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using App.Contracts.DAL.Repositories.Identity;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class RefreshTokenService : BaseEntityService<App.BLL.DTO.Identity.RefreshTokenDTO,
        App.DAL.DTO.Identity.RefreshTokenDTO, IRefreshTokenRepository>,
    IRefreshTokenService
{
    public RefreshTokenService(IRefreshTokenRepository repository, IMapper<App.BLL.DTO.Identity.RefreshTokenDTO, App.DAL.DTO.Identity.RefreshTokenDTO> mapper) :
        base(repository, mapper)
    {
    }
    /// <summary>
    /// Gets all AppUser Refresh tokens
    /// </summary>
    /// <param name="appUserId"></param>
    /// <param name="noTracking"></param>
    /// <returns></returns>
    public async Task<IEnumerable<RefreshTokenDTO?>> GetRefreshTokensByUserIdAsync(Guid appUserId, bool noTracking = true)
    {
        return (await Repository.GetRefreshTokensByUserIdAsync(appUserId)).Select(x => Mapper.Map(x));
    }
    
    /// <summary>
    /// Removes invalid/expired refresh tokens and return all valid
    /// </summary>
    /// <param name="appUserId"></param>
    /// <param name="noTracking"></param>
    /// <returns></returns>
    public async Task<IEnumerable<RefreshTokenDTO>> GetValidRefreshTokensByUserIdAsync(Guid appUserId,
        bool noTracking = true)
    {
        await RemoveInvalidUserTokensAsync(appUserId, noTracking);
        return (await GetRefreshTokensByUserIdAsync(appUserId, noTracking))!;
    }
    
    // TODO May be not neccecery/duplicated by other methods
    /// <summary>
    /// Gets only valid tokens than match old token
    /// </summary>
    /// <param name="appUserId"></param>
    /// <param name="refreshToken"></param>
    /// <param name="noTracking"></param>
    /// <returns></returns>
    public async Task<IEnumerable<RefreshTokenDTO>> GetValidRefreshTokensByUserIdAsync(Guid appUserId, string refreshToken,
        bool noTracking = true)
    {
        return (await Repository.GetRefreshTokensByUserIdAsync(appUserId, noTracking))
            .Where(x =>
                        (x!.Token == refreshToken && x.ExpirationTime > DateTime.UtcNow) ||
                        (x!.PreviousToken == refreshToken && x.PreviousExpirationTime > DateTime.UtcNow))
            .Select(x => Mapper.Map(x))!;
    }
    
    /// <summary>
    /// Generates new refresh Token 
    /// </summary>
    /// <param name="appUserId"></param>
    /// <param name="noTracking"></param>
    /// <returns></returns>
    public async Task<RefreshTokenDTO> GenerateRefreshToken(Guid appUserId, bool noTracking = true)
    {
        var refreshToken = new DAL.DTO.Identity.RefreshTokenDTO();
        refreshToken.AppUserId = appUserId;
        var res = Repository.Add(refreshToken);
        return Mapper.Map(res)!;
    }
    
    /// <summary>
    /// Removes all invalid refresh Tokens
    /// </summary>
    /// <param name="appUserId"></param>
    /// <param name="noTracking"></param>
    public async Task RemoveInvalidUserTokensAsync(Guid appUserId, bool noTracking = true)
    {
        // TODO Will removing all invalid tokens removes posability to logging with multiple devices
        var tokens = await Repository.GetRefreshTokensByUserIdAsync(appUserId, noTracking);
        var invalidTokens = tokens.Where(t => t != null
                                              && t.ExpirationTime < DateTime.UtcNow &&
                                              t.PreviousExpirationTime < DateTime.UtcNow);
        foreach (var invalidToken in invalidTokens)
        {
            await Repository.RemoveAsync(invalidToken!.Id);
        }
    }
}