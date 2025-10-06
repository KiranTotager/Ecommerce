using ECommerce.DTOs.CMSDTOs;

namespace ECommerce.Interfaces.IServices.ICMSServices
{
    public interface ICoupenService
    {
        public Task AddCoupenAsync(AddCoupenDetailsDTO addCoupenDetailsDTO);
    }
}
