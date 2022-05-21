using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using ApiDog.Dto;

namespace DesktopApp
{
    public class SetColorCell : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return new SolidColorBrush(Colors.White);
            string result = value.ToString()?.Trim();
            if (result==("+"))
            {   
                return new SolidColorBrush(Colors.LightGreen);
            }

            if (result.Length > 1 && result.Contains("+"))
            {
                return new SolidColorBrush(Colors.Yellow);
            }
            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}

