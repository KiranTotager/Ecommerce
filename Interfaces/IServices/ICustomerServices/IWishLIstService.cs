using ECommerce.DTOs.CustomerDTOs;

namespace ECommerce.Interfaces.IServices.ICustomerServices
{
    public interface IWishListService
    {
        public Task AddToWishListAsync(AddWishListDto addWishListDto);
        public Task<IEnumerable<WishListResponseDto>> GetAllWishListsOfUserAsync();
    }
}
