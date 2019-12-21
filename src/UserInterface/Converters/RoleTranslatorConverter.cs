using System;
using System.Globalization;
using System.Windows.Data;
using Festispec.Models;

namespace Festispec.UI.Converters
{
    public class RoleTranslatorConverter: IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (!(value is Role role))
                    return null;

                return role switch
                {
                    Role.Employee => "Medewerker",
                    Role.Inspector => "Inspecteur",
                    _ => "Onbekende rol"
                };
            }
    
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (!(value is string roleString))
                    return null;

                return roleString switch
                {
                    "Medewerker" => Role.Employee,
                    "Inspecteur" => Role.Inspector,
                    _ => null
                };
            }
    }
}