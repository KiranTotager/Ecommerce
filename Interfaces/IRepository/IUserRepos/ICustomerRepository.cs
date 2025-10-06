using ECommerce.Models.UserModels;

namespace ECommerce.Interfaces.IRepository.IUserRepos
{
    public interface ICustomerRepository
    {
        public Task AddAsync(Customer Cstmr);
        public Task<Customer> GetByIdAsync(Guid Id);
        public Task<IEnumerable<Customer>> GetAllAsync();
        public Task UpdateAsync(Customer Cstmr);
        public Task DeleteByIdAsync(Guid Id);
    }
}
