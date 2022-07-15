using App.Contracts.DAL.Repositories;
using App.Contracts.DAL.Repositories.Identity;
using App.Contracts.DAL.Repositories.List;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUnitOfWork : IUnitOfWork
{
    IHeadListRepository IHeadListRepository { get; }
    IListItemInSubListRepository IListItemInSubListRepository { get; }
    IListItemRepository IListItemRepository { get; }
    ISubListRepository ISubListRepository { get; }
    IUserListItemProgressRepository IUserListItemProgressRepository { get; }
    IFooBarRepository IFooBarRepository { get;  }
    IRefreshTokenRepository IRefreshTokenRepository { get; }
}