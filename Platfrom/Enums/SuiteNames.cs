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

/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
///// Guid: 9d8c2ee2-20b4-434b-ae6b-6f354a75cb63      
/////clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
///// Machine Name: PC-ShiHS
///// Author: TEST(ShiHongSheng)
/////======================================================================
/////Project Name:
/////Project Description:    
/////Class Name: 
/////Class Version: v1.0.0.0
/////Create Time: 2013/12/3
/////Class Description:  
/////======================================================================
/////Modified Time: 2013/10/17
/////Modified by:(ShiHS)
/////Modified Description: 

namespace Gsafety.PTMS.Bases.Enums
{
    public enum SuiteNames
    {
        [Enum(ResourceName = "BASEINFO_MDVR_SN", Description = "CEIEC MDVR_SN")]
        MdvrSN,

        [Enum(ResourceName = "BASEINFO_MDVR_CORE_SN", Description = "MDVR MDVR_CORE_SN")]
        MdvrCoreSN,

        [Enum(ResourceName = "BASEINFO_PTMS_Camera1", Description = "PTMS_Camera1")]
        Camera1Id,

        [Enum(ResourceName = "BASEINFO_PTMS_Camera2", Description = "PTMS_Camera2")]
        Camera2Id,

        [Enum(ResourceName = "BASEINFO_AlarmButton1", Description = "AlarmButton1")]
        AlarmButton1Id,

        [Enum(ResourceName = "BASEINFO_AlarmButton2", Description = "AlarmButton2")]
        AlarmButton2Id,

        [Enum(ResourceName = "BASEINFO_AlarmButton3", Description = "AlarmButton3")]
        AlarmButton3Id,

        [Enum(ResourceName = "BASEINFO_UPS", Description = "UPS")]
        UpsId,

        [Enum(ResourceName = "BASEINFO_SdCardId", Description = "SdCardId")]
        SdCardId,

        [Enum(ResourceName = "BASEINFO_DOOR_SWITCH_SENSOR", Description = "DOOR_SWITCH_SENSOR")]
        DoorSensorId
    }

    public class SuiteNamesConverter : EnumAdapter<SuiteNames>, IValueConverter
    {
        IList<EnumInfos> infos = new List<EnumInfos>();
        public SuiteNamesConverter()
        {
            infos = this.GetEnumInfos();   
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                return infos.Where(f => f.Name.Equals(value.ToString())).First().LocalizedString;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
