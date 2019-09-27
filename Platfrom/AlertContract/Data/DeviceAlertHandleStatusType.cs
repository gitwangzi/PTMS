/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0d02f911-88c3-4beb-94bd-0753a3468d1d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alert.Contract.Data
/////    Project Description:    
/////             Class Name: DeviceAlertHandleStatusType
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/21 15:15:21
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/21 15:15:21
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Alert.Contract.Data
{
    //设备告警状态
    public enum DeviceAlertHandleStatusType
    {
        /// <summary>
        /// 已安排
        /// </summary>
        Arranged = 0,
        /// <summary>
        /// 已到维修点
        /// </summary>
        Arrived = 1,
    }
}
