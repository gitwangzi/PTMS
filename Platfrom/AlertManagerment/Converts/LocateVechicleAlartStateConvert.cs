/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: eabf8977-83e7-4dc1-9959-6e47a1e48a47      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alert.Converts
/////    Project Description:    
/////             Class Name: LocateVechicleAlartStateConvert
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/14 17:07:01
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/14 17:07:01
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

namespace Gsafety.PTMS.Alert.Converts
{
    /// <summary>
    /// Icon of site of incident
    /// </summary>
    public class LocateVechicleAlartStateConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value==null)
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
