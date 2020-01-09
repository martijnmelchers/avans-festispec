using System.ComponentModel.DataAnnotations;

namespace Festispec.Models
{
    public class DistanceResult : Entity
    {
        public int Id { get; set; }

        [Required] public Address Origin { get; set; }

        [Required] public Address Destination { get; set; }

        [Required] public double Distance { get; set; }
    }
}