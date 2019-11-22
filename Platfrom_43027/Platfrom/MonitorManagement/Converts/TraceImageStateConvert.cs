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

namespace Gsafety.PTMS.Monitor.Converts
{
    /// <summary>
    /// Vehicle Tracking button icon conversion
    /// </summary>
    public class TraceImageStateConvert : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (null != value)
            {
                if ((bool)value)
                {

                    return "/ExternalResource;component/Images/DataGrid_track1.png";

                }
                else
                {
                    return "/ExternalResource;component/Images/DataGrid_track.png";
                
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
