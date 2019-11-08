using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Festispec.Models
{
    public class ContactPersonNote
    {
        public int ContactPersonId { get; set; }

        public ContactPerson ContactPerson { get; set; } 

        public DateTime Created { get; set; } 

        public string Note { get; set; }
    }
}