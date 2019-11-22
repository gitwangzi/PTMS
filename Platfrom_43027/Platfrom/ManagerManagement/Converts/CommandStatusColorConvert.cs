/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 10b307f3-a12f-42bc-9ef3-7196dcce1f2c      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Converts
/////    Project Description:    
/////             Class Name: CommandStatusColorConvert
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/12 15:52:33
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/12 15:52:33
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

namespace Gsafety.PTMS.Manager.Converts
{
    public class CommandStatusColorConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string color = string.Empty;
            CommandSendStatus status = (CommandSendStatus)value;
            switch (status)
            { 
                case CommandSendStatus.Success:
                    color = "Green";
                    break;
                case CommandSendStatus.Failure:
                    color = "Red";
                    break;
            }
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
