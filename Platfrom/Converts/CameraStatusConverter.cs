using Gsafety.PTMS.Share;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 10fca85b-0f69-46d0-8117-1f849e950fde      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.Common.Converts
/////    Project Description:    
/////             Class Name: CameraStatusConverter
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/22 13:48:47
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/22 13:48:47
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

    public class CameraStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch (value.ToString())
                {
                    case "VL":
                        return ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_Camera_VL");
                    case "POORCNT":
                        return ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_Camera_POORCNT");
                    case "FUZZY":
                        return ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_Camera_FUZZY");
                    case "OK":
                        return ApplicationContext.Instance.StringResourceReader.GetString("OK");
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
