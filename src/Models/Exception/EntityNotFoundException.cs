namespace Festispec.Models.Exception
{
    public class EntityNotFoundException : System.Exception
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}