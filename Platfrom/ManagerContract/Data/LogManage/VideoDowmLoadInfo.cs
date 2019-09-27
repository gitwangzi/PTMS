/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e488e1dd-e587-4264-8ce3-27f1a8e88594      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract.Data
/////    Project Description:    
/////             Class Name: VideoDowmLoadInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 7/31/2013 11:43:58 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/31/2013 11:43:58 AM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Gsafety.PTMS.Manager.Contract.Data
{
    [DataContract]
    public class VideoDowmloadInfo
    {
        [DataMember]
        public string DownLoader { get; set; }

        [DataMember]
        public DateTime? RequestTime { get; set; }

        /// <summary>
        /// Ivoke_Type
        /// </summary>
        [DataMember]
        public string Ivoke_Type { get; set; }

        /// <summary>
        /// SubType
        /// </summary>
        [DataMember]
        public string SubType { get; set; }


        [DataMember]
        public string MDVRId { get; set; }

        [DataMember]
        public string FileName { get; set; }

        /// <summary>
        /// ChannelId
        /// </summary>
        [DataMember]
        public string ChannelId { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(DownLoader)))
            {
                builder.AppendLine("DownLoader:" + DownLoader.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RequestTime)))
            {
                builder.AppendLine("RequestTime:" + RequestTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Ivoke_Type)))
            {
                builder.AppendLine("Ivoke_Type:" + Ivoke_Type.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SubType)))
            {
                builder.AppendLine("SubType:" + SubType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MDVRId)))
            {
                builder.AppendLine("MDVRId:" + MDVRId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FileName)))
            {
                builder.AppendLine("FileName:" + FileName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ChannelId)))
            {
                builder.AppendLine("ChannelId:" + ChannelId.ToString());
            }
            return builder.ToString();
        }
    }
}
