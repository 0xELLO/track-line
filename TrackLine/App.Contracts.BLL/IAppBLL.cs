using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using App.Contracts.DAL.Repositories.List;
using Base.Contracts.BLL;

namespace App.Contracts.BLL;

public interface IAppBLL : IBLL
{
    IListItemService ListItemService { get; }
    IFooBarService FooBarService { get; }
    IRefreshTokenService RefreshTokenService { get; }
    ISubListService SubListService { get; }
    IHeadListService HeadListService { get; }
    
    IAppUserService AppUserService { get; }
}