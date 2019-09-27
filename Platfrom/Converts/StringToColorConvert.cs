/////Copyright (C)2014Microsoft All Rights Reserved.
/////======================================================================
/////                   Guid: ce55064b-6e1e-4d76-9c8a-68b4c5a1af3d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-JIANGJ
/////                 Author: TEST(JiangJ)
/////======================================================================
/////           Project Name: Gsafety.Common.Converts
/////    Project Description:    
/////             Class Name: StringToColorConvert
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/11 15:27:40
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/11 15:27:40
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
using System.Linq;
using Gsafety.Common.CommMessage;

namespace Gsafety.Common.Converts
{
    public class StringToColorConvert : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            PredefinedColor si = PredefinedColors.PredefinedColorCollection.Where(x => x.Name == value.ToString()).FirstOrDefault();
            return new SolidColorBrush(si.Value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
