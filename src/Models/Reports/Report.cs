using System.Collections.Generic;

namespace Festispec.Models.Reports
{
    public class Report : Entity
    {
        public int Id { get; set; }
        
        public virtual ICollection<ReportEntry> ReportEntries { get; set; }

        public virtual Festival Festival { get; set; }
    }
}