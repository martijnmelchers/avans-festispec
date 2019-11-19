using System;

namespace Festispec.Models
{
    public class ContactPersonNote : Entity
    {
        public int Id { get; set; }

        public ContactPerson ContactPerson { get; set; } 

        public string Note { get; set; }
    }
}