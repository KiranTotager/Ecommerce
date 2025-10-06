using ECommerce.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.DTOs.CMSDTOs
{
    public class AddCoupenDetailsDTO
    {
        [Required]
        [SwaggerSchema("please provide the type of coupen here")]
        public string CoupenType { get; set; }
        [Required]
        [SwaggerSchema("this property is for the coupen code ")]
        public string CoupenCode { get; set; }
        [Required]
        [SwaggerSchema("this property is for the coupen value ")]
        public string CoupenValue { get; set; }
        [Required]
        [SwaggerSchema("this property is for the coupen status ")]
        public CoupenStatus CoupenStaus { get; set; }
        [Required]
        [SwaggerSchema("this property is for the coupen Expiry date ")]
        public DateTime ExpiryDate { get; set; }
    }
}
