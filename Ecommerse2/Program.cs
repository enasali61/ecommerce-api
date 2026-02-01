
using Domain.Contracts;
using Ecommerse.Api.Extentions;
using Ecommerse.Api.Factories;
using Ecommerse.Api.MiddelWares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presistance.Data;
using Presistance.Repositories;
using Services;
using Services.Abstraction;



namespace Ecommerse2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // presentation service
            builder.Services.AddPresentationServices();

            // core services
            builder.Services.AddCoreServices(builder.Configuration);


            // infrastructure
            builder.Services.AddInfrastructureService(builder.Configuration);

            var app = builder.Build();
            await app.SeedDatabaseAsync();

            #region  Configure midelwares
            // Configure the HTTP request pipeline.

            app.UseCustumExceptionMiddelware();            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseCors("CORSPolicy");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            #endregion

            app.Run();

            
        }
    }
}
