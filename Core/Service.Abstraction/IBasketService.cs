using Shared.DTOS;

namespace Services.Abstraction
{
    public interface IBasketService
    {
        // Get Delete Update
        public Task<BasketDTO> GetBasketAsync(string id);
        public Task<BasketDTO> UpdateBasketAsync(BasketDTO basket);
        public Task<bool> DeleteBasketAsync(string id);
    }
}
