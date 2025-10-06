using ECommerce.DTOs.CustomerDTOs;

namespace ECommerce.Interfaces.IServices.ICustomerServices
{
    public interface ICartItemService
    {
        public Task AddToCartAsync(AddCartDTO addCartDTO);
        public Task<IEnumerable<CartItemResponseDto>> GetAllCartItemsAsync();    
    }
}
