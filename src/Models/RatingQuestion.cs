using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Festispec.Models
{
    public class RatingQuestion : NumericQuestion
    {
        public string LowRatingDescription { get; set; }

        public string HighRatingDescription { get; set; }
    }
}