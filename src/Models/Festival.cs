using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Festispec.Models
{
    public class Festival
    {
        public int Id { get; set; }

        public string FestivalName { get; set; }

        public string Description { get; set; }
        
        public virtual Address Address { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<OpeningHours> OpeningHours { get; set; }

        public virtual ICollection<PlannedInspection> PlannedInspections { get; set; }

        public virtual ICollection<Report> Reports { get; set; }
    }
}