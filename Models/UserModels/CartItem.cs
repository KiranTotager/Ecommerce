using ECommerce.Models.CMSModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.UserModels
{
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public long Id { get; set; } 
        [Required]
        [Range(1,int.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        public DateTime AddedAt { get; set; }

        // Many-to-one: Customer
        public Customer Customer { get; set; }  // navigation property
        public Guid? CustomerId { get; set; }   // foreign key

        public Guest _Guest { get; set; } // navigation property
        public Guid? GuestId { get; set; } //foreign key

        // Many-to-one: Product
        public Product Product { get; set; }    // navigation property
        [Required]
        public Guid ProductId { get; set; }    // foreign key
    }
}
