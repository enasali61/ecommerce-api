using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Services.Abstraction;
using Shared;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<IBasketService> _basketService;
        private readonly Lazy<IAuthenticationService> _authenticationService;
        private readonly Lazy<IPaymentService> _paymentService;
        private readonly Lazy<IOrderService> _orderService;
        private readonly Lazy<ICachService> _cachService;


        public ServiceManager(
            IUnitOfWork unitOfWork,
            IBasketRepository basketRepository,
            IMapper mapper,
            UserManager<User> userManager,
            IOptions<JwtOptions> options,
            IConfiguration configuration,
            ICachRepository cachRepository)
        {
            _productService = new Lazy<IProductService>(
                () => new ProductService(unitOfWork, mapper));
            _basketService = new Lazy<IBasketService>(
                () => new BasketService(mapper, basketRepository));
            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthinticationService(userManager, options, mapper));
            _orderService = new Lazy<IOrderService>(() => new OrderService(mapper, basketRepository, unitOfWork));
            _paymentService = new Lazy<IPaymentService>(() => new PaymentService(configuration, basketRepository, unitOfWork, mapper));
            _cachService = new Lazy<ICachService>(()=> new CachService(cachRepository));
        }
        public IProductService ProductService => _productService.Value;

        public IBasketService BasketService => _basketService.Value;

        public IAuthenticationService authenticationService => _authenticationService.Value;

        public IOrderService orderService => _orderService.Value;

        public IPaymentService PaymentService => _paymentService.Value;

        public ICachService CachService => _cachService.Value;
    }
}
