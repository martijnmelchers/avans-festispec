namespace Festispec.Models
{
    public class Attachment
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public byte[] FileContents { get; set; }

        public virtual Answer Answer { get; set; }
    }
}