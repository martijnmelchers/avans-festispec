using System.ComponentModel.DataAnnotations;

namespace Festispec.Models.Answers
{
    public class Attachment : Entity
    {
        public int Id { get; set; }

        [Required]
        public string FilePath { get; set; }

        [Required]
        public virtual Answer Answer { get; set; }
    }
}