using Domain.Contracts;
using Presistance.Data;
using Services.Abstraction;
using Services;
using Shared;

namespace Ecommerse.Api.Extentions
{
    public static class CoreServicesExtentions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAutoMapper(cfg => { }, typeof(Services.AssemblyReference).Assembly);
            services.AddScoped<IServiceManager, ServiceManager>();

            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            return services;
        }

    }
}
