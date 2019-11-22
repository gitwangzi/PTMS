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
using Gsafety.PTMS.Share;

namespace Gsafety.Common.Converts
{
    public class TargetNullValueConverter : IValueConverter
    {
        private string _defaultStr = null;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value as string) == false)
            {
                return value;
            }

            if (_defaultStr == null)
            {
                _defaultStr = ApplicationContext.Instance.StringResourceReader.GetString(parameter as string);
            }
            return _defaultStr;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var str = value as string;

            if (_defaultStr == null)
            {
                _defaultStr = ApplicationContext.Instance.StringResourceReader.GetString(parameter as string);
            }

            if (str.Contains(_defaultStr))
            {
                str = str.Replace(_defaultStr, "");
            }

            return str;
        }
    }
}
