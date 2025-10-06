namespace ECommerce.CustomExceptions
{
    public class SecurityTokenException : Exception
    {
        public SecurityTokenException(string message):base(message) { }
    }
}
