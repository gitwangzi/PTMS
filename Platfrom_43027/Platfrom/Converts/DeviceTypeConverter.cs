using Gsafety.PTMS.Share;
using System;
using System.Collections.Generic;
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
    public class DeviceTypeConverter : IValueConverter
    {
        Dictionary<int, string> types = null;

        public DeviceTypeConverter()
        {
            types = new Dictionary<int, string>();
            types.Add(0, ApplicationContext.Instance.StringResourceReader.GetString("GNSSModelError"));
            types.Add(1, ApplicationContext.Instance.StringResourceReader.GetString("GNSSNoAntenna"));
            types.Add(2, ApplicationContext.Instance.StringResourceReader.GetString("GNSSCircuit"));
            types.Add(3, ApplicationContext.Instance.StringResourceReader.GetString("PowerSourceNoVoltage"));
            types.Add(4, ApplicationContext.Instance.StringResourceReader.GetString("PowerSourceNoPower"));
            types.Add(5, ApplicationContext.Instance.StringResourceReader.GetString("LEDError"));
            types.Add(6, ApplicationContext.Instance.StringResourceReader.GetString("TTSError"));
            types.Add(7, ApplicationContext.Instance.StringResourceReader.GetString("VidiconError"));
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                int intvalue = System.Convert.ToInt32(value);
                return types[intvalue];
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
