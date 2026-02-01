using Domain.Contracts;
using Services.Abstraction;

namespace Services
{
    internal class CachService(ICachRepository cachRepository) : ICachService
    {
        public async Task<string?> GetCacheValueAsync(string key)
        {
            var value = await cachRepository.GetAsync(key);
            return value == null ? null : value;
        }

        public async Task SetCacheValueAsync(string key, object value, TimeSpan duration)
        {
           await cachRepository.SetAsync(key, value, duration);
           
        }
    }
}
