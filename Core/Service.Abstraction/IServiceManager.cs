using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction
{
    public interface IServiceManager
    {
        //signature for each and every service
        public IProductService ProductService { get; }
        public IBasketService BasketService { get; }
        public IAuthenticationService authenticationService { get; }
        public IOrderService orderService { get; }
        public IPaymentService PaymentService { get; }
        public ICachService CachService { get; }

    }
}
