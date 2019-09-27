/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 844f9d6f-b295-4ce7-8284-0a4faee1e46e      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(jiaoyx)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Contract
/////    Project Description:    
/////             Class Name: InstallInfoResult
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/1 14:53:10
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/21 14:53:10
/////            Modified by: BilongLiu
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Installation.Contract
{
    [DataContract]
    public class InstallInfoResult : InstallationInfo
    {
        /// <summary>
        /// Installation Information
        /// </summary>
        [DataMember]
        public InstallationInfo Installation { get; set; }
        /// <summary>
        /// Audit information
        /// </summary>
        [DataMember]
        public InstallationAudit Audit { get; set; }     
    }
}
