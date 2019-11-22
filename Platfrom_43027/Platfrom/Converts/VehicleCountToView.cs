using Gsafety.PTMS.Share;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 84799a60-5088-4ed2-b205-fd2b614aa3cc      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.Common.Converts
/////    Project Description:    
/////             Class Name: AccStatusConverter
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/22 11:36:17
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/22 11:36:17
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

    public class VehicleCountToView : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                
                switch (value.ToString())
                {
                    case "ON":
                        return ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_ON");
                    case "OFF":
                        return ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_OFF");
                    default:
                        return string.Empty;
                }
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
