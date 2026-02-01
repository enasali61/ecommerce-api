

using System.Text.Json;
using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Entities.Order_Entity;
using Microsoft.AspNetCore.Identity;

namespace Presistance.Data
{
    public class DbIntializer : IDbIntializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public DbIntializer(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task IntializeAsync()
        {
            // create db if not exicted & applinany pending migration
            try { 
                if(_dbContext.Database.GetPendingMigrations().Any()) 
                        await _dbContext.Database.MigrateAsync();
                
                //apply data seeding 
                if (!_dbContext.ProductTypes.Any())
                {
                    // read types from files as string
                    //F:\.net course\c#\Ecommerse2\Infrastructure\Presistance\Data\DataSeeding\types.json
                    var typesData = await File.ReadAllTextAsync(@"..\Infrastructure\Presistance\Data\DataSeeding\types.json");
                    // transform into c# object 
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    // add to db & save changes 
                    if (types is not null && types.Any())
                    {
                       await _dbContext.ProductTypes.AddRangeAsync(types);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                if (!_dbContext.ProductBrands.Any())
                {
                    // read types from files as string

                    var brandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Presistance\Data\DataSeeding\brands.json");
                    // transform into c# object 
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    // add to db & save changes 
                    if (brands is not null && brands.Any())
                    {
                        await _dbContext.ProductBrands.AddRangeAsync(brands);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                if (!_dbContext.Products.Any())
                {
                    // read types from files as string

                    var productsData = await File.ReadAllTextAsync(@"..\Infrastructure\Presistance\Data\DataSeeding\products.json");
                    // transform into c# object 
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    // add to db & save changes 
                    if (products is not null && products.Any())
                    {
                        await _dbContext.Products.AddRangeAsync(products);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                if (!_dbContext.deliveryMethods.Any())
                {
                    // read types from files as string

                    var methodsData = await File.ReadAllTextAsync(@"..\Infrastructure\Presistance\Data\DataSeeding\delivery.json");
                    // transform into c# object 
                    var methods = JsonSerializer.Deserialize<List<DeliveryMethods>>(methodsData);
                    // add to db & save changes 
                    if (methods is not null && methods.Any())
                    {
                        await _dbContext.deliveryMethods.AddRangeAsync(methods);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            } 
            catch(Exception) { throw; } 

        }

        public async Task IntializeIdentityAsync()
        {
            // seed default user and role 

            // 1.seed roles if it does not contain any
            if (!_roleManager.Roles.Any())
            {
                // Admin and superAdmin
              await  _roleManager.CreateAsync(new IdentityRole("Admin"));
              await  _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));

            }

            // 2. assign user => roles
            if (!_userManager.Users.Any())
            {
                var AdminUser = new User
                {
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "01223456443",
                };
                var SuperAdminUser = new User
                {
                    DisplayName = "Super Admin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "01223456443",
                };

               await _userManager.CreateAsync(AdminUser, "Passw0rd");
               await _userManager.CreateAsync(SuperAdminUser, "Passw0rd");
              
                await _userManager.AddToRoleAsync(AdminUser, "Admin");
               await _userManager.AddToRoleAsync(SuperAdminUser, "SuperAdmin");

            }
        }
    }
}
