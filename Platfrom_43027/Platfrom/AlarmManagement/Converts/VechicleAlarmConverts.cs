/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a3b0a696-8238-42d9-8f4d-ebb03019b78e      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Converts
/////    Project Description:    
/////             Class Name: VechicleAlarmConverts
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/22 14:56:56
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/22 14:56:56
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
    public class VechicleAlarmConverts : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return "/ExternalResource;component/Images/DataGrid_in.png";
            }
            if ((bool)value)
            {
                return "/ExternalResource;component/Images/DataGrid_in_red.png";
            }
            else
            {
                return "/ExternalResource;component/Images/DataGrid_in.png";
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
