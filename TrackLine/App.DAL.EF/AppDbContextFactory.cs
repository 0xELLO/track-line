using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace App.DAL.EF;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite("DataSource=app.db;Cache=Shared");
        //optionsBuilder.UseNpgsql("Host=localhost:5434;Username=postgres;Password=postgres;database=trackline");

        return new AppDbContext(optionsBuilder.Options);
    }
}