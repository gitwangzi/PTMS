using System;
using System.Windows.Data;

namespace Gsafety.Common.Converts
{
    public class MarkConvert : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (null != value)
            {
                if ((bool)value)
                {

                    //return "/ExternalResource;component/Images/Mark_Point.png";
                    return "/ExternalResource;component/Images/UnNormalPlot.png";
                }
                else
                {
                    //return "/ExternalResource;component/Images/Un_Mark_Point.png";
                    return "/ExternalResource;component/Images/NormalPlot.png";
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
