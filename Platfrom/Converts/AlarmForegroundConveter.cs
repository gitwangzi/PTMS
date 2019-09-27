using Gsafety.PTMS.ServiceReference.VehicleAlarmService;
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
    public class AlarmForegroundConveter : IValueConverter
    {
        private static string red = "#e84548";
        private static string gray = "#eeeeee";
        private static string yellow = "#ffcc33";
        private static string blue = "#0648ca";

        private static string wrongColor = "#242930";

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            AlarmInfoEx alarminfo = value as AlarmInfoEx;
            if (alarminfo != null)
            {
                if (alarminfo.IsAlive)
                {
                    if (alarminfo.IsDesignated)
                    {
                        //处置完成
                        if (alarminfo.AppealStatus == 4)
                        {
                            //实时 分配给 已处置 
                            return blue;
                        }
                        else
                        {
                            //实时 分配给 未处置 
                            return red;
                        }
                    }
                    else
                    {
                        if (alarminfo.AppealStatus == 4)
                        {
                            return blue;
                        }
                        else
                        {
                            //实时 不是分配给 未处置 
                            return yellow;
                        }
                    }
                }
                else
                {
                    if (alarminfo.IsDesignated)
                    {
                        if (alarminfo.AppealStatus == 4)
                        {
                            return blue;
                        }
                        else
                        {
                            return red;
                        }
                    }
                    else
                    {
                        if (alarminfo.AppealStatus == 4)
                        {
                            //历史 自己已处置 
                            return blue;
                        }
                        else if (string.IsNullOrEmpty(alarminfo.User))
                        {
                            //历史 未处置  未分配的
                            return gray;
                        }
                        else
                        {
                            //历史 未处置  分配给别人的
                            return yellow;
                        }
                    }
                }
            }

            return wrongColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AlarmForegroundImageConveter : IValueConverter
    {
        private static string red = "/ExternalResource;component/Images/AlarmMyself.png";
        private static string gray = "/ExternalResource;component/Images/AlarmNoUser.png";
        private static string yellow = "/ExternalResource;component/Images/AlarmOthers.png";
        private static string blue = "/ExternalResource;component/Images/AlarmHandled.png";

        private static string wrongColor = "";

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            AlarmInfoEx alarminfo = value as AlarmInfoEx;
            if (alarminfo != null)
            {
                if (alarminfo.IsAlive)
                {
                    if (alarminfo.IsDesignated)
                    {
                        //处置完成
                        if (alarminfo.AppealStatus == 4)
                        {
                            //实时 分配给 已处置 
                            return blue;
                        }
                        else
                        {
                            //实时 分配给 未处置 
                            return red;
                        }
                    }
                    else
                    {
                        if (alarminfo.AppealStatus == 4)
                        {
                            return blue;
                        }
                        else
                        {
                            //实时 不是分配给 未处置 
                            return yellow;
                        }
                    }
                }
                else
                {
                    if (alarminfo.IsDesignated)
                    {
                        if (alarminfo.AppealStatus == 4)
                        {
                            return blue;
                        }
                        else
                        {
                            return red;
                        }
                    }
                    else
                    {
                        if (alarminfo.AppealStatus == 4)
                        {
                            //历史 自己已处置 
                            return blue;
                        }
                        else if (string.IsNullOrEmpty(alarminfo.User))
                        {
                            //历史 未处置  未分配的
                            return gray;
                        }
                        else
                        {
                            //历史 未处置  分配给别人的
                            return yellow;
                        }
                    }
                }
            }

            return wrongColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
