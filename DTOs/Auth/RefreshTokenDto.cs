using System.ComponentModel.DataAnnotations;

namespace ECommerce.DTOs.Auth
{
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
