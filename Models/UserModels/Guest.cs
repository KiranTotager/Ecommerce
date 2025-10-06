using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.UserModels
{
    public class Guest
    {
        [Key]
        public Guid Id { get; set; }=Guid.NewGuid();
        public string? Name { get; set; }
        public string? Phone { get; set; }
        [EmailAddress]
        public string? EmailId { get; set; }
        public string? Address { get; set; }

        public List<CartItem> CartItems { get; set; } = new();//navigation property
        public List<WishList> WishLists { get; set; } =new(); //navigation property
        public List<OrderDetail> OrderDetails { get; set; } =new(); //navigation property
        public List<Review> Reviews { get; set; }  =new();//navigation property
    }
}
