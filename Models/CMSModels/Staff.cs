using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.CMSModels
{
    public class Staff
    {
        [Key]
        public Guid Id { get; set; }=Guid.NewGuid();
        [Required]
        public string UserId { get; set; } // foreign key for application user
        public ApplicationUser User { get; set; } // navigation property
        public List<Product> Products { get; set; } = new(); //navigation property
        public List<Category> Categories { get; set; } =new(); //navigation property
        public List<CoupenDetail> CoupenDetails { get; set; } =new(); //navigation property
    }
}
