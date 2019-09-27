/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e8cf3871-f071-443f-a486-d1b5ec4101f5      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.UserMessage
/////    Project Description:    
/////             Class Name: StartInstall
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/23 14:39:05
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/23 14:39:05
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
    /// Start Install
    /// </summary>
    [Serializable]
    [DataContract]
    public class StartInstall
    {
        /// <summary>
        /// MDVR Core Id
        /// </summary>
        [DataMember]
        public string MdvrCoreId { get; set; }

        /// <summary>
        /// Start Install Time
        /// </summary>
        [DataMember]
        public DateTime StartInstallTime { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + MdvrCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(StartInstallTime)))
            {
                builder.AppendLine("StartInstallTime:" + StartInstallTime.ToString());
            }
            return builder.ToString();
        }

    }
}
