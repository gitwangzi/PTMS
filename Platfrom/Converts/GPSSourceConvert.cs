using Gs.PTMS.Common.Data.Enum;
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
    public class GPSSourceConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch ((GPSSourceEnum)(value))
                {
                    case GPSSourceEnum.GPS:
                        return "/ExternalResource;component/Images/GPSSource_GPS.png";
                    case GPSSourceEnum.Suite:
                        return "/ExternalResource;component/Images/GPSSource_Suite.png";
                    case GPSSourceEnum.Mobile:
                        return "/ExternalResource;component/Images/GPSSource_Mobile.png";
                    default:
                        return "/ExternalResource;component/Images/GPSSource_Suite.png";
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
