/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: bcc22bcd-099e-4a62-977c-8e37be5f8648      
/////             clrversion: 4.0.30319.34011
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.UserMessage
/////    Project Description:    
/////             Class Name: RealAlarm
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/2/27 12:26:06
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/2/27 12:26:06
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
    /// Real Alert After Handing PTMS Platform , needing to Send to ARADS
    /// </summary>
    [Serializable]
    [DataContract]
    public class RealAlarm
    {
        /// <summary>
        /// Alarm ID
        /// </summary>
        [DataMember]
        public string AlarmID { get; set; }

        /// <summary>
        /// MDVR CoreId
        /// </summary>
        [DataMember]
        public string MdvrCoreId { get; set; }

        /// <summary>
        /// Alarm Time
        /// </summary>
        [DataMember]
        public DateTime AlarmTime { get; set; }

        /// <summary>
        /// Alert Complete Time
        /// </summary>
        [DataMember]
        public DateTime CompleteTime { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmID)))
            {
                builder.AppendLine("AlarmID:" + AlarmID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + MdvrCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmTime)))
            {
                builder.AppendLine("AlarmTime:" + AlarmTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CompleteTime)))
            {
                builder.AppendLine("CompleteTime:" + CompleteTime.ToString());
            }
            return builder.ToString();
        }

    }
}
