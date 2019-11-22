using Gsafety.PTMS.Share;
using System;
using System.Windows.Data;
using System.Windows.Media;
namespace Gsafety.Common.Converts
{
    public class BoolCoverterToColorConverter : IValueConverter
    {
        SolidColorBrush whiteColorBrush = new SolidColorBrush(Colors.White);
        SolidColorBrush yellowColorBrush = new SolidColorBrush(Colors.Yellow);
        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var valid = (bool)value;
            //solidColorBrush.Color = "#f0f0f0".ToColor();
            if (!valid)
            {
                return yellowColorBrush;
            }
            return whiteColorBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}
