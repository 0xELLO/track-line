using System.Collections;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.EF.Mappers;
using App.DAL.EF.Mappers.Identity;
using App.DAL.EF.Repositories;
using Base.Contracts.DAL;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUOW : BaseUOW<AppDbContext>, IAppUnitOfWork
{
    private readonly AutoMapper.IMapper _mapper;

    public AppUOW(AppDbContext dbContext, AutoMapper.IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }

    private IFooBarRepository? _fooBarRepository;
    public virtual IFooBarRepository FooBarRepository => _fooBarRepository ??= new FooBarRepository(UOWDbContext, new FooBarMapper(_mapper));
    
    private IRefreshTokenRepository? _refreshTokenRepository;
    public virtual IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository ??= new RefreshTokenRepository(UOWDbContext, new RefreshTokenMapper(_mapper));
}