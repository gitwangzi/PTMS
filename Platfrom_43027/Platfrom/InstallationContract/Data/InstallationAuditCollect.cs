/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4bf2eb48-fb1d-48df-81e3-4361dc500840      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Contract.Data
/////    Project Description:    
/////             Class Name: InstallationAuditCollect
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/26 16:53:57
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/26 16:53:57
/////            Modified by:
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
    public class InstallationAuditCollect
    {
        /// <summary>
        /// License plate number
        /// </summary>
        [DataMember]
        public string VehicleID { get; set; }
        /// <summary>
        /// Security Suite No.
        /// </summary>
        [DataMember]
        public string SuiteID { get; set; }
        /// <summary>
        /// Self-test results
        /// </summary>
        [DataMember]
        public int? SelfInspectCheck { get; set; }
        /// <summary>
        /// A key alarm verification results
        /// </summary>
        [DataMember]
        public int? AlarmCheck { get; set; }
        /// <summary>
        /// GPS data audit results
        /// </summary>
        [DataMember]
        public int? GpsCheck { get; set; }
        /// <summary>
        /// Video test validation results
        /// </summary>
        [DataMember]
        public int? VideoCheck { get; set; }
        /// <summary>
        /// Center Video Quality Audit
        /// </summary>
        [DataMember]
        public int? VideoQualityCheck { get; set; }
        /// <summary>
        /// By installing examine whether
        /// </summary>
        [DataMember]
        public int? IsSuccess { get; set; }
        /// <summary>
        /// Approver
        /// </summary>
        [DataMember]
        public string Approver { get; set; }
        /// <summary>
        /// Approval Date
        /// </summary>
        [DataMember]
        public Nullable<DateTime> ApproverTime { get; set; }
        /// <summary>
        /// Approval Description
        /// </summary>
        [DataMember]
        public string Content { get; set; }
    }
}
