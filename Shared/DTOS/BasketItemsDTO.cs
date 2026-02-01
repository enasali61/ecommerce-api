using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS
{
    public record BasketItemsDTO
    {
        public int Id { get; init; }
        public string ProductName { get; init; }
        public string PictureURL { get; init; }

        [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 0!")]
        public decimal Price { get; init; }

        [Range(1, 99, ErrorMessage = "Price must be at least 1 item!")]
        public int Quantity { get; init; }
    }
}
