using ECommerce.Models.CMSModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.UserModels
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public string Comments { get; set; }

        public float? Rating { get; set; }
        [Required]
        public DateTime AddedAt { get; set; }
        public Customer? Cstmr { get; set; } // navigation property
        public Guid? CustomerId { get; set; } // foreign key
        public Guest? _Guest { get; set; } // navigation property
        public Guid? GuestId { get; set; } //foreign key
        public Product Prdct { get; set; } // navigation property
        public Guid ProductId { get; set; } // foreign key
    }
}
