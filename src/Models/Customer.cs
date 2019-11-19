using System.Collections.Generic;

namespace Festispec.Models
{
    public class Customer : Entity
    {
        public int Id { get; set; }

        public int KvkNr { get; set; }

        public string CustomerName { get; set; }

        public Address Address { get; set; }

        public ContactDetails ContactDetails { get; set; }

        public virtual ICollection<ContactPerson> ContactPersons { get; set; }

        public virtual ICollection<Festival> Festivals { get; set; }
    }
}