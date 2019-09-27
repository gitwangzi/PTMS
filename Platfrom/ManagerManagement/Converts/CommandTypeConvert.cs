using Gsafety.PTMS.Share;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: dfe697e7-b430-44bf-a41f-9fa205c6606d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Converts
/////    Project Description:    
/////             Class Name: CommandTypeConvert
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/12 9:42:05
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/12 9:42:05
/////            Modified by:
/////   Modified Description: 
/////======================================================================
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

namespace Gsafety.PTMS.Manager.Converts
{
    public class CommandTypeConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string str = string.Empty;
            switch (value.ToString())
            { 
                case "C30":
                    str = ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_GpsSetting");
                    break;
                case "C64":
                    str =  ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_TemperatureSetting");
                    break;
                case "C82":
                    str =  ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_AbnormalDoor_Setting");
                    break;
                case "C78":
                case "C80":
                    str = ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_AlarmSetting");
                    break;
                case "C107":
                    str = ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_ElectricFence");
                    break;
                case "C68":
                    str = ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_SpeedLimit");
                    break;
            }
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
