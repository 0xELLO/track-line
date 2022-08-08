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
    // TODO FIXED?
    
    public async Task<IEnumerable<RefreshTokenDTO?>> GetRefreshTokensByUserIdAsync(Guid appUserId, bool noTracking = true)
    {
        return (await Repository.GetRefreshTokensByUserIdAsync(appUserId)).Select(x => Mapper.Map(x));
    }

    public async Task<IEnumerable<RefreshTokenDTO>> GetValidRefreshTokensByUserIdAsync(Guid appUserId,
        bool noTracking = true)
    {
        await RemoveInvalidUserTokensAsync(appUserId, noTracking);
        return (await GetRefreshTokensByUserIdAsync(appUserId, noTracking))!;
    }
    
    public async Task<IEnumerable<RefreshTokenDTO>> GetValidRefreshTokensByUserIdAsync(Guid appUserId, string refreshToken,
        bool noTracking = true)
    {
        return (await Repository.GetRefreshTokensByUserIdAsync(appUserId, noTracking))
            .Where(x =>
                        (x!.Token == refreshToken && x.ExpirationTime > DateTime.UtcNow) ||
                        (x!.PreviousToken == refreshToken && x.PreviousExpirationTime > DateTime.UtcNow))
            .Select(x => Mapper.Map(x))!;
    }

    public async Task<RefreshTokenDTO> GenerateRefreshToken(Guid appUserId, bool noTracking = true)
    {
        var refreshToken = new DAL.DTO.Identity.RefreshTokenDTO();
        refreshToken.AppUserId = appUserId;
        var res = Repository.Add(refreshToken);
        return Mapper.Map(res)!;
    }

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