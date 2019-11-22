using Gsafety.PTMS.Share;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 23eb7abc-7ae5-40dd-83ec-906d622c523d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LILF
/////                 Author: TEST(lilf)
/////======================================================================
/////           Project Name: Gsafety.Common.CommMessage
/////    Project Description:    
/////             Class Name: RequestVehicleMonitorArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/7 14:21:44
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/7 14:21:44
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.Bases.Enums;

namespace Gsafety.Common.CommMessage
{
    /// <summary>
    /// 点播车辆时发送过来的消息类型
    /// </summary>
    public class RequestVehicleMonitorArgs : EventArgs
    {
        public string CarNo
        {
            get;
            set;
        }
        /// <summary>
        /// MDVRID
        /// Device Unique Key
        /// </summary>
        public string UniqueId
        {
            get;
            set;
        }

        /// <summary>
        /// operator type
        /// </summary>
        public int Op
        {
            get;
            set;
        }

        public VehicleType CarStyle
        {
            get;
            set;
        }
        /// <summary>
        /// provice
        /// </summary>
        public string Department { get; set; }

        public bool IsAlarm { get; set; }

        public bool IsAlert { get; set; }

        public RequestVehicleMonitorArgs()
        {
            IsAlarm = false;
            IsAlert = false;
            IsGetLastGPS = false;
        }

        public bool IsGetLastGPS { get; set; }
    }
}
