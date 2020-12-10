using System;
using System.Globalization;
using System.Windows.Data;

namespace Demo.Sqless.Wpf.Converters
{
    public class OrderStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 1:
                    return "Obligation";
                case 2:
                    return "Done";
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
