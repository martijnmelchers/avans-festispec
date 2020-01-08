namespace Festispec.Models.Exception
{
    public class FestivalHasQuestionnairesException : System.Exception
    {
        public FestivalHasQuestionnairesException(string message) : base(message)
        {
        }

        public FestivalHasQuestionnairesException(string message, System.Exception innerException) : base(message,
            innerException)
        {
        }

        public FestivalHasQuestionnairesException()
        {
        }
    }
}