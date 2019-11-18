using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Festispec.Models
{
    public class Address : Entity
    {
        public int Id { get; set; }

        public string ZipCode { get; set; }

        public string StreetName { get; set; }

        public int HouseNumber { get; set; }

        public string Suffix { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Festival Festival { get; set; }
    }
}