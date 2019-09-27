using Gsafety.PTMS.Share;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ad42d178-edb8-487b-acb9-eb6f6f5ae85d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.Common.Converts
/////    Project Description:    
/////             Class Name: GpsStatusConverter
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/22 14:30:39
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/22 14:30:39
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
    public class GpsStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch (value.ToString())
                {
                    case "A":
                        return ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_GPS_A");
                    case "V":
                        return ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_GPS_V");
                    case "N":
                        return ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_RecSD_N");
                    default:
                        return string.Empty;
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
