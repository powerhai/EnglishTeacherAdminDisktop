using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
namespace Fool.Common {
    public class Bool2InverseVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = System.Convert.ToBoolean(value);
            return b ? Visibility.Collapsed : Visibility.Visible;

        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}