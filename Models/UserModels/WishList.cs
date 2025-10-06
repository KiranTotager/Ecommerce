using ECommerce.Models.CMSModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.UserModels
{
    public class WishList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public DateTime AddedAt { get; set; } =DateTime.UtcNow;
        public Customer? Cstmr { get; set; } // navigation property
        public Guid? CustomerId { get; set; } // foreign key
        public Guest? _Guest { get; set; } // navigation property
        public Guid? GuestId { get; set; } // foreign key
        public Product Prdct { get; set; } // navigation property  
        [Required]
        public Guid ProductsId { get; set; } // foriegn key
    }
}
