using Ecommerse.Api.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace Ecommerse.Api.Extentions
{
    public static class PresentationServicesExtentions
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddControllers().AddApplicationPart(typeof(Presintation.AssemplyRefrence).Assembly);

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponceFactory.CustumValidationErrorResponse;
            }
            );
            services.ConfigureSwagger();
            services.AddCors(Confg =>
            {
                Confg.AddPolicy("CORSPolicy", options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod()
                    .WithOrigins("http://localhost:4200");
                });
            });

            return services;
        }
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
                });
            });
           
            return services;
        }
    }
}
