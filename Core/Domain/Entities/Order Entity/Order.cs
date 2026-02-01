
namespace Domain.Entities.Order_Entity
{
    public class Order : BaseEntity<Guid>
    {
        public Order()
        {
            
        }
        public Order(
            string userEmail
            , Address shippingAddress,
            ICollection<OrderItem> orderItems,
            DeliveryMethods deliveryMethods,
            decimal subTotal,
            string paymenIntentId)
        {
            UserEmail = userEmail;
            ShippingAddress = shippingAddress;
            OrderItems = orderItems;
            DeliveryMethods = deliveryMethods;
            SubTotal = subTotal;
            PaymenIntentId = paymenIntentId;
        }

        // user email
        public string UserEmail { get; set; }
        // shipping address 
        public Address ShippingAddress { get; set; }
        // order items 
        public ICollection<OrderItem> OrderItems { get; set; } // nav prop
        //payment status
        public OrderPaymentStatus PaymentStatus { get; set; } = OrderPaymentStatus.pending;
        // delivery method 
        public DeliveryMethods DeliveryMethods  { get; set; }
        public int? DeliveryMethodsId  { get; set; } //FK 1:M
        //sub total = orderItem.price * quantity  -for each item
        // total = sub total + shipping price (this is drived attribute will count it at run time )
        public decimal SubTotal { get; set; }
        //Payment
        public string PaymenIntentId { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now; 


    }
}
