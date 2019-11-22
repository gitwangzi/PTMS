/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: df760f6e-d7da-4c8d-9da0-adbcbbfe3369      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alert.Converts
/////    Project Description:    
/////             Class Name: VehicleAlertLocationConvert
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/5 15:16:20
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/5 15:16:20
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
    public class VehicleAlertLocationConvert : IValueConverter
    {
        //location
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return "/ExternalResource;component/Images/DataGrid_position.png";
            }
            if ((bool)value)
            {

                return "/AlertManagerment;component/Image/Locate_Add.png";
            }
            else
            {

                return "/ExternalResource;component/Images/DataGrid_position.png";

            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
