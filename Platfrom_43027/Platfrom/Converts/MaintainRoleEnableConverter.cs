using Gsafety.PTMS.Bases.Enums;
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

namespace Gsafety.Common.Converts
{
    public class MaintainRoleEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                int i = (int)value;
                //if (i != (int)RoleCategory.MaintainAdmin && i != (int)RoleCategory.MaintainMonitor)
                //{
                return true;
                //}
            }
            catch (System.Exception ex)
            {
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
