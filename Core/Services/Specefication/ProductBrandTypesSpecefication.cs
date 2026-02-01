using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using Shared;

namespace Services.Specefication
{
    public class ProductBrandTypesSpecefication : Specefication<Product>
    {
        // create object used to get product by id
        public ProductBrandTypesSpecefication(int id): base(p => p.Id == id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
        // create object used to get all products
        public ProductBrandTypesSpecefication(ProductSpeceficationParams parameters) : base(product => 
        (!parameters.BrandID.HasValue || product.BrandId == parameters.BrandID.Value) // brand id had value and = product brand id 
        &&
        (!parameters.TypeID.HasValue || product.TypeId == parameters.TypeID.Value)
        && (string.IsNullOrWhiteSpace(parameters.Search) || product.Name.ToLower().Contains(parameters.Search.ToLower().Trim()))
        )
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            
           
            if (parameters.Sort is not null )
            { 
                //sorting
                switch(parameters.Sort)
                {
                    case ProductSortingOptions.PriceAsc:
                        SetOrderBy(p => p.Price);
                        break;
                    case ProductSortingOptions.PriceDesc:
                        SetOrderByDescending(p => p.Price);
                        break;
                    case ProductSortingOptions.NameDesc:
                        SetOrderByDescending(p => p.Name);
                        break;
                    default:
                        SetOrderBy(p => p.Name);
                        break;



                }

                
            }
            ApplyPagination(parameters.PageIndex, parameters.PageSize);

        }
    }
}
