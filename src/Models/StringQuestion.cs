using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Festispec.Models
{
    public class StringQuestion : Question
    {
        public const int CharacterLimit = 400;

        public bool IsMultiline { get; set; }
    }
}