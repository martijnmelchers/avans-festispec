using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.Models.Exception
{
    public class EmployeeNotSickException : System.Exception
    {
        public EmployeeNotSickException(string message) : base(message)
        {
        }

        public EmployeeNotSickException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        public EmployeeNotSickException()
        {
        }
    }
}
