using System;

namespace Festispec.Models
{
    public class Account
    {
        public int Id { get; set; }

        public string Password { get; set; }

        public string ActivationCode { get; set; }

        public DateTime IsActivated { get; set; }

        public virtual Employee Employee { get; set; }
    }
}