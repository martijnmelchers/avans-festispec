using System.ComponentModel.DataAnnotations;

namespace Festispec.Models
{
    public class Address : Validateable
    {
        public int Id { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(10)]
        public string ZipCode { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string StreetName { get; set; }

        [Range(0, int.MaxValue)] public int HouseNumber { get; set; }

        [MaxLength(10)] public string Suffix { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(200)]
        public string City { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(75)]
        public string Country { get; set; }

        [Required] public float Latitude { get; set; }

        [Required] public float Longitude { get; set; }

        public override string ToString()
        {
            return HouseNumber == 0 && string.IsNullOrEmpty(StreetName) ? $"{City} {Country}" :
                HouseNumber == 0 ? $"{StreetName}, {City} {Country}" :
                $"{StreetName} {HouseNumber}{Suffix}, {City} {Country}";
        }
    }
}