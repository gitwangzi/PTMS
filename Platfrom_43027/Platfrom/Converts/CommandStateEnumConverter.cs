using Gsafety.PTMS.Share;
using System.Windows.Data;

namespace Gsafety.Common.Converts
{
    public class CommandStateEnumConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ApplicationContext.Instance.StringResourceReader.GetString(value.ToString());
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
