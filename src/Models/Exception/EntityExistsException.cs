using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.Models.Exception
{
    public class EntityExistsException : System.Exception
    {
        public EntityExistsException(string message) : base(message)
        {
        }

        public EntityExistsException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        public EntityExistsException()
        {
        }
    }
}
