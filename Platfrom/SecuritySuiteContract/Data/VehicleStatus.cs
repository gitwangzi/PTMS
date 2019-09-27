/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 76153fd3-4fd5-483c-b5f2-bc8c41da5294      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(jiaoyx)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SecuritySuite.Contract
/////    Project Description:    
/////             Class Name: VehicleStatus
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/1 15:31:33
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/22 15:31:33
/////            Modified by: BilongLiu
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.SecuritySuite.Contract.Data
{
    /// <summary>
    /// 车辆状态信息
    /// </summary>
    [DataContract]
    public class VehicleStatus
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        [DataMember]
        public string CarNumber { get; set; }
        /// <summary>
        /// 车辆类型
        /// </summary>
        [DataMember]
        public int? CarType { get; set; }
        /// <summary>
        /// 配套的安全套件编号
        /// </summary>
        [DataMember]
        public string SutieInfoId { get; set; }
        /// <summary>
        /// MDVR 芯片号
        /// </summary>
        [DataMember]
        public string MdvrCoreId { get; set; }
        /// <summary>
        /// 套件状态
        /// </summary>
        [DataMember]
        public int? Status { get; set; }
        /// <summary>
        /// 是否在线
        /// </summary>
        [DataMember]
        public int? IsOnline { get; set; }
        /// <summary>
        /// 异常原因
        /// </summary>
        [DataMember]
        public string AbnormalCause { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(CarNumber)))
            {
                builder.AppendLine("CarNumber:" + CarNumber.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CarType)))
            {
                builder.AppendLine("CarType:" + CarType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SutieInfoId)))
            {
                builder.AppendLine("SutieInfoId:" + SutieInfoId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + MdvrCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(IsOnline)))
            {
                builder.AppendLine("IsOnline:" + IsOnline.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AbnormalCause)))
            {
                builder.AppendLine("AbnormalCause:" + AbnormalCause.ToString());
            }
            return builder.ToString();
        }

    }
}
