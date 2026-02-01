using System.Text.Json;
using StackExchange.Redis;

namespace Presistance.Repositories
{
    public class CachRepository(IConnectionMultiplexer connectionMultiplexer) : ICachRepository
    {
        private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
        public async Task<string?> GetAsync(string key)
        {
            var value = await _database.StringGetAsync(key);

            return value.IsNullOrEmpty ? default : value;
        }

        public async Task SetAsync(string key, object value, TimeSpan duration)
        {
            var seriledOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var serilzedObj = JsonSerializer.Serialize(value, seriledOptions);
            
            await _database.StringSetAsync(key, serilzedObj, duration);

        }
    }
}
