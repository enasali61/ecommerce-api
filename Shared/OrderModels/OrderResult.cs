namespace Shared.OrderModels
{
    public record OrderResult
    {
        public Guid Id { get; set; }
        // user email
        public string UserEmail { get; set; }
        // shipping address 
        public AddressDto ShippingAddress { get; set; }
        // order items 
        public ICollection<OrderItemsDto> OrderItems { get; set; } = new List<OrderItemsDto>(); // nav prop
        //payment status
        public string PaymentStatus { get; set; }
        // delivery method 
        public string DeliveryMethods { get; set; }
       
        //sub total = orderItem.price * quantity  -for each item
        public decimal SubTotal { get; set; }
        //Payment
        public string PaymenIntId { get; set; } = String.Empty;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public decimal Total { get; set; }     // total = sub total + shipping price (this is drived attribute will count it at run time )

    }
}
