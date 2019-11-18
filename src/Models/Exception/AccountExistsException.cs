using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.Models.Exception
{
    public class AccountExistsException : System.Exception
    {
        public AccountExistsException(string message) : base(message)
        {
        }

        public AccountExistsException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        public AccountExistsException()
        {
        }
    }
}
