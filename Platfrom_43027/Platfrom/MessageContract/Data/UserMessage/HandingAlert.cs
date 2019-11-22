/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a5ce6b90-1774-4537-a61e-a89fbe0ea46e   
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-XUHJ
/////                 Author: TEST(xuhj)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract
/////    Project Description:    
/////             Class Name: CompleteAlarm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/30 15:05:54
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/30 17:05:54
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
    /// Handing Alert
    /// </summary>
    [Serializable]
    [DataContract]
    public class HandingAlert
    {
        /// <summary>
        /// MDVR CoreId
        /// </summary>
        [DataMember]
        public string MdvrCoreId { get; set; }

        /// <summary>
        /// Alert Time
        /// </summary>
        [DataMember]
        public DateTime AlertTime { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + MdvrCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlertTime)))
            {
                builder.AppendLine("AlertTime:" + AlertTime.ToString());
            }
            return builder.ToString();
        }

    }
}
