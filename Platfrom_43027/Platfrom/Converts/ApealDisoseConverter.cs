using Gsafety.PTMS.Share;
using System;
using System.Windows.Data;

namespace Gsafety.Common.Converts
{
    public class ApealDisoseConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                try
                {
                    int result = int.Parse(value.ToString());

                    if (result == 4)
                    {
                        //已处置
                        return ApplicationContext.Instance.StringResourceReader.GetString("Disposed");
                    }
                    return ApplicationContext.Instance.StringResourceReader.GetString("NoDisposed");
                }
                catch (Exception)
                {
                    return ApplicationContext.Instance.StringResourceReader.GetString("NoDisposed");
                }
            }
            return ApplicationContext.Instance.StringResourceReader.GetString("NoDisposed");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
