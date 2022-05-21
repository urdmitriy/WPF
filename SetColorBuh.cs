using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DesktopApp
{
    public class SetColorBuh : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string datePayed = values[0]?.ToString().Trim();
            string descriptionBuh = values[1]?.ToString().Trim();
            
            
            if (!string.IsNullOrWhiteSpace(descriptionBuh))
            {   if (descriptionBuh.Contains("-"))
                {
                    return new SolidColorBrush(Colors.CornflowerBlue);
                }
                if (!string.IsNullOrEmpty(datePayed))
                {
                    return new SolidColorBrush(Colors.LightGreen);
                }
                
                return new SolidColorBrush(Colors.Khaki);
            }

            return new SolidColorBrush(Colors.White);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}