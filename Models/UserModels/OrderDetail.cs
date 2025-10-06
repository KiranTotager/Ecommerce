using ECommerce.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.UserModels
{
    public class OrderDetail
    {
        [Key]
        public Guid OrderId { get; set; }=Guid.NewGuid();
        [Required]
        public string TransactionId { get; set; }
        //public string Name { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        [Required]
        public PaymentMethod PaymentMethod { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public PaymentStatus PaymentStatus { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string PhooneNumber { get; set; }
        [Required]
        public string Note { get; set; }
        // Many-to-one: Customer
        public Customer? Customer { get; set; }       // navigation property
        public Guid? CustomerId { get; set; }       // foreign key

        public Guest? _Guest { get; set; } //navigation property
        public Guid? GuestId { get; set; } //foreign key

        // One-to-many: OrderedItems
        public List<OrderedItem> OrderedItems { get; set; } = new();// navigation property
    }
}
