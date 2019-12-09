using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Festispec.UI.Validation
{
    public class DateFormatValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var s = value as string;
            var match = Regex.Match(s, @"^\d{2}/\d{2}/\d{4}$");
            if (!match.Success)
                return new ValidationResult(false, "Field must be in MM/DD/YYYY format");
            DateTime date;
            var canParse = DateTime.TryParse(s, out date);
            if (!canParse)
                return new ValidationResult(false, "Field must be a valid datetime value");
            if(date.CompareTo(new DateTime(1970, 01, 01)) != 1)
                return new ValidationResult(false, "Date must be later than the year 1970");
            return new ValidationResult(true, null);
        }
    }
}
