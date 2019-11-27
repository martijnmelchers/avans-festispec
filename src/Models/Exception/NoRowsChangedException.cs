namespace Festispec.Models.Exception
{
    public class NoRowsChangedException : System.Exception
    {
        public NoRowsChangedException(string message) : base(message)
        {
        }

        public NoRowsChangedException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        public NoRowsChangedException()
        {
        }
    }
}
