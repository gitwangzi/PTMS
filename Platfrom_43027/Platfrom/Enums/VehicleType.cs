/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4326f7f9-83fc-452a-b5a1-41c4d17e0aea      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share
/////    Project Description:    
/////             Class Name: VehicleType
/////          Class Version: v1.0.0.0
/////            Create Time: 8/13/2013 9:02:27 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/13/2013 9:02:27 AM
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
using Gsafety.Common.Localization.Resource;

namespace Gsafety.PTMS.Bases.Enums
{
    /// <summary>
    /// VehicleType
    /// </summary>
    public enum VehicleType
    {
        /// <summary>
        /// Taxi
        /// </summary>
        [EnumAttribute(ResourceName = "E4_Taxi")]
        Taxi = 1,
        /// <summary>
        /// Bus
        /// </summary>
        [EnumAttribute(ResourceName = "E4_Bus")]
        Bus = 2,
        /// <summary>
        /// Flota
        /// </summary>
        [EnumAttribute(ResourceName = "E4_Flota")]
        Flota = 3
    }

    public class VehicleConverter : EnumAdapter<VehicleType>, IValueConverter
    {
        IList<EnumInfos> infos = new List<EnumInfos>();
        public VehicleConverter()
        {
            infos = base.GetEnumInfos();   
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            
            if (value != null)
            {
                int type = System.Convert.ToInt32(value);
                string unDefined = StringResource.ResourceManager.GetString("UnDefined");  
                return type == 0 ? (string.IsNullOrEmpty(unDefined) ? "UnDefined" : unDefined) : infos.Where(f => f.Value.Equals(type)).First().LocalizedString;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
