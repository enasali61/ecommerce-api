using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ICachRepository
    {
        // get and set cache responce

        // set[ key , value, expiration time]
        public Task SetAsync(string key, object value, TimeSpan duration);
        // get
        public Task<string?> GetAsync(string key);

    }
}
