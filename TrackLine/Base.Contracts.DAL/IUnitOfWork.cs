namespace Base.Contracts.DAL;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
    int SaveChanges();
}