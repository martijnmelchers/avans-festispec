namespace Festispec.Models
{
    public class Attachment : Entity
    {
        public int Id { get; set; }

        public string FilePath { get; set; }

        public virtual Answer Answer { get; set; }
    }
}