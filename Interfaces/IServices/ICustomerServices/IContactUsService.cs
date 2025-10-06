using ECommerce.DTOs.CustomerDTOs;

namespace ECommerce.Interfaces.IServices.ICustomerServices
{
    public interface IContactUsService
    {
        public Task AddContactUsAsync(ContactUsDto contactUsDto);
    }
}
