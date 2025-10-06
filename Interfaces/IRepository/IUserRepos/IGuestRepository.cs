using ECommerce.Models.UserModels;

namespace ECommerce.Interfaces.IRepository.IUserRepos
{
    public interface IGuestRepository
    {
        public Task AddAsync(Guest Gst);
        public Task<Guest> GetByIdAsync(Guid Id);
        public Task<IEnumerable<Guest>> GetAllAsync();
        public Task UpdateAsync(Guest Gst);
        public Task DeleteByIdAsync(Guid Id);
        public Task<bool> IsGuestExist(Guid Id);
    }
}
