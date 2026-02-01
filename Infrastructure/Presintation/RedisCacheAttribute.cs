using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstraction;

namespace Presintation
{
    internal class RedisCacheAttribute(int durationInSecound = 60) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {

            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().CachService;

            string cacheKey = GenrateCacheKey(context.HttpContext.Request);
            // id data alr in cache => return response without entering end point 
            var result = await cacheService.GetCacheValueAsync(cacheKey);
            if (result != null)
            {
                context.Result = new ContentResult
                {
                    Content = result,
                    ContentType = "Application/Json",
                    StatusCode = (int)HttpStatusCode.OK
                };
                return;
            }

            // data not cached => enter end poing
            var contentResult = await next.Invoke();
            if (contentResult.Result is OkObjectResult okObject)
            {
                await cacheService.SetCacheValueAsync(cacheKey, okObject, TimeSpan.FromSeconds(durationInSecound));
            }

        }
        private string GenrateCacheKey(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append(request.Path); // api/product
            
            foreach(var item in request.Query.OrderBy(q => q.Key))
            {
                keyBuilder.Append($"|{item.Key}-{item.Value}");
            }
            return keyBuilder.ToString();
        }
    }
}
