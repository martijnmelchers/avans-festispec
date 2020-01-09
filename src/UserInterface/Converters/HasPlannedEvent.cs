using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Festispec.Models;

namespace Festispec.UI.Converters
{
    internal class HasPlannedEvent : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is ICollection<PlannedInspection> plannedInspections))
                return false;

            return plannedInspections.Any(pi => pi.Employee.Id == (int) values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}