using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.Models.Google
{
    public class AutocompleteResponse
    {
        public string Status { get; set; }
        public List<Prediction> Predictions { get; set; }
    }
}
