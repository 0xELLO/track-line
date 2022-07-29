using System.Collections;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.Contracts.DAL.Repositories.Identity;
using App.Contracts.DAL.Repositories.List;
using App.DAL.EF.Mappers;
using App.DAL.EF.Mappers.Identity;
using App.DAL.EF.Mappers.List;
using App.DAL.EF.Repositories;
using App.DAL.EF.Repositories.Identity;
using App.DAL.EF.Repositories.List;
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
    
    
    
    private IHeadListRepository? _headListRepository;
    public virtual IHeadListRepository HeadListRepository => _headListRepository ??= new HeadListRepository(UOWDbContext, new HeadListMapper(_mapper));


    private IListItemInSubListRepository? _listItemInSubListRepository;
    public virtual IListItemInSubListRepository ListItemInSubListRepository => _listItemInSubListRepository ??= new ListItemInSubListRepository(UOWDbContext, new ListItemInSubListMapper(_mapper));
    
        
    private IListItemRepository? _listItemRepository;
    public virtual IListItemRepository ListItemRepository => _listItemRepository ??= new ListItemRepository(UOWDbContext, new ListItemMapper(_mapper));
    
        
    private ISubListRepository? _subListRepository;
    public virtual ISubListRepository SubListRepository => _subListRepository ??= new SubListRepository(UOWDbContext, new SubListMapper(_mapper));
    
        
    private IUserListItemProgressRepository? _userListItemProgressRepository;
    public virtual IUserListItemProgressRepository UserListItemProgressRepository => _userListItemProgressRepository ??= new UserListItemProgressRepository(UOWDbContext, new UserListItemProgressMapper(_mapper));
}