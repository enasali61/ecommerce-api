global using ShippingAddress = Domain.Entities.Order_Entity.Address;
global using userAddress = Domain.Entities.Identity.Address;
using Domain.Entities.Order_Entity;
using Shared.OrderModels;
using AutoMapper;


namespace Services.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<ShippingAddress, AddressDto>().ReverseMap();

            CreateMap<userAddress, AddressDto>().ReverseMap();
            
            CreateMap<OrderItem, OrderItemsDto>().
                ForMember(d=> d.ProductId, opt => opt.MapFrom(s => s.Product.ProductId))
                .ForMember(d=> d.ProductName, opt => opt.MapFrom(s => s.Product.ProductName))
            .ForMember(d => d.PictureURL, opt => opt.MapFrom(s => s.Product.PictureURL));

            CreateMap<DeliveryMethods, DeliveryMethodResult>();
            
            CreateMap<Order, OrderResult>()
                .ForMember(d => d.PaymentStatus, opt => opt.MapFrom(s => s.PaymentStatus.ToString()))
                .ForMember(d => d.DeliveryMethods,opt => opt.MapFrom(s => s.DeliveryMethods.ShortName))
                .ForMember(d => d.Total, opt => opt.MapFrom(s => s.SubTotal + s.DeliveryMethods.Cost));
        
        }
    }
}
