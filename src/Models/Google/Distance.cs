using Newtonsoft.Json;

namespace Festispec.Models.Google
{
    public class Distance
    {
        [JsonProperty("text")]
        public string DistanceText { get; set; }
        [JsonProperty("value")]
        public int DistanceValue { get; set; }
    }
}