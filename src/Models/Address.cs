﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Festispec.Models
{
    public class Address
    {
        public string ZipCode { get; set; }

        public string StreetName { get; set; }

        public int HouseNumber { get; set; }

        public string Suffix { get; set; }

        public string City { get; set; }

        public string Country { get; set; }
    }
}