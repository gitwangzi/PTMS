/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 03a86289-2bca-4325-a277-f98e5a1da709      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alert.Contract.Data
/////    Project Description:    
/////             Class Name: HandleStatusType
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/12 14:59:11
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/12 14:59:11
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
    //一键报警、设备告警、业务告警处理状态
    public enum HandleStatusType
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
        /// 处置中
        /// </summary>
        Handling = 3,
        /// <summary>
        /// 处置结束
        /// </summary>
        Handled = 4,
    }
}
