using System.Globalization;
using System.Windows.Controls;

namespace Festispec.UI.Validation
{
    public class IsIntegerValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return !int.TryParse(value as string, out int _)
                ? new ValidationResult(false, "Field must be an Integer")
                : new ValidationResult(true, null);
        }
    }
}