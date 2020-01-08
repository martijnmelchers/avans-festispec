using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.Models.Google
{
    public class DistanceMatrixResponse
    {
        public string Status { get; set; }
        public List<GoogleRow> Rows { get; set; }

        [JsonProperty("origin_addresses")]
        public List<string> OriginAddresses { get; set; }
        [JsonProperty("destination_addresses")]
        public List<string> DestinationAddresses { get; set; }
    }
}
