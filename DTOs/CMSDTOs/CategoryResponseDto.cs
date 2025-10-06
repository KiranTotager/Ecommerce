namespace ECommerce.DTOs.CMSDTOs
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public bool isDisplayHome { get; set; }
        public string catImageUrl { get; set; }
        public string CatUrl { get; set; }
        public string catPageTitle { get; set; }
        public string catDescription { get; set; }
        public string catMetaDescription { get; set; }
    }
}
