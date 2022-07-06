using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IFooBarRepository : IEntityRepository<App.DAL.DTO.FooBar>, IFooBarRepositoryCustom<FooBar>
{
    
}

public interface IFooBarRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity?>> GetAllByNameAsync(string name, bool noTracking = true);
    Task<TEntity?> GetByNameAsync(string name, bool noTracking = true);
}
