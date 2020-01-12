using System;
using System.Globalization;
using System.Windows.Data;
using Festispec.Models.Questions;

namespace Festispec.UI.Converters
{
    class ToTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case DrawQuestion _:
                    return "Teken vraag";
                case RatingQuestion _:
                    return "Beoordelings vraag";
                case StringQuestion _:
                    return "Open vraag";
                case MultipleChoiceQuestion _:
                    return "Meerkeuze vraag";
                case UploadPictureQuestion _:
                    return "Foto vraag";
                case NumericQuestion _:
                    return "Numerieke vraag";
                case ReferenceQuestion _:
                    return "Referentie vraag";



                default:
                    return "vraag";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
