using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Festispec.Models
{
    public class NumericQuestion : Question
    {
        public int Minimum { get; set; }

        public int Maximum { get; set; }

        // bijv. Meter, personen, etc.
        public AnswerUnit Unit { get; set; }
    }
}