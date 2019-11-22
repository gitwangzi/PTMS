/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: c87cbdeb-932e-4a95-bc9f-5ac168f726e8      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.Common.Converts
/////    Project Description:    
/////             Class Name: IMultiValueConverter
/////          Class Version: v1.0.0.0
/////            Create Time: 9/24/2013 2:34:22 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 9/24/2013 2:34:22 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.Common.Converts
{
    /// <summary>
    /// see: http://msdn.microsoft.com/en-us/library/system.windows.data.imultivalueconverter.aspx
    /// </summary>
    public interface IMultiValueConverter
    {
        object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);
    }
}
