/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 045458ad-fc80-4525-a6e7-0410e2c35246      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.UserMessage
/////    Project Description:    
/////             Class Name: DeleteUser
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/23 10:47:49
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/23 10:47:49
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
    /// Delete User
    /// </summary>
    [Serializable]
    [DataContract]
    public class DeleteUser
    {
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public DateTime DeleteTime { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(UserName)))
            {
                builder.AppendLine("UserName:" + UserName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DeleteTime)))
            {
                builder.AppendLine("DeleteTime:" + DeleteTime.ToString());
            }
            return builder.ToString();
        }

    }
}
