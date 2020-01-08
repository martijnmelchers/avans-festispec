using Newtonsoft.Json;
using System.Collections.Generic;

namespace Festispec.Models.Google
{
    public class Prediction
    {
        public string Description { get; set; }

        [JsonProperty("distance_meters")]
        public int DistanceMeters { get; set; }

        public string Id { get; set; }
        
        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        public string Reference { get; set; }

        public List<string> Types { get; set; }
    }
}