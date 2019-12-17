using System;

namespace Festispec.UI.Exceptions
{
    public class InvalidNavigationException : Exception
    {
        public InvalidNavigationException()
        {
        }

        public InvalidNavigationException(string message) : base(message)
        {
        }

        public InvalidNavigationException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}