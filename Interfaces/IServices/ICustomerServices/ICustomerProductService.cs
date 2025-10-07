using ECommerce.DTOs.CustomerDTOs;

namespace ECommerce.Interfaces.IServices.ICustomerServices
{
    public interface ICustomerProductService
    {
        public Task<IEnumerable<CustomerProductResponseDto>> GetAllProductsAsnc();
    }
}
