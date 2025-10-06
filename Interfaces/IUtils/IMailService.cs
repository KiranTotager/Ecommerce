using ECommerce.DTOs.CustomerDTOs;

namespace ECommerce.Interfaces.IUtils
{
    public interface IMailService
    {
        public Task OTPSender(string toMail, string OTP);
        public Task ConfirmationTokenSender(string toMail, string url);
        public Task MailSenderHelper(string toMail, string subject, string body);
        public Task AdminContact(ContactUsDto contactUs);
    }
}
