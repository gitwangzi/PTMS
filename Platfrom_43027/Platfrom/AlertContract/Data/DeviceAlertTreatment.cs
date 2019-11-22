/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: cec477c6-2a50-464e-a23b-fc4def3c405a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(BilongLiu)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Contract.Data
/////    Project Description:    
/////             Class Name: AlarmTreatment
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/26 11:09:31
/////      Class Description:  
/////======================================================================
/////          Modified Time: 
/////            Modified by: 
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Gsafety.PTMS.Alert.Contract.Data
{
    [DataContract]
    public class DeviceAlertTreatment
    {
        /// <summary>
        /// 处理Id（主键）
        /// </summary>
        [DataMember]
        public string Id { get; set; }
        /// <summary>
        /// 告警Id
        /// </summary>
        [DataMember]
        public string AlertId { get; set; }
        /// <summary>
        /// 处理人
        /// </summary>
        [DataMember]
        public string DisposeStaff { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        [DataMember]
        public Nullable<DateTime> DisposeTime { get; set; }
        /// <summary>
        /// 处理描述
        /// </summary>
        [DataMember]
        public string Content { get; set; }
    }
}
