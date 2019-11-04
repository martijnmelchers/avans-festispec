namespace Festispec.Models
{
    public abstract class ReportEntry
    {
        public int Id { get; set; }

        public int Order { get; set; }

        public Question Question { get; set; }

        public Report Report { get; set; }
    }
}