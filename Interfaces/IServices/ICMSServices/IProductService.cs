using ECommerce.DTOs.CMSDTOs;

namespace ECommerce.Interfaces.IServices.ICMSServices
{
    public interface IProductService
    {
        public Task AddProductAsync(AddProductDto product);
        public Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();
    }
}
