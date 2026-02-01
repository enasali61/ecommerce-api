using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS
{
    public record BasketDTO
    {
        public string Id { get; init; }
        public IEnumerable<BasketItemsDTO> Items { get; init; }
        public string? PaymentIntentaId { get; set; }
        public string? ClientSecret { get; set; }
        public int? deliveryMethodId { get; set; }
        public decimal? ShippingPrice { get; set; }
    }
}
