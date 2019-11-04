using System.Collections;
using System.Collections.Generic;

namespace Festispec.Models
{
    public class Report
    {
        public int Id { get; set; }
        
        public virtual ICollection<ReportEntry> ReportEntries { get; set; }

        public Festival Festival { get; set; }
    }
}