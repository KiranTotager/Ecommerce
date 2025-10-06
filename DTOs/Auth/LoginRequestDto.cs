using ECommerce.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ECommerce.DTOs.Auth
{
    public class LoginRequestDto
    {
        [Required]
        public string EmailId { get; set; }
        [Required]
        public string Credential { get; set; }
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LoginMethod LMethod { get; set; }
    }
}
