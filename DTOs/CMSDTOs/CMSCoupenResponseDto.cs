using ECommerce.Enums;

namespace ECommerce.DTOs.CMSDTOs
{
    public class CMSCoupenResponseDto
    {
        public string CoupenType { get; set; }
        public string CoupenCode { get; set; }
        public string CoupenValue { get; set; }
        public CoupenStatus CoupenStatus { get; set; }
        public DateTime CoupenExpiryDate { get; set; }
    }
}
