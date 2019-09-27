/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: f36b1e5e-bb0f-4ad9-a231-908e4ed28a76      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.Common.Converts
/////    Project Description:    
/////             Class Name: BoolToVisibility
/////          Class Version: v1.0.0.0
/////            Create Time: 8/17/2013 10:06:07 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/17/2013 10:06:07 AM
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

namespace Gsafety.Common.Converts
{
    public class BoolToVisibility : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {

            Visibility IsShow = Visibility.Collapsed;
            if (value != null)
            {
                var isChecked = System.Convert.ToBoolean(value, culture);
                if (isChecked)
                    IsShow = Visibility.Visible;
            }
            return IsShow;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
  
}
