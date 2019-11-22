/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ae93cf6c-750c-4069-82df-1207b8b09f4a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(jiaoyx)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Contract
/////    Project Description:    
/////             Class Name: InstallationAudit
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/1 14:33:27
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/21 14:33:27
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
    public class InstallationAudit
    {
        /// <summary>
        /// Id primary key installation records
        /// </summary>
        [DataMember]
        public string Id { get; set; }
        /// <summary>
        /// Self-test information unique number
        /// </summary>
        [DataMember]
        public string SelfInspectId { get; set; }
        /// <summary>
        /// Self-test results
        /// </summary>
        [DataMember]
        public int? SelfInspectCheck { get; set; }
        /// <summary>
        /// A unique number key alarm
        /// </summary>
        [DataMember]
        public string AlarmId { get; set; }
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
        /// Video logo
        /// </summary>
        [DataMember]
        public string VideoFileId { get; set; }
        /// <summary>
        /// Video file size
        /// </summary>
        [DataMember]
        public int? VideoFileSize { get; set; }
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

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Id)))
            {
                builder.AppendLine("Id:" + Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SelfInspectId)))
            {
                builder.AppendLine("SelfInspectId:" + SelfInspectId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SelfInspectCheck)))
            {
                builder.AppendLine("SelfInspectCheck:" + SelfInspectCheck.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmId)))
            {
                builder.AppendLine("AlarmId:" + AlarmId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmCheck)))
            {
                builder.AppendLine("AlarmCheck:" + AlarmCheck.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GpsCheck)))
            {
                builder.AppendLine("GpsCheck:" + GpsCheck.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VideoFileId)))
            {
                builder.AppendLine("VideoFileId:" + VideoFileId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VideoFileSize)))
            {
                builder.AppendLine("VideoFileSize:" + VideoFileSize.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VideoCheck)))
            {
                builder.AppendLine("VideoCheck:" + VideoCheck.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VideoQualityCheck)))
            {
                builder.AppendLine("VideoQualityCheck:" + VideoQualityCheck.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(IsSuccess)))
            {
                builder.AppendLine("IsSuccess:" + IsSuccess.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Approver)))
            {
                builder.AppendLine("Approver:" + Approver.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ApproverTime)))
            {
                builder.AppendLine("ApproverTime:" + ApproverTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Content)))
            {
                builder.AppendLine("Content:" + Content.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleID)))
            {
                builder.AppendLine("VehicleID:" + VehicleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteID)))
            {
                builder.AppendLine("SuiteID:" + SuiteID.ToString());
            }
            return builder.ToString();
        }

    }
}
