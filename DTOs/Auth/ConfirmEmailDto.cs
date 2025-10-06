using System.ComponentModel.DataAnnotations;

namespace ECommerce.DTOs.Auth
{
    public class ConfirmEmailDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string token { get; set; }
    }
}
