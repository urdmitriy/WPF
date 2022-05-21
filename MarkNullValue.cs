using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DesktopApp
{
    public class MarkNullValue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value.ToString().Trim()) == (new DateTime(DateTime.Now.Year, 1, 1)).ToString())
            {
                return new SolidColorBrush(Colors.Yellow);
            }
            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}