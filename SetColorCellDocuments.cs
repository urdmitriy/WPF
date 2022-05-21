using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DesktopApp
{
    public class SetColorCellDocuments:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return new SolidColorBrush(Colors.White);
            if (value.ToString().Trim() == "+.")
                return new SolidColorBrush(Colors.LightGreen);
            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}