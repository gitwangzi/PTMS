/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2460f879-bbeb-444b-9ef6-8b994cba8f79      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Converts
/////    Project Description:    
/////             Class Name: CommandStatusConvert
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/12 15:57:49
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/12 15:57:49
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
using Gsafety.PTMS.ServiceReference.CommandManageService;
using Gsafety.PTMS.Share;

namespace Gsafety.PTMS.Manager.Converts
{
    public class CommandStatusConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            CommandSendStatus status = (CommandSendStatus)value;
            if (status == CommandSendStatus.Success)
                return ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Rule_Setting_Success");
            else
                return ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Rule_Setting_Fail");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
