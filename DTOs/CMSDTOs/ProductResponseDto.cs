namespace ECommerce.DTOs.CMSDTOs
{
    public class ProductResponseDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public DateTime ProductCreatedAt { get; set; }
        public DateTime ProductUpdatedAt { get; set; }
        public string ProductImageUrl { get; set; }
        public string ProductAddedIp { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductDiscount { get; set; }
        public int ProductStockQuantity { get; set; }
        public string ProductAddedBy { get; set; }
    }
}
