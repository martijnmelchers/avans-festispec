namespace Festispec.Models.Exception
{
    public class InvalidDataException : System.Exception
    {
        public InvalidDataException()
        {
        }

        public InvalidDataException(string message) : base(message)
        {
        }

        public InvalidDataException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}