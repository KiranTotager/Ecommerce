using ECommerce.Models.CMSModels;

namespace ECommerce.Interfaces.IRepository.ICMSRepos
{
    public interface IStaffRepository
    {
        public Task AddAsync(Staff Stf);
        public Task<Staff> GetByIdAsync(Guid Id);

        public Task<IEnumerable<Staff>> GetAllAsync();
        public Task UpdateAsync(Staff Stf);
        public Task DeleteByIdAsync(Guid Id);
    }
}
