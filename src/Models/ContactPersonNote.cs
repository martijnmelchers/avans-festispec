using System;

namespace Festispec.Models
{
    public class ContactPersonNote : Entity
    {
        public int ContactPersonId { get; set; }

        public ContactPerson ContactPerson { get; set; } 

        public DateTime Created { get; set; } 

        public string Note { get; set; }
    }
}