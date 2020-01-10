namespace Festispec.Models.Exception
{
    public class WrongQuestionTypeException : System.Exception
    {
        public WrongQuestionTypeException(string message) : base(message)
        {
        }

        public WrongQuestionTypeException(string message, System.Exception innerException) : base(message,
            innerException)
        {
        }

        public WrongQuestionTypeException()
        {
        }
    }
}