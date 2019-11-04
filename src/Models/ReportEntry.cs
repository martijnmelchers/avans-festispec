namespace Festispec.Models
{
    public abstract class ReportEntry
    {
        public int Id { get; set; }

        public int Order { get; set; }

        public virtual Question Question { get; set; }

        public virtual Report Report { get; set; }
    }
}