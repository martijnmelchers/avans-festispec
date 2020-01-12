namespace Festispec.Models.Exception
{
    public class NotAuthorizedException : System.Exception
    {
        public NotAuthorizedException(string message) : base(message)
        {
        }

        public NotAuthorizedException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        public NotAuthorizedException()
        {
        }
    }
}