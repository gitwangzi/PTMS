using Gsafety.Common.Localization.Resource;
using System;
using System.ComponentModel;
using System.Windows.Data;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

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
    [Flags]
    public enum UserRole : short
    {
        /// <summary>
        /// E9_PTMS
        /// </summary>
        [EnumAttribute(ResourceName = "E9_PTMS")]
        Monitor = 1,
        /// <summary>
        /// E9_SecurityManager
        /// </summary>
        [EnumAttribute(ResourceName = "E9_SecurityManager")]
        SecurityManager = 2,
        /// <summary>
        /// InstallStation
        /// </summary>
        [EnumAttribute(ResourceName = "E9_InstallStation")]
        InstallStation = 3,
        /// <summary>
        /// Maintenance
        /// </summary>
        [EnumAttribute(ResourceName = "E9_Maintenance")]
        Maintenance = 4,
        /// <summary>
        /// Arads
        /// </summary>
        [EnumAttribute(ResourceName = "E9_Arads")]
        Arads = 5,
        /// <summary>
        /// Other
        /// </summary>
        [EnumAttribute(ResourceName = "E9_Other")]
        Other = 6,
        /// <summary>
        /// AlarmFilterCommissioner
        /// </summary>
        [EnumAttribute(ResourceName = "E9_AlarmFilter")]
        AlarmFilterCommissioner = 7,
    }

    public class UserRoleConverter : EnumAdapter<UserRole>, IValueConverter
    {
        IList<EnumInfos> infos = new List<EnumInfos>();
        public UserRoleConverter()
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
