using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.OrderModels;

namespace Presintation
{
    [Authorize]
    public class OrdersController(IServiceManager serviceManager) : ApiController
    {
        // create order 
        [HttpPost]
        public async Task<ActionResult<OrderResult>> CreateOrder(OrderRequest orderRequest)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await serviceManager.orderService.CreateOrderAsync(orderRequest, email);

            return Ok(order);
        }
        
        // get all orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResult>>> GetAllOrdersByEmail()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await serviceManager.orderService.GetOrderByEmailAsync(email);   
            return Ok(orders);
        }
        
        // get order by id
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResult>> GetOrderById(Guid id)
        {
            var order = await serviceManager.orderService.GetOrderByIdAsync(id);
            return Ok(order);
        }

        // get all delivery methods
        [AllowAnonymous]
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<DeliveryMethodResult>> GetDeliveryMethods()
        {
            var methods = await serviceManager.orderService.GetDeliveryMethodAsync();
            return Ok(methods);
        }

       

    }
}
