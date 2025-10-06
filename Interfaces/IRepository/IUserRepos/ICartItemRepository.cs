using ECommerce.Models.UserModels;

namespace ECommerce.Interfaces.IRepository.IUserRepos
{
    public interface ICartItemRepository
    {
        public Task AddAsync(CartItem Item);
        public Task<CartItem> GetByIdAsync(long Id);
        public Task<IEnumerable<CartItem>> GetAllAsync();
        public Task UpdateAsync(CartItem Item);
        public Task DeleteByIdAsync(long Id);
        public Task<IEnumerable<CartItem>> GetByCustomerIdAsync(Guid customerId);
        public Task<IEnumerable<CartItem>> GetByGuestIdAsync(Guid guestId);
    }
}
