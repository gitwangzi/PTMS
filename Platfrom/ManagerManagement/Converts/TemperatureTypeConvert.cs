/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: b43ff724-06f5-4df3-9707-502162845b2f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Converts
/////    Project Description:    
/////             Class Name: TemperatureType
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/17 14:59:49
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/17 14:59:49
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
using System.Collections;
using System.Linq;
using Gsafety.PTMS.Bases.Enums;
using System.Collections.Generic;
namespace Gsafety.PTMS.Manager.Converts
{
    public class TemperatureTypeConvert : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var targetlist = new List<Gsafety.PTMS.Bases.Enums.EnumInfos>();
                targetlist = new EnumAdapter<Gsafety.PTMS.Manager.Models.TemperatureType>().GetEnumInfos().ToList();
                return targetlist.Where(e => e.Value == int.Parse(value.ToString())).FirstOrDefault().LocalizedString;
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
