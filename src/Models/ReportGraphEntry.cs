using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Festispec.Models
{
    public class ReportGraphEntry : ReportEntry
    {
        public GraphXAxisType GraphXAxisType { get; set; }

        public GraphType GraphType { get; set; }

        public string XAxisLabel { get; set; }

        public string YAxisLabel { get; set; }
    }
}