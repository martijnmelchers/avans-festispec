namespace Festispec.Models.Exception
{
    public class EndDateEarlierThanStartDateException : System.Exception
    {
        public EndDateEarlierThanStartDateException(string message) : base(message)
        {
        }

        public EndDateEarlierThanStartDateException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        public EndDateEarlierThanStartDateException()
        {
        }
    }
}