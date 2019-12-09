using System.Windows.Controls;

namespace Festispec.UI.Validation
{
    public class FieldNotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var s = value as string;
            if (string.IsNullOrEmpty(s))
                return new ValidationResult(false, "Field cannot be blank");
            return new ValidationResult(true, null);
        }
    }
}
