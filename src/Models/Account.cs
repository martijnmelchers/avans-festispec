using System;

namespace Festispec.Models
{
    public class Account : Entity
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public virtual Employee Employee { get; set; }

        public EmployeeRole EmployeeRole { get; set; }

        // Remove any sensitive information that can't be sent to the end user
        public Account ToSafeAccount()
        {
            Password = null;

            return this;
        }
    }
}