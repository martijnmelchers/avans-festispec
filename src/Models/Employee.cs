using System.Collections.Generic;

namespace Festispec.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string EmployeeName { get; set; }

        public string Iban { get; set; }

        public virtual Account Account { get; set; }

        public virtual Address Address { get; set; }

        public virtual ContactDetails ContactDetails { get; set; }

        public virtual ICollection<PlannedEvent> PlannedEvents { get; set; }

        public virtual ICollection<Certificate> Certificates { get; set; }
    }
}