namespace Festispec.Models.Exception
{
    public class QuestionHasReferencesException : System.Exception
    {
        public QuestionHasReferencesException()
        {
        }

        public QuestionHasReferencesException(string message) : base(message)
        {
        }

        public QuestionHasReferencesException(string message, System.Exception innerException) : base(message,
            innerException)
        {
        }
    }
}