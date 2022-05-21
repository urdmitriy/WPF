using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopApp
{
    public class ConvertDateToShort:IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            if (value.ToString() == DateTime.MinValue.ToString())
            {
                return null;
            }
            var result = ((DateTime)value).ToString("d");
            return result;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}