using ECommerce.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string PublicUserName { get; set; }
        [Required]
        public UserStatus Status { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public UserType UserType { get; set; }
    }
}
