using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.DTOs.CustomerDTOs
{
    public class AddWishListDto
    {
        [Required]
        [SwaggerSchema("id of the product which you want to add to wish list")]
        public Guid ProductId { get; set; }
    }
}
