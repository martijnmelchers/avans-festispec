using System.ComponentModel.DataAnnotations;

namespace Festispec.Models
{
    public class ContactPersonNote : Entity
    {
        public int Id { get; set; }

        public ContactPerson ContactPerson { get; set; } 

        [Required, MaxLength(500)]
        public string Note { get; set; }
    }
}