using System;

namespace Festispec.Models
{
    public abstract class Entity : Validateable
    {
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}