using System.Collections.Generic;
using Newtonsoft.Json;

namespace Festispec.Models.Google
{
    public class Place
    {
        [JsonProperty("address_components")] public List<AddressComponent> AddressComponents { get; set; }

        [JsonProperty("formatted_address")] public string Address { get; set; }

        public Geometry Geometry { get; set; }
    }
}