namespace Festispec.Models.Exception
{
    public class InvalidAddressException : System.Exception
    {
        public InvalidAddressException(string message) : base(message)
        {
        }

        public InvalidAddressException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        public InvalidAddressException()
        {
        }
    }
}