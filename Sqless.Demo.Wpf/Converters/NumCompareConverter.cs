using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Sqless.Demo.Wpf
{
    internal class NumCompareConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var param = parameter as string;
            var symbol = param[1] == '=' ? param.Substring(0, 2) : param.Substring(0, 1);
            var num = param.Substring(symbol.Length, param.Length - symbol.Length);

            bool compareValue;
            switch (symbol)
            {
                case ">":
                    compareValue = (int)value > int.Parse(num);
                    break;
                case "=":
                    compareValue = (int)value == int.Parse(num);
                    break;
                case "<":
                    compareValue = (int)value < int.Parse(num);
                    break;
                case "<=":
                    compareValue = (int)value <= int.Parse(num);
                    break;
                case ">=":
                    compareValue = (int)value >= int.Parse(num);
                    break;
                default:
                    throw new NotSupportedException();
            }

            if (targetType == typeof(bool))
            {
                return compareValue;
            }
            else if (targetType == typeof(Visibility))
            {
                return compareValue ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
