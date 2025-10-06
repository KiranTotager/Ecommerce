using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace ECommerce.DTOs.Auth
{
    public class AuthMailRequestDto
    {
        [Required]
        [SwaggerSchema(Description ="please provide the registerd email here",Title ="user email id")]
        public string EmailId { get; set; }
    }
}
