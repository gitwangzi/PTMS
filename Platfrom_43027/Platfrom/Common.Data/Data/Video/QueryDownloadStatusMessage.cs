/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 34efb39b-eeb3-4930-9778-b96eae4acf64      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Contract.VideoSrv
/////    Project Description:    
/////             Class Name: QueryDownloadStatusMessage
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 14:50:53
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 14:50:53
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
    public class QueryDownloadStatusMessage
    {
        /// <summary>
        /// 状态:(0 等待下载,1 指令发送成功,2 下载中,3 下载停止,4 下载完成,10 指令发送失败)
        /// </summary>
        [DataMember]
        public Decimal Status { get; set; }

        /// <summary>
        /// 进度
        /// </summary>
        [DataMember]
        public decimal Percent { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        [DataMember]
        public string Url { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        [DataMember]
        public int Result { get; set; }

        /// <summary>
        /// 下载文件总时长
        /// </summary>
        [DataMember]
        public string Duration_time { get; set; }

        /// <summary>
        /// 文件ID
        /// </summary>
        [DataMember]
        public string FileID { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Percent)))
            {
                builder.AppendLine("Percent:" + Percent.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Url)))
            {
                builder.AppendLine("Url:" + Url.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Result)))
            {
                builder.AppendLine("Result:" + Result.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Duration_time)))
            {
                builder.AppendLine("Duration_time:" + Duration_time.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FileID)))
            {
                builder.AppendLine("FileID:" + FileID.ToString());
            }

            return builder.ToString();
        }
    }
}
