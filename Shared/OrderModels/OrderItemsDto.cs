using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.OrderModels
{
    public record OrderItemsDto
    {
        public int ProductId { get; init; }
        public string ProductName { get; init; }
        public string PictureURL { get; init; }
        public decimal Price { get; init; }
        public int Quantity { get; init; }
    }
}
