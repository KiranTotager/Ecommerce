namespace ECommerce.CustomExceptions
{
    public class DeletionException : Exception
    {
        public DeletionException(string message) : base($"exception occurred while deleting {message}") { }
    }
}
