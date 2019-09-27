/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 634b5c30-95c8-47d3-af4b-38777d1b5214      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.VideoLog.Contract
/////    Project Description:    
/////             Class Name: VerifyUserArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2014-01-17 09:46:36
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014-01-17 09:46:36
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.VideoLog.Contract
{
    [DataContract]
    public class VerifyUserArgs
    {
        [DataMember]
        public string user_name { get; set; }
        [DataMember]
        public string user_pwd { get; set; }
        [DataMember]
        public string md5_str { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(user_name)))
            {
                builder.AppendLine("user_name:" + user_name.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(user_pwd)))
            {
                builder.AppendLine("user_pwd:" + user_pwd.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(md5_str)))
            {
                builder.AppendLine("md5_str:" + md5_str.ToString());
            }
            return builder.ToString();
        }

    }
}
