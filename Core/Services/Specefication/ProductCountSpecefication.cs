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
    public class ProductCountSpecefication : Specefication<Product>
    {
        public ProductCountSpecefication(ProductSpeceficationParams parameters) : base(product =>
        (!parameters.BrandID.HasValue || product.BrandId == parameters.BrandID.Value) // brand id had value and = product brand id 
        &&
        (!parameters.TypeID.HasValue || product.TypeId == parameters.TypeID.Value)
        && (string.IsNullOrWhiteSpace(parameters.Search) || product.Name.ToLower().Contains(parameters.Search.ToLower().Trim()))

        )
        {

        }

    }
}
