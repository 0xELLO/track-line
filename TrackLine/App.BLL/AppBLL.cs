using App.BLL.Mapper;
using App.BLL.Mapper.Identity;
using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.List;
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
    public IFooBarService FooBarService => _fooBarService ??= new FooBarService(UnitOfWork.IFooBarRepository, new FooBarMapper(_mapper));
    
    
    
    private IRefreshTokenService? _refreshTokenService;
    public IRefreshTokenService RefreshTokenService => _refreshTokenService ??= new RefreshTokenService(UnitOfWork.IRefreshTokenRepository, new RefreshTokenMapper(_mapper));
    
    
    private IListItemService? _listItemService;
    public IListItemService ListItemService => _listItemService ??= new ListItemService(UnitOfWork.IListItemRepository,
        UnitOfWork.IListItemInSubListRepository, UnitOfWork.IUserListItemProgressRepository);
    
    
}

  