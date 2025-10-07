using ECommerce.DTOs.CMSDTOs;
using ECommerce.DTOs.CustomerDTOs;

namespace ECommerce.Interfaces.IServices.ICMSServices
{
    public interface ICoupenService
    {
        public Task AddCoupenAsync(AddCoupenDetailsDTO addCoupenDetailsDTO);
        public Task<IEnumerable<CMSCoupenResponseDto>> CMSGetAllCoupensAsync();
        public Task<IEnumerable<CustomerCoupenDetailsResponseDto>> CustomerGetAllACtiveCoupensAsync();
    }
}
