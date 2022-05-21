using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DesktopApp
{
    public class MarkNotEmptyDescriptionBuh : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return new SolidColorBrush(Colors.White);
            if (!string.IsNullOrEmpty(value.ToString()))
            {
                return new SolidColorBrush(Colors.Orchid);
            }
            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new object();
        }
    }
}