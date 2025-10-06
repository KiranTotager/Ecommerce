using ECommerce.Models.UserModels;

namespace ECommerce.Interfaces.IRepository.IUserRepos
{
    public interface IOrderDetailRepository
    {
        public Task AddAsync(OrderDetail Order);
        public Task<OrderDetail> GetByIdAsync(Guid Id);
        public Task<IEnumerable<OrderDetail>> GetAllAsync();
        public Task UpdateAsync(OrderDetail Order);
        public Task DeleteByIdAsync(Guid Id);
    }
}
