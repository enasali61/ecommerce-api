using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;
using Shared.DTOS;

namespace Services.Abstraction
{
    public interface IProductService
    {
        // get all products
        public Task<PaginatedResult<ProductResultDTO>> GetAllProductsAsync(ProductSpeceficationParams parameters);
        // get all brands
        public Task<IEnumerable<BrandResultDTO>> GetAllBrandAsync();

        //get all types
        public Task<IEnumerable<TypeResultDTO>> GetAllTypesAsync();
           
        //get product by id
        public Task<ProductResultDTO> GetProductAsync(int Id);
        

    }
}
