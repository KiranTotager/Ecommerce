using ECommerce.DTOs.CustomerDTOs;
using ECommerce.Interfaces.IRepository.IUserRepos;
using ECommerce.Interfaces.IServices.ICustomerServices;
using ECommerce.Interfaces.IUtils;
using ECommerce.Models.UserModels;

namespace ECommerce.Services.UserServices
{
    public class ContactUsService : IContactUsService
    {
        private readonly IContactUsRepository _contactUsRepository;
        private readonly IMailService _mailService;

        public ContactUsService(IContactUsRepository contactUsRepository,IMailService mailService)
        {
            _contactUsRepository = contactUsRepository;
            _mailService = mailService;
        }
        public async Task AddContactUsAsync(ContactUsDto contactUsDto)
        {
            ContactUs NewContact = new ContactUs
            {
                FullName = contactUsDto.FullName,
                Email = contactUsDto.Email,
                MobileNumber=contactUsDto.PhoneNumber,
                Message = contactUsDto.Message,
            };

            await _contactUsRepository.AddAsync(NewContact);
            await _mailService.AdminContact(contactUsDto);
        }
    }
}
