using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.DTOs.Auth
{
    public class RoleDto
    {
        [Required]
        [SwaggerSchema(Description ="please provid the name of the role which you want to add ")]
        public string Role { get; set; }
    }
}
