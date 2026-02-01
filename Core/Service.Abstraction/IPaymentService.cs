using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOS;

namespace Services.Abstraction
{
    public interface IPaymentService
    {
        // create or update payment intent
        public Task<BasketDTO> CreateOrUpdatePaymentIntentAsync(string basketID);
        public Task UpdateOrderPaymentStatus(string request , string header ); 
    }
}
