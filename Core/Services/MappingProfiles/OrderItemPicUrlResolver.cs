using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Execution;
using Domain.Entities.Order_Entity;
using Microsoft.Extensions.Configuration;
using Shared.OrderModels;

namespace Services.MappingProfiles
{
    public class OrderItemPicUrlResolver(IConfiguration configuration) : IValueResolver<OrderItem, OrderItemsDto, string>
    {
        public string Resolve(OrderItem source, OrderItemsDto destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(source.Product.PictureURL))
                return string.Empty;
            return $"{configuration["BaseURL"]}{source.Product.PictureURL}";
        }
    }
}
