using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.Common.Converts
{
    public class ChannelConverter : IValueConverter
    {
        public ChannelConverter() { }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var realValue = System.Convert.ToString(value);
            var result = -1;
            if (int.TryParse(realValue, out result))
            {
                result += 1;
            }
            else
            {
                result = 1;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var realValue = System.Convert.ToString(value);
            var result = -1;
            if (int.TryParse(realValue, out result))
            {
                result -= 1;
            }
            else
            {
                result = 0;
            }
            return result;
        }
    }
}
