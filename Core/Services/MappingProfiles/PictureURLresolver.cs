using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Shared.DTOS;

namespace Services.MappingProfiles
{
    public class PictureURLresolver(IConfiguration configuration) : IValueResolver<Product, ProductResultDTO, string>
    {
        public string Resolve(Product source, ProductResultDTO destination, string destMember, ResolutionContext context)
        {
            // BaseURL + PictureURL
            
            // picture not null
            if (string.IsNullOrWhiteSpace(source.pictureUrl))
                return string.Empty;
          
            return $"{configuration["BaseURL"]}{source.pictureUrl}";

        }
    }
}
