namespace Festispec.Models.Exception
{
    public class QuestionHasAnswersException : System.Exception
    {

        public QuestionHasAnswersException()
        {
        }

        public QuestionHasAnswersException(string message) : base(message)
        {
        }

        public QuestionHasAnswersException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
