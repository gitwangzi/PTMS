using Gsafety.Common.Localization.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
/////Create Time: 2013/10/17 
/////Class Description:  
/////======================================================================
/////Modified Time: 2013/10/17
/////Modified by:(ShiHS)
/////Modified Description: 

namespace Gsafety.PTMS.Bases.Enums
{
    /// <summary>
    /// DeviceAlertType(enum14)
    /// </summary>
    public enum DeviceAlertType : int
    {
        /// <summary>
        /// OverTemperature（DeviceReport）
        /// </summary>
        [EnumAttribute(ResourceName = "E14_OverTemperature", Description = "Description", Flag = "flag")]
        OverTemperature = 11,
        /// <summary>
        /// GpsFault（DeviceReport）
        /// </summary>
        [Description("E14_GpsFault")]
        [EnumAttribute(ResourceName = "E14_GpsFault")]
        GpsFault = 12,
        /// <summary>
        /// VedioShelter(DeviceReport）
        /// </summary>
        [EnumAttribute(ResourceName = "E14_VedioShelter")]
        VedioShelter = 13,
        /// <summary>
        /// VedioNoSignal(DeviceReport）
        /// </summary>
        [EnumAttribute(ResourceName = "E14_VedioNoSignal")]
        VedioNoSignal = 14,
        /// <summary>
        /// AbnormalFire (DeviceReport）
        /// </summary>
        [EnumAttribute(ResourceName = "E14_AbnormalFire")]
        AbnormalFire = 15,
        /// <summary>
        /// MDVRSdFault(DeviceReport）
        /// </summary>
        [EnumAttribute(ResourceName = "E14_SdFault")]
        SdFault = 16,
        /// <summary>
        /// PasswordFault(DeviceReport）
        /// </summary>
        [EnumAttribute(ResourceName = "E14_PasswordFault")]
        PasswordFault = 17,
        /// <summary>
        /// AbnormalValtage(DeviceReport）
        /// </summary>
        [EnumAttribute(ResourceName = "E14_AbnormalValtage")]
        AbnormalValtage = 18,

        [EnumAttribute(ResourceName = "ManualAlert")]
        Manual = 99,
       
    }

    public class DeviceAlertTypeConverter : EnumAdapter<DeviceAlertType>, IValueConverter
    {
        IList<EnumInfos> infos = new List<EnumInfos>();
        public DeviceAlertTypeConverter()
        {
            infos = this.GetEnumInfos();
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                int type = System.Convert.ToInt32(value);
                return infos.Where(f => f.Value.Equals(type)).First().LocalizedString;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object Convert(object value)
        {
            if (value != null)
            {
                int type = System.Convert.ToInt32(value);
                return infos.Where(f => f.Value.Equals(type)).First().LocalizedString;
            }
            return string.Empty;
        }
    }
    public class DeviceAlertConvert : EnumAdapter<DeviceAlertType>
    {
        IList<EnumInfos> infos = new List<EnumInfos>();
        public DeviceAlertConvert()
        {
            infos = this.GetEnumInfos();
        }
        public string Convert(object value)
        {
            if (value != null)
            {
                int type = System.Convert.ToInt32(value);
                return infos.Where(f => f.Value.Equals(type)).First().LocalizedString;
            }
            return string.Empty;
        }
    }


}
