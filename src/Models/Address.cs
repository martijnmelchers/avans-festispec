using System.ComponentModel.DataAnnotations;

namespace Festispec.Models
{
    public class Address : Validateable
    {
        [Required, MinLength(4), MaxLength(10)]
        public string ZipCode { get; set; }

        [Required, MinLength(1), MaxLength(50)]
        public string StreetName { get; set; }

        [Range(0, int.MaxValue)]
        public int HouseNumber { get; set; }

        [MaxLength(10)]
        public string Suffix { get; set; }

        [Required, MinLength(1), MaxLength(200)]
        public string City { get; set; }

        [Required, MinLength(1), MaxLength(75)]
        public string Country { get; set; }
    }
}