using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using Festispec.Models;

namespace Festispec.UI.Converters
{
    internal class HasPlannedEvent : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var EmployeesToAdd = values[0] as ObservableCollection<Employee>;
            var EmployeesAdded = values[1] as ObservableCollection<Employee>;
            return EmployeesToAdd.Contains(values[2] as Employee) || EmployeesAdded.Contains(values[2] as Employee);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}