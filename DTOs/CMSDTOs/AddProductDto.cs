using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.DTOs.CMSDTOs
{
    public class AddProductDto
    {
        [Required]
        [SwaggerSchema("provide the name of the product")]
        public string ProductName { get; set; }
        [Required]
        [SwaggerSchema("provide the descritpion for the product")]
        public string ProductDescription { get; set; }
        [Required]
        [SwaggerSchema("provide the price for the product here")]
        public decimal ProductPrice { get; set; }
        [SwaggerSchema("provide the description for the product")]
        public decimal? ProductDiscount { get; set; }
        [Required]
        [SwaggerSchema("provide the available quantity of product")]
        public int StockQuantity { get; set; }
        [Required]
        [SwaggerSchema("provide the image of the product")]
        public IFormFile ProductImage { get; set; }
        [Required]
        [SwaggerSchema("provide the product category name")]
        public string ProductCategory { get; set; }
    }
}
