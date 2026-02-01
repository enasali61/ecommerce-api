namespace Domain.Entities
{
    public class BasketItems // products in cart
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string PictureURL { get; set; }
        public decimal Price { get; set; }
        public int  Quantity  { get; set; }


    }
}
