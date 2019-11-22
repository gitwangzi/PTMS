using Gsafety.Common.Localization;
using Gsafety.Common.Localization.Resource;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: c06a6fca-d17a-4233-b874-ab244649d1e0      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LANQ
/////                 Author: TEST(lanq)
/////======================================================================
/////           Project Name: Gsafety.Common.Converts
/////    Project Description:    
/////             Class Name: LocalizedStringConverter
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/4 17:35:03
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/4 17:35:03
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
    public class LocalizedStringConverter : IValueConverter
    {
        public string ConvertToString(object value)
        {
            var result = StringResource.ResourceManager.GetString(value.ToString());
            if (result == null)
            {
                result = value.ToString();
            }

            return result;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return ConvertToString(value);
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("unexpected Converback");
        }
    }
}
