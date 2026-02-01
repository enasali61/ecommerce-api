using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Order_Entity;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Services.Abstraction;
using Services.Specefication;
using Shared.OrderModels;

namespace Services
{
    internal class OrderService(IMapper mapper,
        IBasketRepository basketRepository,
        IUnitOfWork unitOfWork
        ) : IOrderService
    {
        public async Task<OrderResult> CreateOrderAsync(OrderRequest request, string email)
        {
            // 1. shipping addresss
            var address = mapper.Map<ShippingAddress>(request.ShipToAddress);
            // 2. order items from baasket using its id => basket items
            var basket = await basketRepository.GetBasketAsync(request.BasketId)
                ?? throw new BasketNotFoundException(request.BasketId);
            
            // get items at basket 
            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product, int>()
                    .GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
                orderItems.Add(CreateOrderItem(item, product));
            }
            
            // 3. know delivery methods 
            var deliveryMethods = await unitOfWork.GetRepository<DeliveryMethods,int>()
                .GetByIdAsync(request.DeliveryMethodId) ?? throw new DeliveryMethodsNotFoundException(request.DeliveryMethodId);
            // 4. subtotal
            var orderRepo = unitOfWork.GetRepository<Order, Guid>();
            var existingOrder = await orderRepo.GetByIdAsync(new OrderWithPaymentIntentSpec(basket.PaymentIntentaId!));
            
            if (existingOrder is not null)
            {
                orderRepo.DeleteAsync(existingOrder); 
            }

            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);
            // 5. create order 
            var order = new Order(email,address,orderItems,deliveryMethods,subtotal,basket.PaymentIntentaId!);
            // 6. save to database 
            await orderRepo.AddAsync(order);
            try
            {
                await unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                // CAPTURE AND THROW THE REAL ERROR
                var fullError = (dbEx);

                // Also log to console immediately
                Console.WriteLine("=== DATABASE ERROR DETAILS ===");
                Console.WriteLine(fullError);
                Console.WriteLine("=== STACK TRACE ===");
                Console.WriteLine(dbEx.StackTrace);

                throw new Exception($"Database save failed: {fullError}");
            }
            // map<order , order result > & return
            return mapper.Map<OrderResult>(order);
        }

        private OrderItem CreateOrderItem(BasketItems item, Product product)
        => new OrderItem(new ProductInOrderItem(product.Id, product.Name, product.pictureUrl),item.Price,item.Quantity);

        public async Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodAsync()
        {
           var methods = await unitOfWork.GetRepository<DeliveryMethods, int>()
                .GetAllAsync();
            return mapper.Map<IEnumerable<DeliveryMethodResult>>(methods);
        }

        public async Task<IEnumerable<OrderResult>> GetOrderByEmailAsync(string email)
        {
            var orders = await unitOfWork.GetRepository<Order, Guid>()
                .GetAllAsync(new OrderWithIncludeSpec(email));
            return mapper.Map<IEnumerable<OrderResult>>(orders);
        }

        public async Task<OrderResult> GetOrderByIdAsync(Guid id)
        {
            var order = await unitOfWork.GetRepository<Order, Guid>()
                .GetByIdAsync(new OrderWithIncludeSpec(id))
                ?? throw new OrderNotFoundException(id);
            return mapper.Map<OrderResult>(order);
        }
    }
}
