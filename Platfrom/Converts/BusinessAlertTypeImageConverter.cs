using Gsafety.PTMS.Bases.Enums;
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
    public class BusinessAlertTypeImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int key = int.Parse(value.ToString());
            var result = string.Empty;
            switch (key)
            {
                case (int)BusinessAlertType.OverSpeed:
                    result = "/ExternalResource;component/Images/OverSpeed.png";
                    break;
                case (int)BusinessAlertType.InOutAera:
                    result = "/ExternalResource;component/Images/InOutAera.png";
                    break;
                case (int)BusinessAlertType.InOutRoute:
                    result = "/ExternalResource;component/Images/InOutRoute.png";
                    break;
                case (int)BusinessAlertType.RouteOffset:
                    result = "/ExternalResource;component/Images/RouteOffset.png";
                    break;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
