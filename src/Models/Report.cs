using System.Collections;
using System.Collections.Generic;

namespace Festispec.Models
{
    public class Report : Entity
    {
        public int Id { get; set; }
        
        public virtual ICollection<ReportEntry> ReportEntries { get; set; }

        public virtual Festival Festival { get; set; }
    }
}