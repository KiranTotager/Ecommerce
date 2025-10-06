using ECommerce.Models.CMSModels;

namespace ECommerce.Interfaces.IRepository.ICMSRepos
{
    public interface ICategoryRepository
    {
        public Task AddAsync(Category Category);
        public Task<Category> GetByIdAsync(int Id);
        public Task<IEnumerable<Category>> GetAllAsync();
        public Task UpdateAsync(Category Category);
        public Task DeleteByIdAsync(int Id);
        public Task<Category> FindByNameAsync(string Name);
    }
}
