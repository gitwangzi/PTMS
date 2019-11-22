/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 5c663903-3536-4f41-8a82-812bce3b7b39      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: LIN-20130409ZRS
/////                 Author: TEST(zhujf)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Contract.Data
/////    Project Description:    
/////             Class Name: SuiteSoftwareVersion
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/11 11:32:15
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/11 11:32:15
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Maintain.Contract.Data
{
    /// <summary>
    /// 安全套件软件版本映射
    /// </summary>
    [DataContract]
    class SuiteSoftwareVersion
    {
        /// <summary>
        /// 安全套件软件版本映射编号
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// 统一版本号
        /// </summary>
        [DataMember]
        public string Unified_Version { get; set; }

        /// <summary>
        /// 内部版本号
        /// </summary>
        [DataMember]
        public string Vendor_Version { get; set; }

        /// <summary>
        /// 硬件厂家
        /// </summary>
        [DataMember]
        public string Vendor { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [DataMember]
        public string Creater { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        [DataMember]
        public bool Valid { get; set; }
    }
}
