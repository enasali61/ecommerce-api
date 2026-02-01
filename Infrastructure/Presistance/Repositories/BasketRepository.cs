using System.Text.Json;
using StackExchange.Redis;

namespace Presistance.Repositories
{
    public class BasketRepository(IConnectionMultiplexer connectionMultiplexer) : IBasketRepository // to start connection with redis db
    {
        private readonly IDatabase _database = connectionMultiplexer.GetDatabase();

        public async Task<CustomerBasket?> GetBasketAsync(string id)
        {
            var value = await _database.StringGetAsync(id); //Json
            if (value.IsNullOrEmpty)
                return null;
            return JsonSerializer.Deserialize<CustomerBasket>(value, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }); // c# object
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket, TimeSpan? TimeToLive = null)
        {
            var jsonBasket = JsonSerializer.Serialize<CustomerBasket>(basket); // set json at in-memory db
            var isCreatedOrUpdated = await _database.StringSetAsync(basket.Id, jsonBasket, TimeToLive ?? TimeSpan.FromDays(33));
            return isCreatedOrUpdated ? await GetBasketAsync(basket.Id) : null;
        }
        public async Task<bool> DeleteBasketAsync(string id)
        => await _database.KeyDeleteAsync(id);
    }
}
