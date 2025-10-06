using ECommerce.Models.UserModels;

namespace ECommerce.Interfaces.IRepository.IUserRepos
{
    public interface IReviewRepository
    {
        public Task AddAsync(Review Rvw);
        public Task<Review> GetByIdAsync(long Id);
        public Task<IEnumerable<Review>> GetAllAsync();
        public Task UpdateAsync(Review Rvw);
        public Task DeleteByIdAsync(long Id);
        public Task<IEnumerable<Review>> GetByProductIdAsync(Guid productId);
    }
}
