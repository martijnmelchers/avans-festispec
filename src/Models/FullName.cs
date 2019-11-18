namespace Festispec.Models
{
    public class FullName
    {
        public int Id { get; set; }

        public string First { get; set; }
        public string Middle { get; set; }
        public string Last { get; set; }

        public virtual ContactPerson ContactPerson { get; set; }

        public virtual Employee Employee { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Middle) ? $"{First} {Last}" : $"{First} {Middle} {Last}";
        }
    }
}

