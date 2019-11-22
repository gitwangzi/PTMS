using Gsafety.PTMS.Share;
using System;
using System.Windows.Data;

namespace Gsafety.Common.Converts
{
    public class AlarmSourceToImageConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value != null)
            {
                try
                {
                    int i = int.Parse(value.ToString());
                    switch(i)
                    {
                        case 0:
                            return "/ExternalResource;component/Images/NormalDeviceAlarm.png";
                            break;
                        case 1:
                            return "/ExternalResource;component/Images/NormalMobileAlarm.png";
                            break;
                        case 2:
                            return "/ExternalResource;component/Images/NormalHandAlarm.png";
                            break;
                        default:
                            break;
                    }
                }
                catch(Exception ex)
                {
                    ApplicationContext.Instance.Logger.LogException("AlarmSourceToImageConverter", ex);
                    return "";
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
