/////Copyright (C) Gsafety 2015 .All Rights Reserved.
/////======================================================================
/////                   Guid: b330c854-98eb-454e-b63e-d31aa8f549e6      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Enums
/////    Project Description:    
/////             Class Name: localizeSpeed
/////          Class Version: v1.0.0.0
/////            Create Time: 2015/4/1 10:09:39
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2015/4/1 10:09:39
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
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
using System.Linq;
using Gsafety.PTMS.Bases.Enums;

namespace Gsafety.PTMS.Bases.Enums
{
    public enum localizeSpeed
    {
        ///<summmary>
        ///TRAFFIC_FAILED
        ///</summary>
        [EnumAttribute(ResourceName = "TRAFFIC_FAILED")]
        TRAFFIC_FAILED = 0,
        /////<summmary>
        /////True
        /////</summary>
        //[EnumAttribute(ResourceName = "TRAFFIC_FAILED")]
        //TRAFFIC_FAILED = 1,
        ///<summmary>
        ///TRAFFIC_WAITINTSEND
        ///</summary>
        [EnumAttribute(ResourceName = "TRAFFIC_WAITINTSEND")]
        TRAFFIC_WAITINTSEND = 2,
        ///<summmary>
        ///TRAFFIC_SENDING
        ///</summary>
        [EnumAttribute(ResourceName = "TRAFFIC_SENDING")]
        TRAFFIC_SENDING = 3,

    }
    public class localizeSpeedConverter : EnumAdapter<localizeSpeed>, IValueConverter
    {

        IList<EnumInfos> infoss = new List<EnumInfos>();
        public localizeSpeedConverter()
        {
            infoss = base.GetEnumInfos();
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                int type = System.Convert.ToInt32(value);
                return infoss.Where(f => f.Value.Equals(type)).First().LocalizedString;
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
