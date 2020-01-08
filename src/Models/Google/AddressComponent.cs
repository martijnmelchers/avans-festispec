using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.Models.Google
{
    public class AddressComponent
    {
        [JsonProperty("long_name")]
        public string LongName { get; set; }
        [JsonProperty("short_name")]
        public string ShortName { get; set; }
        public List<string> Types { get; set; }
    }
}
