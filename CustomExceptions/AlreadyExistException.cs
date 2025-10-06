namespace ECommerce.CustomExceptions
{
    public class AlreadyExistException : Exception
    {
        public AlreadyExistException(string message) :base(message+" already exist")
        { }
    }
}
