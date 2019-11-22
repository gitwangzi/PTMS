/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 125c7ef8-4e3a-4c72-802c-74aceac8489a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.Convert
/////    Project Description:    
/////             Class Name: MarkGraphicConvert
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/24 9:32:06
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/24 9:32:06
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

namespace Gsafety.PTMS.Traffic.Converts
{
    public class Mark_Point_Convert : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (null != value)
            {
                if ((bool)value)
                {

                    return "/ExternalResource;component/Images/Mark_Point.png";

                }
                else
                {
                    return "/ExternalResource;component/Images/Un_Mark_Point.png";

                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
    
    public class Mark_Polygon_Convert : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (null != value)
            {
                if ((bool)value)
                {

                    return "/ExternalResource;component/Images/DataGrid_in_red.png";

                }
                else
                {
                    return "/ExternalResource;component/Images/DataGrid_in.png";

                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
