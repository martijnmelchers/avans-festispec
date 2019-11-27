using System.ComponentModel.DataAnnotations;

namespace Festispec.Models
{
    public class FullName
    {
        [Required, MaxLength(40)]
        public string First { get; set; }
        
        [MaxLength(40)]
        public string Middle { get; set; }
        
        [Required, MaxLength(40)]
        public string Last { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Middle) ? $"{First} {Last}" : $"{First} {Middle} {Last}";
        }
    }
}

