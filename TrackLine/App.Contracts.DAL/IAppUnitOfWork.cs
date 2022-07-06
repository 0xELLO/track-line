using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUnitOfWork : IUnitOfWork
{
    IFooBarRepository FooBarRepository { get;  }
    IRefreshTokenRepository RefreshTokenRepository { get; }
}