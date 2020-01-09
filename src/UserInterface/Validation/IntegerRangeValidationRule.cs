using System;
using System.Globalization;
using System.Windows.Controls;

namespace Festispec.UI.Validation
{
    public class IntegerRangeValidationRule : ValidationRule
    {
        public int Min { get; set; } = 0;
        public int Max { get; set; } = int.MaxValue;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int intValue;
            
            switch (value)
            {
                case string stringValue:
                {
                    if (!int.TryParse(stringValue, out intValue))
                        return new ValidationResult(false, "Value is not an integer.");
                    break;
                }
                
                case int integerValue:
                    intValue = integerValue;
                    break;
                
                default:
                    return new ValidationResult(false, "Value is not an integer.");
            }

            if (intValue < Min || intValue > Max)
                return new ValidationResult(false, "Integer is out of range.");

            return new ValidationResult(true, null);
        }
    }
}