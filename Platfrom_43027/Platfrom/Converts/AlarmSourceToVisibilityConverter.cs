using Gsafety.PTMS.Share;
using System;
using System.Windows;
using System.Windows.Data;

namespace Gsafety.Common.Converts
{
    public class AlarmSourceToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                try
                {
                    int i = int.Parse(value.ToString());
                    if (i == 1)
                    {
                        return Visibility.Visible;
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
