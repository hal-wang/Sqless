using System;
using System.Globalization;
using System.Windows.Data;

namespace Hubery.Sqless.Demo.Wpf.Converters
{
    public class TimestampConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timestamp = (long)value;
            var localTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).LocalDateTime;
            if (string.IsNullOrEmpty(parameter as string))
            {
                return localTime.ToString();
            }
            else
            {
                return localTime.ToString(parameter as string);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
