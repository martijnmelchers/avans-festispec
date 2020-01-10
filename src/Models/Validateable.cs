using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Festispec.Models.Interfaces;

namespace Festispec.Models
{
    public abstract class Validateable : IValidateable, IDataErrorInfo
    {
        public string Error => null;

        public string this[string columnName] => ValidateProperty(columnName);

        public virtual bool Validate()
        {
            return Validator.TryValidateObject(this, new ValidationContext(this, null, null), null, true);
        }

        private string ValidateProperty(string propertyName)
        {
            PropertyInfo info = GetType().GetProperty(propertyName) ?? throw new InvalidOperationException();
            object value = info.GetValue(this);

            var errorInfos = new List<string>();
            foreach (object attribute in info.GetCustomAttributes(true))
            {
                if (!(attribute is ValidationAttribute va)) continue;

                if (!va.IsValid(value)) errorInfos.Add(va.FormatErrorMessage(string.Empty));
            }

            return errorInfos.Any() ? errorInfos.FirstOrDefault() : null;
        }
    }
}