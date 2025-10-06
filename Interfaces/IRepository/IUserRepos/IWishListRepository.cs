using ECommerce.Models.UserModels;

namespace ECommerce.Interfaces.IRepository.IUserRepos
{
    public interface IWishListRepository
    {
        public Task AddAsync(WishList WList);
        public Task<WishList> GetByIdAsync(long Id);
        public Task<IEnumerable<WishList>> GetAllAsync();
        public Task UpdateAsync(WishList WList);
        public Task DeleteAsync(long Id);
        public Task<IEnumerable<WishList>> GetByCustomerIdAsync(Guid CId);
        public Task<IEnumerable<WishList>> GetByGuestIdAsync(Guid GId);
    }
}
