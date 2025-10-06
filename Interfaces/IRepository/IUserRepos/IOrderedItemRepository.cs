using ECommerce.Models.UserModels;

namespace ECommerce.Interfaces.IRepository.IUserRepos
{
    public interface IOrderedItemRepository
    {
        public Task AddAsync(OrderedItem OItem);
        public Task<OrderedItem> GetByIdAsync(long Id);
        public Task<IEnumerable<OrderedItem>> GetAllAsync();
        public Task UpdateAsync(OrderedItem OItem);
        public Task DeleteByIdAsync(long Id);
    }
}
