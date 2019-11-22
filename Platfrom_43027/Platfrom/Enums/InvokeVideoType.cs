using System;
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
/////Create Time: 2013/11/4
/////Class Description:  
/////======================================================================
/////Modified Time: 2013/11/4
/////Modified by:(ShiHS)
/////Modified Description: 

namespace Gsafety.PTMS.Bases.Enums
{
    /// <summary>
    /// InvokeVideoType
    /// </summary>
    public enum InvokeVideoType : short
    {
        /// <summary>
        ///  CommonRealPlay
        /// </summary>
        [EnumAttribute(ResourceName = "Ep_CommonRealPlay")]
        CommonRealPlay = 1,
        /// <summary>
        /// AlarmRealPlay
        /// </summary>
        [EnumAttribute(ResourceName = "Ep_AlarmRealPlay")]
        AlarmRealPlay = 2,
        /// <summary>
        ///Alarm15sRealPlay
        /// </summary>
        [EnumAttribute(ResourceName = "Ep_Alarm15sRealPlay")]
        Alarm15sRealPlay = 3,
        /// <summary>
        /// HistoryVideoPlay
        /// </summary>
        [EnumAttribute(ResourceName = "Ep_HistoryVideoPlay")]
        HistoryVideoPlay = 4
    }

    public class InvokeVideoTypeConverter : EnumAdapter<InvokeVideoType>, IValueConverter
    {
        IList<EnumInfos> infos = new List<EnumInfos>();
        public InvokeVideoTypeConverter()
        {
            infos = this.GetEnumInfos();     
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            
            if (value != null && value.ToString().Trim() != "DownloadMdvrVideo")
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
