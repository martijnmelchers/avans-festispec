using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Festispec.UI.Validation
{
    public class TimeFormatValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var input = value as string;
            if (string.IsNullOrEmpty(input))
                return new ValidationResult(false, "Field cannot be blank");
            var match = Regex.Match(input, @"^\d{2}:\d{2}$");
            if (!match.Success)
                return new ValidationResult(false, "Field must be in hh/mm format");
            TimeSpan time;
            var canParse = TimeSpan.TryParse(input, out time);
            if (!canParse)
                return new ValidationResult(false, "Field must be a valid timespan value");
            return new ValidationResult(true, null);
        }
    }
}
