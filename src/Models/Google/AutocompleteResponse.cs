using System.Collections.Generic;

namespace Festispec.Models.Google
{
    public class AutocompleteResponse
    {
        public string Status { get; set; }
        public List<Prediction> Predictions { get; set; }
    }
}