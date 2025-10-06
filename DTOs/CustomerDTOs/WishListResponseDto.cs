namespace ECommerce.DTOs.CustomerDTOs
{
    public class WishListResponseDto
    {
        public long WishListId { get; set; }
        public Guid ProductId { get; set; }
        public DateTime ProductAddedAt { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductDiscount { get; set; }
        public string ProductImageUrl { get; set; }
        public string ProductCategory { get; set; } 
    }
}
