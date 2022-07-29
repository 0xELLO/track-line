using App.Contracts.DAL.Repositories;
using App.Contracts.DAL.Repositories.Identity;
using App.Contracts.DAL.Repositories.List;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUnitOfWork : IUnitOfWork
{
    IHeadListRepository HeadListRepository { get; }
    IListItemInSubListRepository ListItemInSubListRepository { get; }
    IListItemRepository ListItemRepository { get; }
    ISubListRepository SubListRepository { get; }
    IUserListItemProgressRepository UserListItemProgressRepository { get; }
    IFooBarRepository FooBarRepository { get;  }
    IRefreshTokenRepository RefreshTokenRepository { get; }
}