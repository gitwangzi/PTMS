using Gsafety.PTMS.Share;
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
    /// <summary>
    /// 时间日期转换器
    /// </summary>
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    var datatTime = (DateTime)value;
                    return datatTime.ToString();
                }
                return string.Empty;
            }
            catch (System.Exception ex)
            {
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 日期转换器
    /// </summary>
    public class DateTimeConverFormate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    var datatTime = (DateTime)value;
                    return datatTime.ToString(ApplicationContext.Instance.ServerConfig.DateFormat);
                }
                return null;
            }
            catch (System.Exception ex)
            {
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 日期时间转换器
    /// </summary>
    public class LongDateTimeFormate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    var datatTime = (DateTime)value;
                    return datatTime.ToString(ApplicationContext.Instance.ServerConfig.LongDateFormat);
                }
            }
            catch (System.Exception ex)
            {
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 时间转换器
    /// </summary>
    public class TimeConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    var datatTime = (DateTime)value;
                    return datatTime.ToString("T");
                }
            }
            catch (System.Exception ex)
            {
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
