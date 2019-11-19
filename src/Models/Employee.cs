﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models
{
    public class Employee : Entity
    {
        public int Id { get; set; }

        public FullName Name { get; set; }

        [Required, MaxLength(30)]
        public string Iban { get; set; }

        public virtual Account Account { get; set; }

        public Address Address { get; set; }

        public ContactDetails ContactDetails { get; set; }

        public virtual ICollection<PlannedEvent> PlannedEvents { get; set; }

        public virtual ICollection<Certificate> Certificates { get; set; }
    }
}