using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
    public class RefreshToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        [Required(ErrorMessage ="token is required")]
        public string Token { get; set; }
        [Required]
        public DateTime ExpiryDate { get; set; }
        [Required]
        public string DeviceInfo { get; set; }
        [Required]
        public string  IPAddress { get; set; }
        [Required]
        public bool IsRevoked { get; set; }
        public string UserId { get; set; } // foriegn key 
        public ApplicationUser User { get; set; } // Navigation property
    }
}
