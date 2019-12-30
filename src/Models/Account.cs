using System;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models
{
    public class Account : Entity
    {
        public int Id { get; set; }

        [Required, MinLength(5), MaxLength(45)]
        public string Username { get; set; }

        [Required, MaxLength(100)]
        public string Password { get; set; }

        public virtual Employee Employee { get; set; }

        public DateTime? IsNonActive { get; set; }

        [Required]
        public Role Role { get; set; }

        // Remove any sensitive information that can't be sent to the end user
        public Account ToSafeAccount()
        {
            Password = null;

            return this;
        }

        public bool Validate(string password)
        {
            return password.Length >= 5 && Validate();
        }
    }
}