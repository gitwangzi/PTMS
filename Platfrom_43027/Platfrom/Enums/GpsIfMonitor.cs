/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 8fda5cf5-c566-46cb-8902-ae4f1af25ed7      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Enums
/////    Project Description:    
/////             Class Name: GpsIfMonitor
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/21 10:59:27
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/21 10:59:27
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

namespace Gsafety.PTMS.Bases.Enums
{
    public enum GpsIfMonitor
    {
        ///<summmary>
        ///True
        ///</summary>
        [EnumAttribute(ResourceName = "Real")]
        TRUE = 1,
        ///<summmary>
        ///Flase
        ///</summary>
        [EnumAttribute(ResourceName = "NotTrue")]
        FALSE = 0,
    }
    public class GpsMonitorConverter : EnumAdapter<GpsIfMonitor>, IValueConverter
    {
        IList<EnumInfos> infoss = new List<EnumInfos>();
        public GpsMonitorConverter()
        {
            infoss = base.GetEnumInfos();
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                int type = System.Convert.ToInt32(value);             
                return infoss.Where(f => f.Value.Equals(type)).First().LocalizedString;
            }
            return string.Empty;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
