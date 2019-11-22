/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: af3ced33-dfe4-4f1c-9aca-08fa9338c9cb      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(jiaoyx)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share.Enum
/////    Project Description:    
/////             Class Name: BusinessAlertType
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/31 11:20:07
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/31 11:20:07
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
    public enum BusinessAlertType
    {
        /// <summary>
        /// Enter AlertFence
        /// </summary>
        [Enum(ResourceName = "BusinessAlertType_OverSpeed")]
        OverSpeed = 0,
        /// <summary>
        /// Exceed AlertFence
        /// </summary>
        [Enum(ResourceName = "BusinessAlertType_InOutAera")]
        InOutAera = 1,
        /// <summary>
        /// Common OverspeedAlert
        /// </summary>
        [Enum(ResourceName = "BusinessAlertType_InOutRoute")]
        InOutRoute = 2,
        /// <summary>
        /// OverspeedAlert In Fence
        /// </summary>
        [Enum(ResourceName = "BusinessAlertType_RouteOffset")]
        RouteOffset = 3,
        /// <summary>
        /// LowspeedAlert In Fence
        /// </summary>
        [Enum(ResourceName = "BusinessAlertType_FatigueDrive")]
        FatigueDrive = 4,
        /// <summary>
        /// breaking away from projected route Alert
        /// </summary>
        [Enum(ResourceName = "BusinessAlertType_DangerWarning")]//Update
        DangerWarning = 5,
        /// <summary>
        /// OverSpeedAlert（monitoring platform）
        /// </summary>
        [Enum(ResourceName = "BusinessAlertType_OverSpeedWarning")]
        OverSpeedWarning = 6,
        /// <summary>
        /// NormalSpeed （monitoring platform）
        /// </summary>
        [Enum(ResourceName = "BusinessAlertType_FatigueDriveWarning")]
        FatigueDriveWarning = 7,
        /// <summary>
        /// MonitorInRoute（monitoring platform）
        /// </summary>
        [Enum(ResourceName = "BusinessAlertType_DriveTime")]
        DriveTime = 8,
        /// <summary>
        /// MonitorInFenceOverSpeed2Normal（monitoring platform）
        /// </summary>
        [Enum(ResourceName = "BusinessAlertType_VehicleStolen")]
        VehicleStolen = 9,
        /// <summary>
        /// MonitorInFenceUnderSpeed2Normal（monitoring platform）
        /// </summary>
        [Enum(ResourceName = "BusinessAlertType_IllegalIgnition")]
        IllegalIgnition = 10,
        [Enum(ResourceName = "BusinessAlertType_IllegalDisplacement")]
        IllegalDisplacement = 11,
        [Enum(ResourceName = "BusinessAlertType_CollisionWaring")]
        CollisionWaring = 12,
        [Enum(ResourceName = "BusinessAlertType_RolloverWaring")]
        RolloverWaring = 13
    }

    public class BusinessAlertTypeConverter : EnumAdapter<BusinessAlertType>, IValueConverter
    {
        IList<EnumInfos> infos = new List<EnumInfos>();
        public BusinessAlertTypeConverter()
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
    }
}
