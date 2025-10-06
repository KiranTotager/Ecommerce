using ECommerce.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.CMSModels
{
    public class CoupenDetail
    {
        [Key]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public int CId { get; set; }
        [Required]
        public string CType { get; set; }
        [Required]
        public string CValue { get; set; }
        [Required]
        public string CCode { get; set; }
        [Required]
        public DateTime AddedOn { get; set; }    // changed to DateTime
        [Required]
        public string AddedIp { get; set; }
        public DateTime UpdatedOn { get; set; }  // changed to DateTime
        public string UpdatedIp { get; set; }
        [Required]
        public CoupenStatus Status { get; set; }
        [Required]
        public DateTime ExpiryDate { get; set; }
        public List<Staff> Staffs { get; set; } = new(); // navigation property

    }
}
