/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6df4a1e2-0fe7-4c71-9ecf-dda0dc0a3eb7      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.UserMessage
/////    Project Description:    
/////             Class Name: HandingAlarm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/29 17:23:54
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/29 17:23:54
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract.Data
{
    /// <summary>
    /// GPS Location Data V30
    /// </summary>
    [Serializable]
    [DataContract]
    public class HandingAlarm
    {
        /// <summary>
        /// Mdvr Core ID
        /// </summary>
        [DataMember]
        public string MdvrCoreId { get; set; }

        /// <summary>
        /// Alarm Time
        /// </summary>
        [DataMember]
        public DateTime AlarmTime { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + MdvrCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmTime)))
            {
                builder.AppendLine("AlarmTime:" + AlarmTime.ToString());
            }
            return builder.ToString();
        }

    }
}
