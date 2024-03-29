﻿using App.BLL.DTO.Identity;
using App.Contracts.DAL.Repositories;
using App.Contracts.DAL.Repositories.Identity;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IRefreshTokenService  : IEntityService<App.BLL.DTO.Identity.RefreshTokenDTO>,
    IRefreshTokenRepositoryCustom<App.BLL.DTO.Identity.RefreshTokenDTO>
{
    public Task<IEnumerable<RefreshTokenDTO>> GetValidRefreshTokensByUserIdAsync(Guid appUserId,
        bool noTracking = true);
    public Task<IEnumerable<RefreshTokenDTO>> GetValidRefreshTokensByUserIdAsync(Guid appUserId, string refreshToken,
        bool noTracking = true);

    public Task<RefreshTokenDTO> GenerateRefreshToken(Guid appUserId, bool noTracking = true);

}