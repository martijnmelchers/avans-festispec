using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models
{
    public class ContactPerson : Entity
    {
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string Role { get; set; }

        public FullName Name { get; set; }

        public ContactDetails ContactDetails { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<ContactPersonNote> Notes { get; set; }
    }
}