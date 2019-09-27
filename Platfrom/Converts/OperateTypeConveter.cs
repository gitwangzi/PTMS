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
    public class OperateTypeConveter : IValueConverter
    {
        Dictionary<int, string> types = null;
        public OperateTypeConveter()
        {
            types = new Dictionary<int, string>();
            types.Add(1, ApplicationContext.Instance.StringResourceReader.GetString("RoleManager"));
            types.Add(2, ApplicationContext.Instance.StringResourceReader.GetString("UserManage"));
            types.Add(3, ApplicationContext.Instance.StringResourceReader.GetString("LogManage"));

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
