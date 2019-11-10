using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.Models
{
    public abstract class Entity
    {
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
