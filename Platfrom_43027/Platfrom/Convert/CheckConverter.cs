/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 650e0196-11c2-4558-8991-d99a323349c6      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: zhangzl
/////======================================================================
/////           Project Name: Gsafety.Common.Converter
/////    Project Description:    
/////             Class Name: CheckConverter
/////          Class Version: v1.0.0.0
/////            Create Time: 8/15/2013 11:31:23 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/15/2013 11:31:23 AM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Globalization;
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

namespace Gsafety.Common.Converter
{
    public class CheckConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value != null && !(bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value != null && !(bool)value);
        }
    }
}
