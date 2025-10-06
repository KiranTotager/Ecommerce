namespace ECommerce.CustomExceptions
{
    public class NotFoundException :Exception
    {
        public NotFoundException(string message) : base(message+" not found")
        {

        }
    }
}
