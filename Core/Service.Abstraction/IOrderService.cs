using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.OrderModels;

namespace Services.Abstraction
{
    public interface IOrderService
    {
        // get order by id 
        public Task<OrderResult> GetOrderByIdAsync(Guid id);
        // get all orders for user by his email
        public Task<IEnumerable<OrderResult>> GetOrderByEmailAsync(string email);

        // create arder  
        public Task<OrderResult> CreateOrderAsync(OrderRequest request, string email);

        // get all delivery methods 
        public Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodAsync();

    }
}
