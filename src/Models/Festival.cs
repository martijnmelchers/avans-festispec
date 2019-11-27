using Festispec.Models.Reports;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models
{
    public class Festival : Entity
    {
        public Festival(string festivalname, string description, Customer customer)
        {
            FestivalName = festivalname;
            Description = description;
            Customer = customer;
        }

        public Festival() { }

        public int Id { get; set; }

        [Required, MaxLength(45)]
        public string FestivalName { get; set; }

        [Required, MaxLength(250)]
        public string Description { get; set; }
        
        public Address Address { get; set; }

        [Required]
        public virtual Customer Customer { get; set; }

        public virtual Report Report { get; set; }

        public OpeningHours OpeningHours { get; set; }

        public virtual ICollection<PlannedInspection> PlannedInspections { get; set; }

        public virtual ICollection<Questionnaire> Questionnaires { get; set; }
    }
}