/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 43858cd0-fb98-4c23-b7ed-925e6df95a8d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LANQ
/////                 Author: TEST(lanq)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Contract.Data
/////    Project Description:    
/////             Class Name: SecuritySuiteStatusType
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 10:18:20
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/22 10:18:20
/////            Modified by: BilongLiu
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInformation.Contract.Data
{
    /// <summary>
    /// Device Suite Status
    /// </summary>
    public enum DeviceSuiteStatus
    {
        /// <summary>
        /// All
        /// </summary>
        None = 0,
        /// <summary>
        /// Initial
        /// </summary>
        Initial = 10,
        /// <summary>
        /// Working
        /// </summary>
        Working = 20,
        /// <summary>
        /// Testing
        /// </summary>
        Testing = 22,
        /// <summary>
        /// Running
        /// </summary>
        Running = 23,
        /// <summary>
        /// Abnormal
        /// </summary>
        Abnormal = 24,
        /// <summary>
        /// WaitingMaintenance
        /// </summary>
        WaitingMaintenance = 25,
        /// <summary>
        /// Maintenance
        /// </summary>
        Maintenance = 30,
        /// <summary>
        /// Scrap
        /// </summary>
        Scrap = 40,
        /// <summary>
        /// History
        /// </summary>
        History = 99,
    }
}
