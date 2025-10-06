using ECommerce.Models.CMSModels;

namespace ECommerce.Interfaces.IRepository.ICMSRepos
{
    public interface IProductRepository
    {
        public Task AddAsync(Product Prdct);
        public Task<Product> GetByIdAsync(Guid Id);
        public Task<IEnumerable<Product>> GetAllAsync();
        public Task UpdateAsync(Product Prdct);
        public Task DeleteByIdAsync(Guid Id);
        public Task<bool> IsProductExistAsync(Guid Id);
    }
}
