using Gsafety.PTMS.Bases.Models;
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

namespace Gsafety.PTMS.Monitor.Converts
{
    /// <summary>
    /// Visual state transitions
    /// </summary>
    public class VisibilityStateConvert : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (null != value)
            {
                switch (parameter.ToString())
                {
                    case "Locate":
                    case "Trace":
                        {
                            if ((bool)value)
                            {
                                return Visibility.Visible;

                            }
                            else
                            {
                                return Visibility.Collapsed;
                            }

                        }
                    case "UnSelectGroup":
                        {
                            Vehicle vehicle = value as Vehicle;
                            if (null != vehicle && !String.IsNullOrEmpty(vehicle.GroupID))
                            {
                                return Visibility.Visible;
                            }
                            return Visibility.Collapsed;
                        }
                    case "DriveRouteVisibility":
                        {
                            if (null != value && !String.IsNullOrEmpty(value.ToString()))
                            {
                                int state = (int)value;
                                if (state == 1)
                                {
                                    return Visibility.Collapsed;
                                }
                            }
                            return Visibility.Visible;

                        }
                    case "ListGroupVisibility":
                        {
                            if (null != value && !String.IsNullOrEmpty(value.ToString()))
                            {
                                int count = int.Parse(value.ToString());
                                if (count > 0)
                                {
                                    return Visibility.Visible;
                                }
                            }
                            return Visibility.Collapsed;

                        }
                }

            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
