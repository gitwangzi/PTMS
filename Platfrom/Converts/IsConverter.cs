using Gsafety.PTMS.Share;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: f087f48d-942f-4ca8-8057-60dc97f31716      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.Common.Converts
/////    Project Description:    
/////             Class Name: IsConverter
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/23 14:43:38
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/23 14:43:38
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

namespace Gsafety.Common.Converts
{
    public class IsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = string.Empty;

            if (value != null)
            {
                switch (value.ToString())
                {
                    case "1":
                        return ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_Unhandled");
                    case "2":
                        return ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_Verified");
                    case "3":
                        return ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_handled");
                    default:
                        return "";
                }
            }
            return s;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return parameter.ToString();
            }
            return null;
        }
    }
}
