/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 1dd579e3-4aae-44bf-9a81-fe43049983e1      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.Common.Converters
/////    Project Description:    
/////             Class Name: VehicleTypeToImage
/////          Class Version: v1.0.0.0
/////            Create Time: 8/16/2013 4:54:34 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/16/2013 4:54:34 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;

namespace Gsafety.Common.Converters
{
    public class VehicleTypeToImage : IValueConverter
    {
        readonly string RootPath = "/MainPage;component/Images/";
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                return string.Format("{0}{1}", RootPath, "Bus.png");
            }
            return string.Format("{0}{1}", RootPath, "Bus.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
