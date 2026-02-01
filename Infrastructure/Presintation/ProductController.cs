using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared;
using Shared.DTOS;
using Shared.ErrorModels;

namespace Presintation
{
    //[ApiController]
    //[Route("api/[controller]")] //baseurl/api/
    //[Authorize]
    public class ProductsController(IServiceManager serviceManager) : ApiController
    {
        #region get all products
        [RedisCache]
        [HttpGet] //get: baseurl/api/products
        public async Task<ActionResult<IEnumerable<ProductResultDTO>>> GetAllProducts([FromQuery]ProductSpeceficationParams parameters)
        {
            var products = await serviceManager.ProductService.GetAllProductsAsync(parameters);
            
            return Ok(products);
        }

        #endregion

        #region get all brands
        [HttpGet("Brands")] //get: baseurl/api/products/Brands
        public async Task<ActionResult<IEnumerable<BrandResultDTO>>> GetAllBrands()
        {
            var brands = await serviceManager.ProductService.GetAllBrandAsync();
            return Ok(brands);
        }
        #endregion

        #region get all types
        [HttpGet("Types")] //get: baseurl/api/products/Types
        public async Task<ActionResult<IEnumerable<TypeResultDTO>>> GetAllTypes()
        {
            var types = await serviceManager.ProductService.GetAllTypesAsync();

            return Ok(types);
        }
        #endregion

        #region get products by id
        [ProducesResponseType(typeof(ProductResultDTO), (int)HttpStatusCode.OK)]

        [HttpGet("{id}")] //get: baseurl/api/product
        public async Task<ActionResult<ProductResultDTO>> GetProduct(int id)
        {
            var product = await serviceManager.ProductService.GetProductAsync(id);

            return Ok(product);
        }
        #endregion
    }
}
