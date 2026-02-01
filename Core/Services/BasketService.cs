using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Services.Abstraction;
using Shared.DTOS;

namespace Services
{
    public class BasketService(IMapper _mapper,IBasketRepository basketRepository) : IBasketService
    {
        public async Task<bool> DeleteBasketAsync(string id)
        => await basketRepository.DeleteBasketAsync(id);

        public async Task<BasketDTO> GetBasketAsync(string id)
        {
           var basket = await basketRepository.GetBasketAsync(id);
            return basket is null ? throw new BasketNotFoundException(id) : _mapper.Map<BasketDTO>(basket);        
        }

        public async Task<BasketDTO> UpdateBasketAsync(BasketDTO basket)
        {
            var customerBasket = _mapper.Map<CustomerBasket>(basket);
            var updatedBasket = await basketRepository.UpdateBasketAsync(customerBasket);
            return updatedBasket is null ? throw new Exception("can not update basket") : _mapper.Map<BasketDTO>(updatedBasket);  
        }
    }
}
