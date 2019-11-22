/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6bdca02f-32db-4d20-a75b-02d0fa207b65      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alert.Contract.Data
/////    Project Description:    
/////             Class Name: DeviceAlertStatusType
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/21 15:13:56
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/21 15:13:56
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
    public enum DeviceAlertStatusType
    {
        /// <summary>
        /// 未处置
        /// </summary>
        Unhandle = 1,
        /// <summary>
        /// 已核实
        /// </summary>
        Checked = 2,
        /// <summary>
        /// 已处置
        /// </summary>
        Handled = 3,
    }
}
