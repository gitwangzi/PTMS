using Gsafety.PTMS.Share;
using System;
using System.Windows;
using System.Windows.Data;

namespace Gsafety.Common.Converts
{
    public class MainPageMenuConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && parameter != null)
            {

                if (ApplicationContext.Instance.AuthenticationInfo.FunctionNames != null)
                {
                    if (ApplicationContext.Instance.AuthenticationInfo.FunctionNames.Contains(parameter.ToString()))
                    {
                        return Visibility.Visible;
                    }
                    else
                    {
                        return Visibility.Collapsed;
                    }
                }
                //lcz修改
                //return Visibility.Collapsed;
                return Visibility.Visible;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
