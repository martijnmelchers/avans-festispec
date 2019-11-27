using System.ComponentModel.DataAnnotations;

namespace Festispec.Models
{
    public class ContactDetails
    {
        [MaxLength(50)]
        public string PhoneNumber { get; set; }

        [MaxLength(50)]
        public string EmailAddress { get; set; }
    }
}