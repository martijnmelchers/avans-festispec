using System.ComponentModel.DataAnnotations;
using Festispec.Models.Interfaces;

namespace Festispec.Models
{
    public abstract class Validateable : IValidateable
    {
        public bool Validate()
        {
            return Validator.TryValidateObject(this, new ValidationContext(this, null, null), null, true);
        }
    }
}