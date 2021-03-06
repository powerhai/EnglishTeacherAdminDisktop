using System;
using System.Globalization;
using System.Windows.Data;
namespace Fool.Common {
    public class Bool2InverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = System.Convert.ToBoolean(value);
            return !b;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}