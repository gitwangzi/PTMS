/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2d949da5-8484-48b9-aed2-6de8a10ed9d2      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.UserMessage
/////    Project Description:    
/////             Class Name: ChangeUser
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/23 10:48:03
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/23 10:48:03
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
    /// Change User
    /// </summary>
    [Serializable]
    [DataContract]
    public class ChangeUser
    {
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public int Type { get; set; }

        [DataMember]
        public DateTime ChangeTime { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(UserName)))
            {
                builder.AppendLine("UserName:" + UserName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Type)))
            {
                builder.AppendLine("Type:" + Type.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ChangeTime)))
            {
                builder.AppendLine("ChangeTime:" + ChangeTime.ToString());
            }
            return builder.ToString();
        }

    }
}
