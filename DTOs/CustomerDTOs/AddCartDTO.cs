using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.DTOs.CustomerDTOs
{
    public class AddCartDTO
    {
        [Required]
        [Range(0,int.MaxValue)]
        [SwaggerSchema("please provide the number of items to add to cart")]
        public int Quantity { get; set; } = 1;
        [Required]
        [SwaggerSchema("please provide product id which you want to add to cart")]
        public Guid ProductId { get; set; }
    }
}
