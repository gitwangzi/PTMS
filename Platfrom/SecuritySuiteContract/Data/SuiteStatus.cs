/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 64f46f15-b9df-4d23-b999-53680d18beb7      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LANQ
/////                 Author: TEST(lanq)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SecuritySuite.Contract.Data
/////    Project Description:    
/////             Class Name: SuiteStatus
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/29 15:41:37
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/29 15:41:37
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Gsafety.PTMS.SecuritySuite.Contract.Data
{
    /// <summary>
    /// 安全套件状态信息
    /// </summary>
    [DataContract]
    public class SuiteStatus
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        [DataMember]
        public string VehicleID { get; set; }
        /// <summary>
        /// 车辆类型
        /// </summary>
        [DataMember]
        public int? VehicleType { get; set; }
        /// <summary>
        /// 车主
        /// </summary>
        [DataMember]
        public string Owner { get; set; }
        /// <summary>
        /// 车主联系电话
        /// </summary>
        [DataMember]
        public string OwnerPhone { get; set; }
        /// <summary>
        /// 安全套件编号
        /// </summary>
        [DataMember]
        public string SuiteID { get; set; }
        /// <summary>
        /// MDVR 芯片号
        /// </summary>
        [DataMember]
        public string MdvrCoreID { get; set; }
        /// <summary>
        /// 是否在线
        /// </summary>
        [DataMember]
        public int? OnlineStatus { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        [DataMember]
        public string Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember]
        public string Latitude { get; set; }
        /// <summary>
        /// 上下线时间
        /// </summary>
        [DataMember]
        public DateTime? StatusChangeTime { get; set; }
        /// <summary>
        /// 上下线持续时间
        /// </summary>
        [DataMember]
        public TimeSpan StatusTimeSpan { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleID)))
            {
                builder.AppendLine("VehicleID:" + VehicleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleType)))
            {
                builder.AppendLine("VehicleType:" + VehicleType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Owner)))
            {
                builder.AppendLine("Owner:" + Owner.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OwnerPhone)))
            {
                builder.AppendLine("OwnerPhone:" + OwnerPhone.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteID)))
            {
                builder.AppendLine("SuiteID:" + SuiteID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreID)))
            {
                builder.AppendLine("MdvrCoreID:" + MdvrCoreID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OnlineStatus)))
            {
                builder.AppendLine("OnlineStatus:" + OnlineStatus.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Longitude)))
            {
                builder.AppendLine("Longitude:" + Longitude.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Latitude)))
            {
                builder.AppendLine("Latitude:" + Latitude.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(StatusChangeTime)))
            {
                builder.AppendLine("StatusChangeTime:" + StatusChangeTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(StatusTimeSpan)))
            {
                builder.AppendLine("StatusTimeSpan:" + StatusTimeSpan.ToString());
            }
            return builder.ToString();
        }

    }
}
