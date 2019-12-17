using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models
{
    public class Employee : Entity
    {
        public int Id { get; set; }

        public FullName Name { get; set; }

        // https://en.wikipedia.org/wiki/International_Bank_Account_Number#Basic_Bank_Account_Number
        // "Each country can have a different national routing/account numbering system,
        // up to a maximum of 30 alphanumeric characters."
        [Required, MaxLength(30)]
        public string Iban { get; set; }

        [Required]
        public virtual Account Account { get; set; }

        public Address Address { get; set; }

        public ContactDetails ContactDetails { get; set; }

        public virtual ICollection<PlannedEvent> PlannedEvents { get; set; }

        public virtual ICollection<Certificate> Certificates { get; set; }

        public override bool Validate()
        {
            return base.Validate()
                   && Name.Validate()
                   && (Account.Password != null
                       ? Account.Validate(Account.Password)
                       : Account.Validate())
                   && Address.Validate()
                   && ContactDetails.Validate();
        }
    }
}