using System.Text;
using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Presistance.Data;
using Presistance.Identity;
using Presistance.Repositories;
using Shared;
using StackExchange.Redis;

namespace Ecommerse.Api.Extentions
{
    public static class InfrastructureServiceExtentions
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddDbContext<IdentityAppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ICachRepository, CachRepository>();

            services.AddScoped<IDbIntializer, DbIntializer>();

            services.AddSingleton<IConnectionMultiplexer>(
                services => ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!)
                );
            services.ConfigureIdentityService();
            services.ConfigureJwt(configuration);
            return services;
        }

        public static IServiceCollection ConfigureIdentityService(
            this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(option =>
           {
               option.Password.RequireDigit = true;
               option.Password.RequireLowercase = false;
               option.Password.RequireUppercase = false;
               option.Password.RequireNonAlphanumeric = false;
               option.Password.RequiredLength = 8;

               option.User.RequireUniqueEmail= true;

           }).AddEntityFrameworkStores<IdentityAppDbContext>();
            return services;
        }

        public static IServiceCollection ConfigureJwt(this IServiceCollection services,IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();
            // validate on token
           
            services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                }).AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        // values
                        ValidAudience = jwtOptions.Audience,
                        ValidIssuer = jwtOptions.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))

                    };
                });
            services.AddAuthorization();
            return services;
        } 
    }
}
