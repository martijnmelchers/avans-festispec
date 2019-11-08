using System;

namespace Festispec.Models
{
    public class Account
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ActivationCode { get; set; }

        public DateTime IsActivated { get; set; }

        public virtual Employee Employee { get; set; }

        // Remove any sensitive information that can't be sent to the end user
        public Account ToSafeAccount()
        {
            Password = null;

            return this;
        }
    }
}