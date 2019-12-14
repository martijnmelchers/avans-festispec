using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.Models.Google
{
    public class PlaceDetailResponse
    {
        public string Status { get; set; }

        [JsonProperty("result")]
        public Place Place { get; set; }
    }
}
