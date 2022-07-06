using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using Base.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApp;

public static class AppDataHelper
{
    /// <summary>
    /// Sets app Default operations and data for database (manage from appsettings) 
    /// </summary>
    public static void SetupAppData(IApplicationBuilder app, IWebHostEnvironment enc, IConfiguration configuration)
    {
        using var serviceScope = app
            .ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();

        using var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

        if (context == null)
        {
            throw new ApplicationException("problem in services no db context");
        }
        
        if (context.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory") return;

        // TODO - check data base state
        // can't connect
        // can't connect wrong user 
        // can connect - but no database
        // can connect - but no user

        if (configuration.GetValue<bool>("DataInitialization:DropDatabase"))
        {
            context.Database.EnsureDeleted();
        }

        if (configuration.GetValue<bool>("DataInitialization:MigrateDatabase"))
        {
            context.Database.Migrate();
        }
        
        // Seeds default Identities, Roles (admin, user, etc)
        if (configuration.GetValue<bool>("DataInitialization:SeedIdentity"))
        {
            using var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();
            using var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<AppRole>>();

            if (userManager == null || roleManager == null)
            {
                throw new NullReferenceException("user manager or role manager is null!!");
            }

            var roles = new string[]
            {
                AppUserRoles.Admin.ToString(),
                AppUserRoles.User.ToString()
            };

            foreach (var roleInfo in roles)
            {
                var role = roleManager.FindByNameAsync(roleInfo).Result;
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

            var users = new (string username, string password, string roles)[]
            {
                ("admin@itcollege.ee", "Password.1", AppUserRoles.Admin.ToString() + "," + AppUserRoles.User.ToString()),
                ("user@itcollege.ee", "Password.1", AppUserRoles.User.ToString()),
                ("newuser@itcollege.ee", "Password.1", ""),
            };

            foreach (var userInfo in users)
            {
                var user = userManager.FindByEmailAsync(userInfo.username).Result;
                if (user == null)
                {
                    user = new AppUser()
                    {
                        Email = userInfo.username,
                        UserName = userInfo.username,
                        EmailConfirmed = true
                    };
                    var identityResult = userManager.CreateAsync(user, userInfo.password).Result;
                    if (!identityResult.Succeeded)
                    {
                        throw new ApplicationException("Cannot create user!");
                    }
                }

                if (!string.IsNullOrWhiteSpace(userInfo.roles))
                {
                    var identityResultRole = userManager.AddToRolesAsync(user, userInfo.roles.Split(",")
                            .Select(r => r.Trim()))
                        .Result;
                }
            }

        }
        // Seeds default data
        if (configuration.GetValue<bool>("DataInitialization:SeedData"))
        {
            
            context.SaveChanges();
            
        }
    }
}