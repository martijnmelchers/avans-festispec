using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Festispec.UI.Validation
{
    public class DateFormatValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = value as string;
            var match = Regex.Match(input, @"^\d{2}-\d{2}-\d{4}$");
            if (!match.Success)
                return new ValidationResult(false, "Field must be in MM/DD/YYYY format");
            var canParse = DateTime.TryParse(input, out var date);
            if (!canParse)
                return new ValidationResult(false, "Field must be a valid datetime value");
            return date.CompareTo(new DateTime(1970, 01, 01)) != 1 ? new ValidationResult(false, "Date must be later than the year 1970") : new ValidationResult(true, null);
        }
    }
}