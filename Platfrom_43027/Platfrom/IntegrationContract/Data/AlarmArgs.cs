/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7a598144-3468-4592-b1f1-926a4ff73685      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Contract.Data
/////    Project Description:    
/////             Class Name: AlarmArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-27 13:01:34
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-27 13:01:34
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Integration.Contract.Data
{
    [DataContract]
   public  class AlarmArgs
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        [DataMember]
        public string VehicleId { get; set; }

        /// <summary>
        /// Ant 报警ID
        /// </summary>
        [DataMember]
        public string AlarmId { get; set; }

        /// <summary>
        /// 真假警情；0:真实警情，1:不真实</param>
        /// </summary>
        [DataMember]
        public int AlarmType { get; set; }

        /// <summary>
        /// 处置结束时间
        /// </summary>
        [DataMember]       
        public DateTime DispatchEndTime { get; set; }

        /// <summary>
        ///  处警员（处警负责人）
        /// </summary>
        [DataMember]
        public string Dispatcher { get; set; }

        /// <summary>
        /// 处警过程或结果说明
        /// </summary>
        [DataMember]
        public string Content { get; set; }
    }
}
