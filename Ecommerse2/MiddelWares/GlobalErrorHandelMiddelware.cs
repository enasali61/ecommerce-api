using System.Net;
using System.Text.Json;
using Domain.Exceptions;
using Shared.ErrorModels;

namespace Ecommerse.Api.MiddelWares
{
    public class GlobalErrorHandelMiddelware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandelMiddelware> _logger;

        public GlobalErrorHandelMiddelware(RequestDelegate next, ILogger<GlobalErrorHandelMiddelware> logger)
        {
            _next = next;
            _logger = logger;
        }
        //response will have status code and error msg
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
                if (httpContext.Response.StatusCode == (int) HttpStatusCode.NotFound)
                {
                    await HandelNotFounfEndPointAsync(httpContext);
                }
            }
            catch (Exception exception)
            {
                //log for exception
                _logger.LogError($"somting went wrong {exception}");
                // handel exception
                await HandelExceptionAsync(httpContext, exception);
            }

        }

        private async Task HandelNotFounfEndPointAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json";
            var response = new ErrorDetails
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                ErrorMessage = $"the end point {httpContext.Request.Path} not found"
            }.ToString();
            await httpContext.Response.WriteAsync(response);
        }

        private async Task HandelExceptionAsync(HttpContext httpContext, Exception exception)
        {
            // set content type as application\json
            httpContext.Response.ContentType = "application/json";
            // set defualt status code to 500
            httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            // return standared response 
           
            var response = new ErrorDetails // c# object
            {
                ErrorMessage = exception.Message,
            };

            httpContext.Response.StatusCode = exception switch
            {
                NotFoundException => (int) HttpStatusCode.NotFound, //404
                UnAuthurisedException => (int) HttpStatusCode.Unauthorized, //401
              ValidationException validationException => HandelValidationException(validationException, response),
                _ => (int) HttpStatusCode.InternalServerError, // 500
            };

                     
         
            await httpContext.Response.WriteAsync(response.ToString()); // to json
        }

        private int HandelValidationException(ValidationException validationException, ErrorDetails response)
        {
            response.Errors = validationException.Errors;
            return (int) HttpStatusCode.BadRequest; //400
        }
    }
}
