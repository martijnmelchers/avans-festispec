namespace Festispec.Models.Reports
{
    public class ReportGraphEntry : ReportEntry
    {
        public GraphXAxisType GraphXAxisType { get; set; }

        public GraphType GraphType { get; set; }

        public string XAxisLabel { get; set; }

        public string YAxisLabel { get; set; }
    }
}