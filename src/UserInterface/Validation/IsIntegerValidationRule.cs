using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Festispec.UI.Validation
{
    public class IsIntegerValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var s = value as string;
            int integer;
            var canParse = int.TryParse(s, out integer);
            if (!canParse)
                return new ValidationResult(false, "Field must be an Integer");
            return new ValidationResult(true, null);
        }
    }
}
