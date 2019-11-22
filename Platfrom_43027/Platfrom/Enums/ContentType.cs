using System.ComponentModel;
using System.Windows.Data;
using System.Linq;
using System;
using Gsafety.Common.Localization.Resource;
using System.Collections.Generic;

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
/////Create Time: 2013/10/18 16:57:29
/////Class Description:  
/////======================================================================
/////Modified Time: 2013/10/18 16:57:29
/////Modified by:(ShiHS)
/////Modified Description: 

namespace Gsafety.PTMS.Bases.Enums
{
    /// <summary>
    /// ContentType
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// RealTimeGps
        /// </summary>
        [EnumAttribute(ResourceName = "E8_RealTimeGps")]
        RealTimeGps = 1,
        /// <summary>
        /// HistoryGps
        /// </summary>
        [EnumAttribute(ResourceName = "E8_HistoryGps")]
        HistoryGps = 2,
        ///
        ///Fixed the third test bug No.20 by XiangboLiu on 2015/06/15
        ///begin
        ///     
        /// <summary>
        /// AlarmGps
        /// </summary>
       [EnumAttribute(ResourceName = "E8_AlarmGps")]
        AlarmGps = 3,
        ///<summary>
        ///VEN911
        ///</summary>
       [EnumAttribute(ResourceName = "E8_VEN911")]
       VEN911= 7,
        ///<summary>
        ///ECU911
        ///</summary>
       [EnumAttribute(ResourceName = "E8_ECU911")]
        ECU911 = 9
    }

    public class ContentTypeConverter : EnumAdapter<ContentType>, IValueConverter
    {
        IList<EnumInfos> infos = new List<EnumInfos>();
        public ContentTypeConverter()
        {
            infos = this.GetEnumInfos();   
        }
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //value is enumerate
            if (value != null)
            {
                int type = System.Convert.ToInt32(value);
                string unDefined = StringResource.ResourceManager.GetString("UnDefined");  // database is undefined
                return type == 0 ? (string.IsNullOrEmpty(unDefined) ? "UnDefined" : unDefined) : infos.Where(f => f.Value.Equals(type)).FirstOrDefault().LocalizedString;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
