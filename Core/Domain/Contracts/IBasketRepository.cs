using Domain.Entities;

namespace Domain.Contracts
{
    public interface IBasketRepository
    {
        // get basket 
        public Task<CustomerBasket?> GetBasketAsync(string id);
        // delete basket 
        public Task<bool> DeleteBasketAsync(string id);
        // creat or update basket
        public Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket , TimeSpan? TimeToLive = null); 
    }
}
