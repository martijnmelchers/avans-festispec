namespace Festispec.Models.Exception
{
    public class GoogleZeroResultsException : System.Exception
    {
        public GoogleZeroResultsException(string message) : base(message)
        {
        }

        public GoogleZeroResultsException(string message, System.Exception innerException) : base(message,
            innerException)
        {
        }

        public GoogleZeroResultsException()
        {
        }
    }
}