using System.Windows.Controls;

namespace Festispec.UI.Validation
{
    public class FieldNotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            return string.IsNullOrEmpty(value as string) 
                ? new ValidationResult(false, "Field cannot be blank") 
                : new ValidationResult(true, null);
        }
    }
}
