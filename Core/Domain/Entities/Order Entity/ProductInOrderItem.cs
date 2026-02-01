using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Order_Entity
{
    public class ProductInOrderItem
    {
        public ProductInOrderItem()
        {
            
        }
        public ProductInOrderItem(int productId, string productName, string pictureURL)
        {
            ProductId = productId;
            ProductName = productName;
            PictureURL = pictureURL;
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureURL { get; set; }
    }
}
