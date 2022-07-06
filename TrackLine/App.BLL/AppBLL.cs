using App.BLL.Mapper;
using App.BLL.Mapper.Identity;
using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
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
    
    
}

  