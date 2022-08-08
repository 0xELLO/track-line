using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using App.Domain.List;
using Base.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Tests.WebApp;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup: class
{
    private static bool dbInitialized = false;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // find DbContext
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<AppDbContext>));

            // if found - remove
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // and new DbContext
            services.AddDbContext<AppDbContext>(options => { options.UseInMemoryDatabase("InMemoryDbForTesting"); });



            // create db and seed data
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();
            var logger = scopedServices
                .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

            db.Database.EnsureCreated();
            using var userManager = scopedServices.GetService<UserManager<AppUser>>();
            using var roleManager = scopedServices.GetService<RoleManager<AppRole>>();
            try
            {
                if (dbInitialized == false)
                {
                    dbInitialized = true;
                    var users = new (string username, string password, string roles)[]
                    {
                        ("admin@itcollege.ee", "Password.1", "user,admin"),
                        ("user@itcollege.ee", "Password.1", "user"),
                        ("newuser@itcollege.ee", "Password.1", ""),
                    };

                    foreach (var userInfo in users)
                    {
                        var user = userManager!.FindByEmailAsync(userInfo.username).Result;
                        if (user == null)
                        {
                            user = new AppUser()
                            {
                                Email = userInfo.username,
                                UserName = userInfo.username,
                                EmailConfirmed = true,
                            };
                            var identityResult = userManager.CreateAsync(user, userInfo.password).Result;

                            if (!identityResult.Succeeded)
                            {
                                throw new ApplicationException("Cannot create user!");
                            }
                        }
                    
                        if (!string.IsNullOrWhiteSpace(userInfo.roles))
                        {
                            var identityResultRole = userManager.AddToRolesAsync(user,
                                userInfo.roles.Split(",").Select(r => r.Trim())
                            ).Result;
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the " +
                                    "database with test messages. Error: {Message}", ex.Message);
            }
            
            try
            {
                
                var user = userManager!.FindByNameAsync("admin@itcollege.ee").Result;

                // create items here to inject into database (standart items to test)
                var headList = db.HeadList.Add(new HeadList
                {
                    AppUserId = user.Id,
                    DefaultTitle = "test123",
                });
                db.SaveChanges();
                
                var fooBar = db.FooBar.Add(new FooBar
                {
                    Name = "foo bar"
                });
                db.SaveChanges();
                
                var roles = new string[]
                {
                    AppUserRoles.Admin.ToString(),
                    AppUserRoles.User.ToString()
                };

                foreach (var roleInfo in roles)
                {
                    var role = roleManager!.FindByNameAsync(roleInfo).Result;
                    if (role == null)
                    {
                        var identityResult = roleManager.CreateAsync(new AppRole()
                        {
                            Name = roleInfo,
                        }).Result;

                        if (!identityResult.Succeeded)
                        {
                            throw new ApplicationException("Role creation failed");
                        }
                    }
                }
                db.SaveChanges();

            }
            catch (Exception ex)
            {

                logger.LogError(ex, "An error occurred seeding the " +
                                    "database with test messages. Error: {Message}", ex.Message);
            }
        });
    }
}