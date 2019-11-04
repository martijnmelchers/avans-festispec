using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Festispec.Models
{
    public class DrawQuestion : StringQuestion
    {
        public byte[] FileContents { get; set; }
    }
}