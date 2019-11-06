using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Festispec.Models
{
    public class LiaisonNote
    {
        public int LiaisonId { get; set; }
        public Liaison Liaison { get; set; } 

        public DateTime Created { get; set; } 

        public string Note { get; set; }
    }
}