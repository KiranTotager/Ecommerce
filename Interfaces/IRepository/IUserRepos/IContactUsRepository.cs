using ECommerce.Models.UserModels;

namespace ECommerce.Interfaces.IRepository.IUserRepos
{
    public interface IContactUsRepository
    {
        public Task AddAsync(ContactUs Contact);
        public Task<ContactUs> GetByIdAsync(long Id);
        public Task<IEnumerable<ContactUs>> GetAllAsync();
        public Task UpdateAsync(ContactUs Contact);
        public Task DeleteByIdAsync(long Id);
    }
}
