using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction
{
    public interface ICachService
    {
        public Task SetCacheValueAsync(string key, object value , TimeSpan duration);
        public Task<string?> GetCacheValueAsync(string key);
    }
}
