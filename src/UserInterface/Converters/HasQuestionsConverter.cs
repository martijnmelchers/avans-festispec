using Festispec.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Festispec.UI.Converters
{
    class HasQuestionsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var invert = bool.Parse(parameter as string ?? "false");

            if (!(value is Questionnaire questionnaire))
                return Visibility.Hidden;

            if (invert)
                return questionnaire.Questions.Sum(x => x.AnswerCount) == 0 ? Visibility.Hidden : Visibility.Visible;

            return questionnaire.Questions.Sum(x => x.AnswerCount) == 0  ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
