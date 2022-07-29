using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace App.Contracts.BLL.Services;

public interface IAppUserService
{
    public Task<AppUser?> GetUserFromJwt(string authorization, UserManager<AppUser> userManager);
}