namespace Domain.Entities
{
    public class CustomerBasket 
    {
        // Cart each has id and have items = products
        public string Id { get; set; }
        public IEnumerable<BasketItems> Items { get; set; }

        public string? PaymentIntentaId { get; set; }
        public string? ClientSecret { get; set; }
        public int? deliveryMethodId { get; set; }
        public decimal? ShippingPrice { get; set; }
    }
}
