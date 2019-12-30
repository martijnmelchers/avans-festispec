namespace Festispec.Models.Exception
{
    public class DateHasPassedException : System.Exception
    {
        public DateHasPassedException(string message) : base(message)
        {
        }

        public DateHasPassedException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        public DateHasPassedException()
        {
        }
    }
}