namespace ECommerce.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string UserId { get; set; }
        public string  AuthToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
