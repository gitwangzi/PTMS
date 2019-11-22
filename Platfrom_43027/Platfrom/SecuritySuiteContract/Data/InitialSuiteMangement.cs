using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Linq.Expressions;
using Gsafety.PTMS.Base.Contract.Data;

namespace Gsafety.PTMS.SecuritySuite.Contract.Data
{
    [DataContract]
    public class InitialSuiteMangement
    {
        /// <summary>
        /// 安全套件编号
        /// </summary>
        [DataMember]
        public string SuiteID { get; set; }
        /// <summary>
        /// 安全套件INFOID
        /// </summary>
        [DataMember]
        public string SuiteINFOID { get; set; }
        /// <summary>
        /// 安全套件状态 
        /// </summary>
        [DataMember]
        public int? CurrentStatus { get; set; }
        /// <summary>
        /// 临时安全套件状态 
        /// </summary>
        [DataMember]
        public int? TempCurrentStatus { get; set; }
        /// <summary>
        /// 安全套件芯片号
        /// </summary>
        [DataMember]
        public string MdvrCoreId { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        [DataMember]
        public string VehicleID { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        [DataMember]
        public string UserInfo { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteID)))
            {
                builder.AppendLine("SuiteID:" + SuiteID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteINFOID)))
            {
                builder.AppendLine("SuiteINFOID:" + SuiteINFOID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CurrentStatus)))
            {
                builder.AppendLine("CurrentStatus:" + CurrentStatus.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(TempCurrentStatus)))
            {
                builder.AppendLine("TempCurrentStatus:" + TempCurrentStatus.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + MdvrCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleID)))
            {
                builder.AppendLine("VehicleID:" + VehicleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UserInfo)))
            {
                builder.AppendLine("UserInfo:" + UserInfo.ToString());
            }
            return builder.ToString();
        }

    }
}
