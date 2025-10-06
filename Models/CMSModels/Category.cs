using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.CMSModels
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public bool DisplayHome { get; set; }
        public string? ImageUrl  { get; set; }
        [Required]
        public string CategoryUrl { get; set; }
        [Required]
        [MaxLength(200)]
        public string PageTitle { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [Required]
        [MaxLength(250)]
        public string MetaDesc { get; set; }
        [Required]
        public int DisplayOrder { get; set; }
        public List<Product> Products { get; set; } = new();  //navigation property
        public List<Staff> Staffs { get; set; } = new(); //navigation property

    }
}
