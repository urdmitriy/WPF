using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopApp
{
    public class NullDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == DateTime.MinValue.ToString())
            {
                return "";
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}