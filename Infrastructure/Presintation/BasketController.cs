using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.DTOS;

namespace Presintation
{
    //[ApiController]
    //[Route("api/[controller]")] //baseurl/api/Basket
    //[Authorize]
    public class BasketController(IServiceManager _serviceManager) : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<BasketDTO>> Get(string id)
        {
            var basket = await _serviceManager.BasketService.GetBasketAsync(id);
            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDTO>> Update(BasketDTO basketDto)
        {
            var basket = await _serviceManager.BasketService.UpdateBasketAsync(basketDto);
            return Ok(basket);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
          await  _serviceManager.BasketService.DeleteBasketAsync(id);
            return NoContent(); // 204
        }
    }
}
