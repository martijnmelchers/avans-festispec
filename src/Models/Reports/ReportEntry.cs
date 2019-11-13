using Festispec.Models.Questions;

namespace Festispec.Models.Reports
{
    public abstract class ReportEntry : Entity
    {
        public int Id { get; set; }

        public int Order { get; set; }

        public virtual Question Question { get; set; }

        public virtual Report Report { get; set; }
    }
}