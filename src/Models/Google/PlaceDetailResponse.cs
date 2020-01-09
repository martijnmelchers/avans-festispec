using Newtonsoft.Json;

namespace Festispec.Models.Google
{
    public class PlaceDetailResponse
    {
        public string Status { get; set; }

        [JsonProperty("result")] public Place Place { get; set; }
    }
}