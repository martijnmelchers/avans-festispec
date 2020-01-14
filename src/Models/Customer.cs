using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models
{
    public class Customer : Entity
    {
        public Customer()
        {
            ContactDetails = new ContactDetails();
        }

        public int Id { get; set; }

        [Required] public int KvkNr { get; set; }

        [Required] [MaxLength(20)] public string CustomerName { get; set; }

        [MaxLength(500)] public string Notes { get; set; }

        public Address Address { get; set; }
        public ContactDetails ContactDetails { get; set; }
        public virtual ICollection<Festival> Festivals { get; set; }
    }
}