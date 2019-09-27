using Gsafety.PTMS.Bases.Enums;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 973a869c-a746-41da-be8c-63a303f63599      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Converts
/////    Project Description:    
/////             Class Name: TemperatureSetTypeConvert
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/17 11:14:46
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/17 11:14:46
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
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
namespace Gsafety.PTMS.Manager.Converts
{
    public class TemperatureSetTypeConvert : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var targetlist = new List<Gsafety.PTMS.Bases.Enums.EnumInfos>();
                targetlist = new EnumAdapter<Gsafety.PTMS.Manager.Models.TemperatureSettingType>().GetEnumInfos().ToList();
                return targetlist.Where(e => e.Name == value.ToString()).FirstOrDefault().LocalizedString;
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
