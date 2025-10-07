using ECommerce.Models.CMSModels;

namespace ECommerce.Interfaces.IRepository.ICMSRepos
{
    public interface ICoupenDetailRepository
    {
        public Task AddAsync(CoupenDetail Coupen);
        public Task<CoupenDetail> GetByIdAsync(int Id);
        public Task<IEnumerable<CoupenDetail>> GetAllAsync();
        public Task UpdateAsync(CoupenDetail Coupen);
        public Task DeleteByIdAsync(int Id);
        public Task<CoupenDetail> FindByCCodeAsync(string CCode);
        public Task<IEnumerable<CoupenDetail>> GetActiveCoupensAsync();
    }
}
