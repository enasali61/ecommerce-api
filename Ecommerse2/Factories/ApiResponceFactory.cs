using System.Net;
using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;

namespace Ecommerse.Api.Factories
{
    public class ApiResponceFactory
    {
        // context 
        // string => key , name of param
        // modelstateDictionary
        public static IActionResult CustumValidationErrorResponse(ActionContext context)
        {
            // get all errors in model state
            var errors = context.ModelState.Where(error => error.Value.Errors.Any())
                .Select(error => new ValidationError
                {
                    Feild = error.Key,
                    Errors = error.Value.Errors.Select(e => e.ErrorMessage)
                });
            // create to custum response 
            var response = new ValidationErrorResponse
            {
                statusCode = (int)HttpStatusCode.BadRequest,
                errorMessage = "validation failed",
                Errors = errors
            };
            // return 
            return new BadRequestObjectResult(response);
        }
    }
}
