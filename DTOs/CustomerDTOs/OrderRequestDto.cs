using ECommerce.Enums;

namespace ECommerce.DTOs.CustomerDTOs
{
    public class OrderRequestDto
    {
        public string ConsumerFullName { get; set; }
        public string ConsumerStreet { get; set; }
        public string ConsumerCity { get; set; }
        public string ConsumerState { get; set; }
        public string ConsumerPostalCode { get; set; }
        public string ConsumerPhoneNumber { get; set; }
        public string? CoupenCode { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string ConsumerNote { get; set; }

    }
}
