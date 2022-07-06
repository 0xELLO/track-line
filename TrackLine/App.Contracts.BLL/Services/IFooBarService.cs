using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IFooBarService : IEntityService<App.BLL.DTO.FooBar>, IFooBarRepositoryCustom<App.BLL.DTO.FooBar>
{
    
}