using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.DTOS;

namespace Presintation
{
    public class PaymentsController(IServiceManager serviceManager) : ApiController
    {
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdatePayment(string basketId)
        { 
            var result = await serviceManager.PaymentService.CreateOrUpdatePaymentIntentAsync(basketId);
            return Ok(result);
        }

        const string endpointSecret = "whsec_...";

        [HttpPost("WebHook")] //https://localhost:7102/api/Payments/WebHook
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeHeader = Request.Headers["Stripe-Signature"];

            //var stripeEvent = EventUtility.ConstructEvent(request,
            //        header, endPointSecret, throwOnApiVersionMismatch: false);

            await serviceManager.PaymentService.UpdateOrderPaymentStatus(json, stripeHeader!);
            return new EmptyResult();
        }
    }
}

