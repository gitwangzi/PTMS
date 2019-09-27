/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 04fbb59a-ce27-4e52-9bef-715d8f3f8a37      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.Ant.Message.Contract.Data.Alarm
/////    Project Description:    
/////             Class Name: ReturnStatus
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/27 11:44:05
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/27 11:44:05
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.Ant.Message.Contract.Data
{
    /// <summary>
    /// 911回复
    /// </summary>
    public class ReturnStatus
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 接警ID
        /// </summary>
        public string IncidentAppealId { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg { get; set; }
    }

}
