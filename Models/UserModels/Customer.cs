using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.UserModels
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }=Guid.NewGuid();
        public string UserId { get; set; } // foreign key for application user
        public ApplicationUser User{ get; set; } // navigation property

        // Navigation properties
        public List<CartItem> CartItems { get; set; } =new();
        public List<WishList> WishLists { get; set; } = new();
        public List<OrderDetail> OrderDetails { get; set; } =new();
        public List<Review> Reviews { get; set; } =new();
    }
}
