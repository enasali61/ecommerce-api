using AutoMapper;
using Domain.Entities;
using Shared.DTOS;

namespace Services.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile() 
        {
            // create maping profile for product => product result dto
            CreateMap<Product, ProductResultDTO>()
                .ForMember(D => D.BrandName, option => option.MapFrom(S => S.ProductBrand.Name))
                .ForMember(D => D.TypeName, option => option.MapFrom(S => S.ProductType.Name))
                .ForMember(D => D.pictureUrl, option => option.MapFrom<PictureURLresolver>());

            // create maping profile for productbrand => brand result dto
            CreateMap<ProductBrand, BrandResultDTO>();
            // create maping profile for producttype => type result dto
            CreateMap<ProductType, TypeResultDTO>();

        }

    }
}
