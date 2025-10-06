using ECommerce.Models.CMSModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.UserModels
{
    public class OrderedItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Discount { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Tax { get; set; }
        public OrderDetail ODetail { get; set; } // navigation property
        public Guid OrderId { get; set; } // foreign key
        public Product Prdct { get; set; } // navigation property
        public Guid  ProductId { get; set; } // foreign key
    }
}
