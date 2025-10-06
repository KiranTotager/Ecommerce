using ECommerce.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.CMSModels
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Description { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Discount { get; set; }
        [Required]
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        public string AddedIp { get; set; }
        public List<CartItem> CartItems { get; set; } = new(); // navigation property
        public List<WishList> WishLists { get; set; } =new(); // navigation property
        public List<OrderedItem> OrderedItems { get; set; } =new(); // navigation property
        public List<Review> Reviews { get; set; } =new(); // navigation property
        public List<Staff> Staffs  { get; set; } =new(); // navigation property
        public  Category Ctgry { get; set; } // navigation property
        public int CategoryId { get; set; } // foreign key
    }
}
