using Gsafety.PTMS.Share;
using System;
using System.Windows.Data;

namespace Gsafety.Common.Converts
{
    public class ServiceTypeConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (value != null)
            {
                switch (value.ToString())
                {
                    case "Comercial":
                        return ApplicationContext.Instance.StringResourceReader.GetString("Comercial");
                        break;
                    case "Public":
                        return ApplicationContext.Instance.StringResourceReader.GetString("Public");
                        break;

                    case "Private":
                        return ApplicationContext.Instance.StringResourceReader.GetString("Private");
                        break;
                    case "Unknown":
                        return ApplicationContext.Instance.StringResourceReader.GetString("Unkown");
                        break;
                    default:
                        return string.Empty;

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
