/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 31053915-3f08-43ec-af03-a58c08b170f2      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: VideoLogContract
/////    Project Description:    
/////             Class Name: AddVideoLogArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-10-14 13:31:27
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-10-14 13:31:27
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
    public class AddVideoLogArgs
    {
        /// <summary>
        /// User Name
        /// </summary>
        [DataMember]
        public string user_name { get; set; }
        /// <summary>
        /// Invoke type
        /// </summary>
        [DataMember]
        public string invoke_type { get; set; }

        /// <summary>
        /// Message ID
        /// </summary>
        [DataMember]
        public string msg_id { get; set; }

        /// <summary>
        /// PassWord
        /// </summary>
        [DataMember]
        public string user_pwd { get; set; }

        /// <summary>
        /// UsreType
        /// </summary>
        [DataMember]
        public string user_type { get; set; }

        /// <summary>
        /// user dep
        /// </summary>
        [DataMember]
        public string user_dep { get; set; }


        /// <summary>
        /// action time
        /// </summary>
        [DataMember]
        public string action_time { get; set; }

        /// <summary>
        /// sub type
        /// </summary>
        [DataMember]
        public string sub_type { get; set; }

        /// <summary>
        /// mdvr_id
        /// </summary>
        [DataMember]
        public string mdvr_id { get; set; }

        /// <summary>
        /// channel
        /// </summary>
        [DataMember]
        public string channel { get; set; }
        /// <summary>
        /// start_time
        /// </summary>
        [DataMember]
        public string start_time { get; set; }
        /// <summary>
        /// end_time
        /// </summary>
        [DataMember]
        public string end_time { get; set; }
        /// <summary>
        /// file
        /// </summary>
        [DataMember]
        public string file { get; set; }

        /// <summary>
        /// extend1
        /// </summary>
        [DataMember]
        public string extend1 { get; set; }
        /// <summary>
        ///  extend2
        /// </summary>
        [DataMember]
        public string extend2 { get; set; }
        /// <summary>
        ///  extend3
        /// </summary>
        [DataMember]
        public string extend3 { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(user_name)))
            {
                builder.AppendLine("user_name:" + user_name.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(invoke_type)))
            {
                builder.AppendLine("invoke_type:" + invoke_type.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(msg_id)))
            {
                builder.AppendLine("msg_id:" + msg_id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(user_pwd)))
            {
                builder.AppendLine("user_pwd:" + user_pwd.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(user_type)))
            {
                builder.AppendLine("user_type:" + user_type.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(user_dep)))
            {
                builder.AppendLine("user_dep:" + user_dep.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(action_time)))
            {
                builder.AppendLine("action_time:" + action_time.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(sub_type)))
            {
                builder.AppendLine("sub_type:" + sub_type.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(mdvr_id)))
            {
                builder.AppendLine("mdvr_id:" + mdvr_id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(channel)))
            {
                builder.AppendLine("channel:" + channel.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(start_time)))
            {
                builder.AppendLine("start_time:" + start_time.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(end_time)))
            {
                builder.AppendLine("end_time:" + end_time.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(file)))
            {
                builder.AppendLine("file:" + file.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(extend1)))
            {
                builder.AppendLine("extend1:" + extend1.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(extend2)))
            {
                builder.AppendLine("extend2:" + extend2.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(extend3)))
            {
                builder.AppendLine("extend3:" + extend3.ToString());
            }
            return builder.ToString();
        }

    }
}
