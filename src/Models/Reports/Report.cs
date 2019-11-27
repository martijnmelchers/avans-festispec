using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models.Reports
{
    public class Report : Entity
    {
        public int Id { get; set; }
        
        public virtual ICollection<ReportEntry> ReportEntries { get; set; }

        [Required]
        public virtual Festival Festival { get; set; }
    }
}