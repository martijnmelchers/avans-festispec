using System.Windows.Controls;

namespace Festispec.UI.Validation
{
    public class FieldNotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var input = value as string;
            if (string.IsNullOrEmpty(input))
                return new ValidationResult(false, "Field cannot be blank");
            return new ValidationResult(true, null);
        }
    }
}
