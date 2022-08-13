using App.BLL.Mapper;
using App.BLL.Mapper.Identity;
using App.BLL.Mapper.LIst;
using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.List;
using App.Domain.Identity;
using AutoMapper;
using Base.BLL;

namespace App.BLL;

public class AppBLL : BaseBLL<IAppUnitOfWork>, IAppBLL
{
    protected IAppUnitOfWork UnitOfWork;
    private readonly AutoMapper.IMapper _mapper;
    
    public AppBLL(IAppUnitOfWork unitOfWork, IMapper mapper)
    {
        UnitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public override async Task<int> SaveChangesAsync()
    {
        return await UnitOfWork.SaveChangesAsync();
    }

    public override int SaveChanges()
    {
        return UnitOfWork.SaveChanges();
    }


    private IFooBarService? _fooBarService;
    public IFooBarService FooBarService => _fooBarService ??= new FooBarService(UnitOfWork.FooBarRepository, new FooBarMapper(_mapper));
    
    
    private IRefreshTokenService? _refreshTokenService;
    public IRefreshTokenService RefreshTokenService => _refreshTokenService ??= new RefreshTokenService(UnitOfWork.RefreshTokenRepository, new RefreshTokenMapper(_mapper));
    
    
    private IListItemService? _listItemService;
    public IListItemService ListItemService => _listItemService ??= new ListItemService(UnitOfWork.ListItemRepository,
        UnitOfWork.ListItemInSubListRepository, UnitOfWork.UserListItemProgressRepository, new MinimalListItemMapper(_mapper));
    
    
    private IHeadListService? _headListService;
    public IHeadListService HeadListService => _headListService ??= new HeadListService(UnitOfWork.HeadListRepository, new HeadListMapper(_mapper));
    
    
    private ISubListService? _subListService;
    public ISubListService SubListService => _subListService ??=
        new SubListService(UnitOfWork.SubListRepository, new SubListMapper(_mapper));

    private IAppUserService? _appUserService;
    public IAppUserService AppUserService => _appUserService ??= new AppUserService();
    
    
    private ISearchService? _searchService;
    public ISearchService SearchService => _searchService ??= new SearchService(UnitOfWork.ListItemRepository);

}

  