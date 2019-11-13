namespace Festispec.Models.Exception
{
    public class QuestionNotFoundException : System.Exception
    {

        public QuestionNotFoundException()
        {
        }

        public QuestionNotFoundException(string message) : base(message)
        {
        }

        public QuestionNotFoundException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
