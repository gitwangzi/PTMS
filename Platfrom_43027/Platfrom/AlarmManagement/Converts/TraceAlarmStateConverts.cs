/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ad39968a-6623-4c52-81a4-ce1b8729dfc1      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Converts
/////    Project Description:    
/////             Class Name: TraceAlarmStateConvert
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/22 17:10:52
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/22 17:10:52
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

namespace Gsafety.PTMS.Alarm.Converts
{
    public class TraceAlarmStateConverts : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return "/ExternalResource;component/Images/DataGrid_track.png";
            }
            if ((bool)value)
            {

                return "/ExternalResource;component/Images/DataGrid_track1.png";
            }
            else
            {
                return "/ExternalResource;component/Images/DataGrid_track.png";
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
