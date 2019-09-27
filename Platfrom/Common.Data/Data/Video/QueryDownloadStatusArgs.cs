/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 3fdf0219-c267-41a5-a48f-e6eef6f0a6f7      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Contract.VideoSrv
/////    Project Description:    
/////             Class Name: QueryDownloadStatusArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 11:53:03
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 11:53:03
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    public class QueryDownloadStatusArgs
    {
        /// <summary>
        /// 设备芯片号
        /// </summary>
        [DataMember]
        public string Mdvr_Id { get; set; }

        /// <summary>
        ///  文件唯一编号
        /// </summary>
        [DataMember]
        public string Mdvr_File_Id { get; set; }

        /// <summary>
        /// 下载文件的开始偏移时间(秒)
        /// </summary>
        [DataMember]
        public int Offset_Start_Time { get; set; }

        /// <summary>
        /// 下载文件的结束偏移时间(秒)
        /// </summary>
        [DataMember]
        public int Offset_End_Time { get; set; }

        /// <summary>
        /// 文件IDs
        /// </summary>
        [DataMember]
        public List<string> FileIDs { get; set; }


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Mdvr_Id)))
            {
                builder.AppendLine("Mdvr_Id:" + Mdvr_Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Mdvr_File_Id)))
            {
                builder.AppendLine("Mdvr_File_Id:" + Mdvr_File_Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Offset_Start_Time)))
            {
                builder.AppendLine("Offset_Start_Time:" + Offset_Start_Time.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Offset_End_Time)))
            {
                builder.AppendLine("Offset_End_Time:" + Offset_End_Time.ToString());
            }
            return builder.ToString();
        }

    }
}