using System.Collections.Generic;

namespace Festispec.Models
{
    public class Liaison
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public string LiaisonName { get; set; }

        public virtual ContactDetails ContactDetails { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<LiaisonNote> Notes { get; set; }
    }
}