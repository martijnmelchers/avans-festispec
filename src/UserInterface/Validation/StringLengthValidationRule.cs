using System.Globalization;
using System.Windows.Controls;

namespace Festispec.UI.Validation
{
    public class StringLengthValidationRule : ValidationRule
    {
        public int MinLength { get; set; } = 0;

        public int MaxLength { get; set; } = int.MaxValue;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!(value is string stringValue))
                return new ValidationResult(false, "Value is not a string.");

            if (stringValue.Length < MinLength || stringValue.Length > MaxLength)
                return new ValidationResult(false, "String length is out of range.");

            return new ValidationResult(true, null);
        }
    }
}