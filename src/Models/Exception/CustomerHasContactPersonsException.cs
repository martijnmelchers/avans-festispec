using System;

namespace Festispec.DomainServices.Services
{
    public class CustomerHasContactPersonsException : Exception
    {
        public CustomerHasContactPersonsException()
        {
        }

        public CustomerHasContactPersonsException(string message) : base(message)
        {
        }

        public CustomerHasContactPersonsException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}