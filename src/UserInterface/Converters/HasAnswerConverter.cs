using System;
using System.Globalization;
using System.Windows.Data;
using Festispec.Models.Questions;

namespace Festispec.UI.Converters
{
    internal class HasAnswerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var question = value as Question;

            return question?.AnswerCount == 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}