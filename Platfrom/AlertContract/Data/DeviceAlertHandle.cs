/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 28083d1e-be1e-4cbe-bcc5-a0ff40b6f215      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LINGL
/////                 Author: TEST(zhangzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alert.Contract.Data
/////    Project Description:    
/////             Class Name: DeviceAlertHandle
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/30 17:30:24
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/30 17:30:24
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Alert.Contract.Data
{
    [DataContract]
    public class DeviceAlertHandle
    {
        /// <summary>
        /// alert id（primary key ）
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string HandleUser { get; set; }
        
        [DataMember]
        public DateTime? HandleTime { get; set; }
        
        [DataMember]
        public string StationId { get; set; }
        
        [DataMember]
        public string HandleContent { get; set; }
        
        [DataMember]
        public string VehicleId { get; set; }
        
        [DataMember]
        public DateTime? StartTime { get; set; }
        
        [DataMember]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [DataMember]
        public string Contact { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [DataMember]
        public string ContactPhone { get; set; }
        ///// <summary>
        ///// 告警ID
        ///// </summary>
        //[DataMember]
        //public string AlertId { get; set; }
        /// <summary>
        /// 处置状态
        /// </summary>
        [DataMember]
        public int Status { get; set; }
        /// <summary>
        /// 维修记录ID
        /// </summary>
        [DataMember]
        public string MaintenanceId { get; set; }

        /// <summary>
        /// MDVR芯片号
        /// </summary>
        [DataMember]
        public string MdvrCoreId { get; set; }
    }
}
