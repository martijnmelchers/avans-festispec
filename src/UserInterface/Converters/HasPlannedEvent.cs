using Festispec.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace Festispec.UI.Converters
{
    class HasPlannedEvent : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
           
            ObservableCollection<Employee> EmployeesToAdd = values[0] as ObservableCollection<Employee>;
            return EmployeesToAdd.Contains(values[1] as Employee);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
