using System.Collections.Generic;

namespace Festispec.Models
{
    public class ContactPerson
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public string ContactPersonName { get; set; }

        public virtual ContactDetails ContactDetails { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<ContactPersonNote> Notes { get; set; }
    }
}