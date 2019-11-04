using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Festispec.Models
{
    public class RatingQuestion : Question
    {
        public string LowRatingDescription { get; set; }

        public string HighRatingDescription { get; set; }
    }
}