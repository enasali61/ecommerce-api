using Domain.Contracts;
using Ecommerse.Api.MiddelWares;

namespace Ecommerse.Api.Extentions
{
    public static class WebApplicationExtentions
    {
        public static async Task<WebApplication> SeedDatabaseAsync(this WebApplication app)
        {
            //create boject from type implements idbintializer
            using var scope = app.Services.CreateScope(); //using عشان لما افتح  connection و اقفله 
            var dbIntializer = scope.ServiceProvider.GetRequiredService<IDbIntializer>();
           
            await dbIntializer.IntializeAsync();
            await dbIntializer.IntializeIdentityAsync();

            return app;
        }
        
        public static WebApplication UseCustumExceptionMiddelware(this WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandelMiddelware>();
            return app;
        }
    }
}
