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
/////             Class Name: GpsUploadType
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/21 10:59:27
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/21 10:59:27
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.Common.Localization.Resource;
using Gsafety.PTMS.Bases.Enums;
using System;
using System.Linq;
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

namespace Gsafety.PTMS.Bases.Enums
{
    public enum GpsUploadType
    {
        ///<summmary>
        ///distance
        ///</summary>
        [EnumAttribute(ResourceName = "MANAGER_GpsSetting_SendType_DistanceValue")]
        MANAGER_GpsSetting_SendType_DistanceValue = 0,
        ///<summmary>
        ///Time
        ///</summary>
        [EnumAttribute(ResourceName = "Rpt_Alarm_Time")]
        Rpt_Alarm_Time = 1,
        ///<summmary>
        ///All
        ///</summary>
        [EnumAttribute(ResourceName = "MANAGER_SendType_Mex")]
        MANAGER_SendType_Mex = 2,
    }
    public class GpsUploadConverter : EnumAdapter<GpsUploadType>, IValueConverter
        {
            IList<EnumInfos> infoss = new List<EnumInfos>();
            public GpsUploadConverter()
            {
                infoss = base.GetEnumInfos();  
            }
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                if (value != null)
                {
                    int type = System.Convert.ToInt32(value);
                    //string unDefined = StringResource.ResourceManager.GetString("UnDefined");
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
