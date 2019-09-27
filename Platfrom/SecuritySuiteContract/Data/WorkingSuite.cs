/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0541e157-89d1-49ad-a387-36bf9cb6ad94      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(BilongLiu)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Monitor.Contract
/////    Project Description:    
/////             Class Name: AlertInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/26 15:34:43
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/26 15:34:43
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.Alert.Contract.Data;

namespace Gsafety.PTMS.SecuritySuite.Contract
{
    /// <summary>
    /// 工作套件
    /// </summary>
    [DataContract]
    public class WorkingSuite
    {
        /// <summary>
        /// 安全套件表（主键）
        /// </summary>
        [DataMember]
        public string Id { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        [DataMember]
        public string VehicleId{ get; set; }
        /// <summary>
        /// MDVR芯片号
        /// </summary>
        [DataMember]
        public string MdvrCoreId { get; set; }
        /// <summary>
        /// 安全套件状态
        /// </summary>
        [DataMember]
        public int? Status { get; set; }
        /// <summary>
        /// 上下线切换时间
        /// </summary>
        [DataMember]
        public DateTime? SwitchTime { get; set; }
        /// <summary>
        /// 是否在线
        /// </summary>
        [DataMember]
        public int? OnlineFlag { get; set; }
        /// <summary>
        /// 设备异常原因
        /// </summary>
        [DataMember]
        public string AbnormalCause { get; set; }
        /// <summary>
        /// 安全套件号
        /// </summary>
        [DataMember]
        public string SuiteId { get; set; }
        /// <summary>
        /// 异常开始时间
        /// </summary>
        [DataMember]
        public DateTime? FaultTime { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Id)))
            {
                builder.AppendLine("Id:" + Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleId)))
            {
                builder.AppendLine("VehicleId:" + VehicleId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + MdvrCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SwitchTime)))
            {
                builder.AppendLine("SwitchTime:" + SwitchTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OnlineFlag)))
            {
                builder.AppendLine("OnlineFlag:" + OnlineFlag.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AbnormalCause)))
            {
                builder.AppendLine("AbnormalCause:" + AbnormalCause.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteId)))
            {
                builder.AppendLine("SuiteId:" + SuiteId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FaultTime)))
            {
                builder.AppendLine("FaultTime:" + FaultTime.ToString());
            }
            return builder.ToString();
        }

    }
}
