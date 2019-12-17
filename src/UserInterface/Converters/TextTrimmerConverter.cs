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
    class TextTrimmerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = value.ToString();

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
