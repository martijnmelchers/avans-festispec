using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Festispec.Models;

namespace Festispec.UI.Converters
{
    internal class HasQuestionsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool invert = bool.Parse(parameter as string ?? "false");

            if (!(value is Questionnaire questionnaire))
                return Visibility.Hidden;

            if (invert)
                return questionnaire.Questions.Sum(x => x.AnswerCount) == 0 ? Visibility.Hidden : Visibility.Visible;

            return questionnaire.Questions.Sum(x => x.AnswerCount) == 0 ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}