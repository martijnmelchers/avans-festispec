using System.Globalization;
using System.Windows.Controls;

namespace Festispec.UI.Validation
{
    public class StringsEqualValidationRule : ValidationRule
    {
        public string StringToMatch { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!(value is string stringValue))
                return new ValidationResult(false, "Value is not an string.");

            return stringValue != StringToMatch
                ? new ValidationResult(false, "Strings do not match.")
                : new ValidationResult(true, null);
        }
    }
}