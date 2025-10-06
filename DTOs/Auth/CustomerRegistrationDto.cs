using System.ComponentModel.DataAnnotations;

namespace ECommerce.DTOs.Auth
{
    public class CustomerRegistrationDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string EmailId { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string  Address { get; set; }
         
    }
}
