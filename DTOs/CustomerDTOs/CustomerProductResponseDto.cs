namespace ECommerce.DTOs.CustomerDTOs
{
    public class CustomerProductResponseDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string  ProductImageUrl { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductDiscount { get; set; }
    }
}
