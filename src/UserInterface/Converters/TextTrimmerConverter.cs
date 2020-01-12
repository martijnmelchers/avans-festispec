using System;
using System.Globalization;
using System.Windows.Data;

namespace Festispec.UI.Converters
{
    internal class TextTrimmerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value.ToString();

            try
            {
                int length = int.Parse(parameter.ToString());

                if (result.Length > length)
                    result = result.Substring(0, length) + "...";
            }
            catch
            {
                result += "...";
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}