using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Festispec.UI.Validation
{
    public class TimeFormatValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = value as string;
            if (string.IsNullOrEmpty(input))
                return new ValidationResult(false, "Field cannot be blank");
            Match match = Regex.Match(input, @"^\d{2}:\d{2}$");
            if (!match.Success)
                return new ValidationResult(false, "Field must be in hh/mm format");
            bool canParse = TimeSpan.TryParse(input, out TimeSpan _);
            return !canParse ? new ValidationResult(false, "Field must be a valid timespan value") : new ValidationResult(true, null);
        }
    }
}