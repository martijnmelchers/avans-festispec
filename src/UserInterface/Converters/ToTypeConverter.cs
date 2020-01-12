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
            return value switch
            {
                DrawQuestion _ => "Teken vraag",
                RatingQuestion _ => "Beoordelings vraag",
                StringQuestion _ => "Open vraag",
                MultipleChoiceQuestion _ => "Meerkeuze vraag",
                UploadPictureQuestion _ => "Foto vraag",
                NumericQuestion _ => "Numerieke vraag",
                ReferenceQuestion _ => "Referentie vraag",
                _ => "vraag"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
