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
    public class DeliverStatusConvert : IValueConverter
    {
        Dictionary<int, string> resources = new Dictionary<int, string>();
        public DeliverStatusConvert()
        {
            resources.Add(0, ApplicationContext.Instance.StringResourceReader.GetString("UnDelivered"));
            resources.Add(1, ApplicationContext.Instance.StringResourceReader.GetString("WaitDown"));
            resources.Add(2, ApplicationContext.Instance.StringResourceReader.GetString("Downing"));            
            resources.Add(3, ApplicationContext.Instance.StringResourceReader.GetString("DownSuccess"));
            resources.Add(4, ApplicationContext.Instance.StringResourceReader.GetString("DownError"));
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                int intvalue = int.Parse(value.ToString());
                if (resources.ContainsKey(intvalue))
                {
                    return resources[intvalue];
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
