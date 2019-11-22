/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 84d14203-4176-49b5-895c-79b3a5ef1050      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.UserMessage
/////    Project Description:    
/////             Class Name: UpgradeNotify
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/7 11:04:12
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/7 11:04:12
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
    /// Upgrade Notify
    /// </summary>
    [Serializable]
    [DataContract]
    public class UpgradeNotify
    {
        /// <summary>
        /// Notify Time
        /// </summary>
        [DataMember]
        public DateTime NotifyTime { get; set; }
    }
}
