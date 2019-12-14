using Newtonsoft.Json;

namespace Festispec.Models.Google
{
    public class Location
    {
        [JsonProperty("lat")]
        public float Latitude { get; set; }

        [JsonProperty("lng")]
        public float Longitude { get; set; }
    }
}