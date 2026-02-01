using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public enum ProductSortingOptions
    { 
        NameAsc,
        NameDesc,
        PriceAsc,
        PriceDesc
    }
    public class ProductSpeceficationParams
    {
        public ProductSortingOptions? Sort { get; set; }
        public int? BrandID { get; set; }
        public int? TypeID { get; set; }

        private const int MaxPageSize = 10;
        private const int DefultPageSize = 5;
        public int PageIndex { get; set; } = 1;
        private int pageSize = DefultPageSize;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > MaxPageSize ? MaxPageSize : value ;
        }
        public string? Search { get; set; }
    }
}
