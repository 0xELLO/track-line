using App.BLL.DTO.Identity;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class RefreshTokenService : BaseEntityService<App.BLL.DTO.Identity.RefreshToken,
        App.DAL.DTO.Identity.RefreshToken, IRefreshTokenRepository>,
    IRefreshTokenService
{
    public RefreshTokenService(IRefreshTokenRepository repository, IMapper<App.BLL.DTO.Identity.RefreshToken, App.DAL.DTO.Identity.RefreshToken> mapper) :
        base(repository, mapper)
    {
    }

    public async Task<IEnumerable<RefreshToken?>> GetRefreshTokensByUserIdAsync(string appUserId, bool noTracking = true)
    {
        return (await Repository.GetRefreshTokensByUserIdAsync(appUserId)).Select(x => Mapper.Map(x));
    }

    public async Task<IEnumerable<RefreshToken>> GetValidRefreshTokensByUserIdAsync(string appUserId,
        bool noTracking = true)
    {
        await RemoveInvalidUserTokensAsync(appUserId, noTracking);
        return (await GetRefreshTokensByUserIdAsync(appUserId, noTracking))!;
    }

    public async Task RemoveInvalidUserTokensAsync(string appUserId, bool noTracking = true)
    {
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