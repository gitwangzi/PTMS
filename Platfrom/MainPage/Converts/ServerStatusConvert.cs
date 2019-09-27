/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ac34bbf7-d2d3-448a-adbd-a696a34c69a5      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.Common.Converts
/////    Project Description:    
/////             Class Name: AlertTypeColorConvert
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/12 14:30:40
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/12 14:30:40
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
using System.Linq;
using Gsafety.PTMS.ServiceReference.AppConfigService;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Share;
namespace Gsafety.PTMS.MainPage.Converts
{
    public class ServerStatusConvert : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            MessageServiceStatus status = (MessageServiceStatus)value;
            string serverStatusDescript = ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_ServiceConnect");
            if (status == MessageServiceStatus.Connected)
            {
                serverStatusDescript = ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_ServiceConnect");
            }
            else if (status == MessageServiceStatus.DisConnected)
            {
                serverStatusDescript = ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_ServiceDisConnect");
            }
            else if (status == MessageServiceStatus.RequestConnect)
            {
                serverStatusDescript = ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_ServiceConnecting");
            }

            return serverStatusDescript;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
