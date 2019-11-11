namespace Festispec.Models.Exception
{
    public class QuestionnaireNotFoundException : System.Exception
    {

        public QuestionnaireNotFoundException()
        {
        }

        public QuestionnaireNotFoundException(string message) : base(message)
        {
        }

        public QuestionnaireNotFoundException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
