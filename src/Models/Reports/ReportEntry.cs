using System.ComponentModel.DataAnnotations;
using Festispec.Models.Questions;

namespace Festispec.Models.Reports
{
    public abstract class ReportEntry : Entity
    {
        public int Id { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public virtual Question Question { get; set; }

        [Required]
        public virtual Report Report { get; set; }
    }
}