using Swashbuckle.AspNetCore.Annotations;

namespace ECommerce.DTOs.CustomerDTOs
{
    public class CartItemResponseDto
    {
        [SwaggerSchema("id of the product")]
        public Guid ProductId { get; set; }
        [SwaggerSchema("number of quntity of product in cart")]
        public int ProductQuantity { get; set; }
        [SwaggerSchema("name of the product")]
        public string ProductName { get; set; }
        [SwaggerSchema("product Description")]
        public string ProductDescription { get; set; }
        [SwaggerSchema("price of a product")]
        public decimal ProductPrice { get; set; }
        [SwaggerSchema("discount on the product")]
        public decimal ProductDiscount { get; set; }
        [SwaggerSchema("url of the image")]
        public string ProductImageUrl { get; set; }
        [SwaggerSchema("cateogry of the product")]
        public string ProductCategory { get; set; }
        [SwaggerSchema("total sum of the perticular items in cart")]
        public decimal ItemsTotalPrice { get; set; }
        [SwaggerSchema]
        public decimal GrandSum { get; set; }
    }
}
