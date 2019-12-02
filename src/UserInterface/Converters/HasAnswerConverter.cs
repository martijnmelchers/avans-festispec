using Festispec.DomainServices.Interfaces;
using Festispec.Models.Questions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace Festispec.UI.Converters
{
    class HasAnswerConverter : IValueConverter
    {
        //private IQuestionnaireService _questionnaireService = 

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var question = value as Question;

            return question.AnswerCount == 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
