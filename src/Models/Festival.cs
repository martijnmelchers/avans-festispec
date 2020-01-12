using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models
{
    public class Festival : Entity
    {
        public int Id { get; set; }

        [Required] [MaxLength(45)] public string FestivalName { get; set; }

        [Required] [MaxLength(250)] public string Description { get; set; }

        [Required] public Address Address { get; set; }

        [Required] public virtual Customer Customer { get; set; }
        
        [Required] public OpeningHours OpeningHours { get; set; }

        public virtual ICollection<PlannedInspection> PlannedInspections { get; set; }

        public virtual ICollection<Questionnaire> Questionnaires { get; set; }
    }
}