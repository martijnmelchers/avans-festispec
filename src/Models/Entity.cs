using Festispec.Models.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models
{
    public abstract class Entity : IValidateable
    {
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool Validate()
        {
            return Validator.TryValidateObject(this, new ValidationContext(this, null, null), null, true);     
        }
    }
}