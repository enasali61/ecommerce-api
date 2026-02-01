global using Product = Domain.Entities.Product; 
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Order_Entity;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Services.Abstraction;
using Services.Specefication;
using Shared.DTOS;
using Shared.OrderModels;
using Stripe;
using Stripe.Forwarding;

namespace Services
{
    internal class PaymentService( IConfiguration configuration,
        IBasketRepository basketRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : IPaymentService
    {
        
        public async Task<BasketDTO> CreateOrUpdatePaymentIntentAsync(string basketID)
        {
            // 1. setup stripe API key = secret key
            StripeConfiguration.ApiKey = configuration.GetSection("StripeSittings")["SecretKey"];

            // 2. get basket => basket repository 
            var basket = await basketRepository.GetBasketAsync(basketID)
                ?? throw new BasketNotFoundException(basketID);
            // 3. validate on basket items price = product price in DB
            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product, int>()
                    .GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);

                //  update item price from DB in case it was wrong
                item.Price = product.Price;
            }
            // 4.get delivery method and shipping price
            if (!basket.deliveryMethodId.HasValue)
                throw new Exception("no delivery method was selected");
            
                // 5.return delrivery method from DB and assign price of basket 
             var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethods, int>()
                    .GetByIdAsync(basket.deliveryMethodId.Value) ??
                    throw new DeliveryMethodsNotFoundException(basket.deliveryMethodId.Value);
            
            basket.ShippingPrice = deliveryMethod.Cost;
            // 6.total = sub + shipping price 
            var amount = (long) (basket.Items.Sum(i => i.Price * i.Quantity) + basket.ShippingPrice) * 100;
            
            var service = new PaymentIntentService();

            // 7.create or update payment intent 
            if (string.IsNullOrWhiteSpace(basket.PaymentIntentaId))
            {
                // create 
                var createOptions = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "USD",
                    PaymentMethodTypes = new List<string>
                    {
                        "card"
                    }
                };

                // calling stripe API from service
                var PaymentIntent = await service.CreateAsync(createOptions);
                basket.PaymentIntentaId = PaymentIntent.Id;
                basket.ClientSecret = PaymentIntent.ClientSecret;
            }
            else
            {
                // update
                // if product price changed 
                // or user changes delivery method
                // or remove any item from basket
                var updatedOptions = new PaymentIntentUpdateOptions
                {
                    Amount = amount,
                };
                await service.UpdateAsync(basket.PaymentIntentaId,updatedOptions);
            }
            // 8.save all changes to db
            await basketRepository.UpdateBasketAsync(basket);

            // 9.mapping basket to basketDto and return
            return mapper.Map<BasketDTO>(basket);
        }

        public async Task UpdateOrderPaymentStatus(string request, string header)
        {
            var endPointSecret = configuration.GetSection("StripeSittings")["EndPointSecret"]; 
                var stripeEvent = EventUtility.ConstructEvent(request,
                    header,endPointSecret,throwOnApiVersionMismatch:false);

                // Handle the event
                // If on SDK version < 46, use class Events instead of EventTypes
               
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            
            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentPaymentFailed:
                    {
                       await UpdatePaymentFailed(paymentIntent!.Id);  
                        break;
                    }

                case EventTypes.PaymentIntentSucceeded:
                    {
                        await UpdatePaymentSuccessed(paymentIntent!.Id);
                        break;
                    }
                // ... handle other event types
                default:
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                    break;
            }

        }

        private async Task UpdatePaymentSuccessed(string paymentIntentId)
        {
            var orderRepo = unitOfWork.GetRepository<Order, Guid>();
            var order = await orderRepo.GetByIdAsync(new OrderWithPaymentIntentSpec(paymentIntentId) ??
                throw new Exception());
            order.PaymentStatus = OrderPaymentStatus.PaymentRecevied;
            orderRepo.UpdateAsync(order);
            await unitOfWork.SaveChangesAsync();
        }

        private async Task UpdatePaymentFailed(string paymentIntentId)
        {
            var orderRepo = unitOfWork.GetRepository<Order, Guid>();
            var order = await orderRepo.GetByIdAsync(new OrderWithPaymentIntentSpec(paymentIntentId) ??
                throw new Exception());
            order.PaymentStatus = OrderPaymentStatus.PaymentFailed;
            orderRepo.UpdateAsync(order);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
