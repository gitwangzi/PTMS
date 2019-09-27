using Gsafety.PTMS.Share;
using System;
using System.Globalization;
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
    public class PlayMusicConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {

            if (value != null)
            {
                var isChecked = System.Convert.ToBoolean(value, culture);
                if (isChecked)
                    return ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_Alarm_CloseMusic");
                else
                    return ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_Alarm_PlayMusic");
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
