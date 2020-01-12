using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Festispec.Models.Questions;

namespace Festispec.UI.Converters
{
    class HideButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as Question).AnswerCount > 0 ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
