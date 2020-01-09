namespace Festispec.Models.Exception
{
    public class CustomerHasContactPersonsException : System.Exception
    {
        public CustomerHasContactPersonsException()
        {
        }

        public CustomerHasContactPersonsException(string message) : base(message)
        {
        }

        public CustomerHasContactPersonsException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}