using Gsafety.PTMS.Share;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e8e48888-cf33-4ad9-8d6a-d8e5988c1b13      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.Common.Converts
/////    Project Description:    
/////             Class Name: VehicleTreenodeLenght
/////          Class Version: v1.0.0.0
/////            Create Time: 11/26/2013 7:02:43 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 11/26/2013 7:02:43 PM
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
using Gsafety.PTMS.Bases.Models;
namespace Gsafety.Common.Converts
{
    public class VehicleTreenodeLenght : IValueConverter
    {
        public Object Convert(object values, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null)
                return 10;
            else if (values is VehicleEx)
            {
                return 60;
            }
            //else if (values is Vehicle)
            //{
            //    return 55;
            //}

            return 10;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
