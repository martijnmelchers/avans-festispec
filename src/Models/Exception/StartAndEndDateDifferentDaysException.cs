namespace Festispec.Models.Exception
{
    public class StartAndEndDateDifferentDaysException : System.Exception
    {
        public StartAndEndDateDifferentDaysException(string message) : base(message)
        {
        }

        public StartAndEndDateDifferentDaysException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        public StartAndEndDateDifferentDaysException()
        {
        }
    }
}