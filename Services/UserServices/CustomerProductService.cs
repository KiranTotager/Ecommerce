using ECommerce.DTOs.CustomerDTOs;
using ECommerce.Interfaces.IRepository.ICMSRepos;
using ECommerce.Interfaces.IServices.ICustomerServices;
using ECommerce.Models.CMSModels;

namespace ECommerce.Services.UserServices
{
    public class CustomerProductService : ICustomerProductService
    {
        private readonly IProductRepository _productRepository;
        public CustomerProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<IEnumerable<CustomerProductResponseDto>> GetAllProductsAsnc()
        {
            IEnumerable<Product> Products = await _productRepository.GetAllAsync();
            IEnumerable<CustomerProductResponseDto> customerProductResponseDtos = Products
                .Select(Prdct => new CustomerProductResponseDto
                {
                    ProductId = Prdct.ProductId,
                    ProductName = Prdct.Name,
                    ProductCategoryName = Prdct.Ctgry.Name,
                    ProductDescription = Prdct.Description ?? "",
                    ProductPrice = Prdct.Price,
                    ProductDiscount = Prdct?.Discount ?? 0.0M,
                    ProductImageUrl = Prdct?.ImageUrl ?? ""
                });
            return customerProductResponseDtos;
        }
    }
}
