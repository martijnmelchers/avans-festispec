using System;
using System.Globalization;
using System.Windows.Data;

namespace Festispec.UI.Converters
{
    public class UpperCaseStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString().ToUpperInvariant();
        }
    
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}