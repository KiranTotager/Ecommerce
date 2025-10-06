namespace ECommerce.CustomExceptions
{
    public class EmailNotConfirmedException : Exception
    {
        public EmailNotConfirmedException(string emailId) : base($"{emailId} is not confirmed") { }
    }
}
