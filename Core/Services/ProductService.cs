using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Services.Abstraction;
using Services.Specefication;
using Shared;
using Shared.DTOS;

namespace Services
{
    // primary ctor ==============>
    internal class ProductService(IUnitOfWork _unitOfWork,IMapper _mapper) : IProductService
    {

        public async Task<PaginatedResult<ProductResultDTO>> GetAllProductsAsync(ProductSpeceficationParams parameters)
        {
            //get all products => unit of work
            var products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(new ProductBrandTypesSpecefication(parameters));
            //mapping to product result dto => imapper
            var productsResult = _mapper.Map< IEnumerable<ProductResultDTO>>(products);
            // return product

            var count = productsResult.Count();
            var totalCount =await _unitOfWork.GetRepository<Product, int>().CountAsync(new ProductCountSpecefication(parameters));
            var result = new PaginatedResult<ProductResultDTO>(
                parameters.PageIndex,                
                count,
                totalCount,
                productsResult
                );
            return result;
        }
        public async Task<ProductResultDTO> GetProductAsync(int Id)
        {
            //get  product => unit of work
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(new ProductBrandTypesSpecefication(Id));
            //mapping to product result dto => imapper
            //var productResult = _mapper.Map<ProductResultDTO>(product);
            // return product
            ///return productResult;
            return product is null ? throw new ProductNotFoundException(Id) : _mapper.Map<ProductResultDTO>(product);
        }
        public async Task<IEnumerable<BrandResultDTO>> GetAllBrandAsync()
        {
            //get all brands => unit of work
            var brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            //mapping to brand result dto => imapper
            var brandsResult = _mapper.Map<IEnumerable<BrandResultDTO>>(brands);
            // return brand
            return brandsResult;
        }
        public async Task<IEnumerable<TypeResultDTO>> GetAllTypesAsync()
        {
            //get all types => unit of work
            var types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            //mapping to type result dto => imapper
            var typesResult = _mapper.Map<IEnumerable<TypeResultDTO>>(types);
            // return types
            return typesResult;
        }

        
    }
}
