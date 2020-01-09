using System;
using System.Globalization;
using System.Windows.Data;
using Festispec.Models.Questions;

namespace Festispec.UI.Converters
{
    internal class CanEditQuestionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as Question).Id != 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}