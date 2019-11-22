/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a499054e-e60e-47f4-bdb9-bbd7fb2fe202      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Contract.VideoSrv
/////    Project Description:    
/////             Class Name: QueryServerFileListArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 10:50:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 10:50:41
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
    public class QueryServerFileListArgs
    {
        /// <summary>
        /// 设备芯片号
        /// </summary>
        [DataMember]
        public string MdvrCoreSn { get; set; }

        /// <summary>
        /// 1:第一路视频，2:第二路视频,99:所有通道；
        /// </summary>
        [DataMember]
        public int Channel { get; set; }

        /// <summary>
        /// 0.普通文件；1.报警文件；2.所有文件
        /// </summary>
        [DataMember]        
        public int  Video_Type { get; set; }

        /// <summary>
        /// 查询起始时间，范围包括此时间。如：2013-01-01 01:01:01;
        /// </summary>
        [DataMember]
        public DateTime Start_Time { get; set; }

        /// <summary>
        /// 下载的截止时间，范围包括此时间。如：2013-01-01 01:01:01;
        /// </summary>        
        [DataMember]
        public DateTime  End_Time { get; set; }


        /// <summary>
        /// 分页大小
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        [DataMember]
        public int PageNum { get; set; }


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreSn)))
            {
                builder.AppendLine("Mdvr_Id:" + MdvrCoreSn.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Channel)))
            {
                builder.AppendLine("Channel:" + Channel.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Video_Type)))
            {
                builder.AppendLine("Video_Type:" + Video_Type.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Start_Time)))
            {
                builder.AppendLine("Start_Time:" + Start_Time.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(End_Time)))
            {
                builder.AppendLine("End_Time:" + End_Time.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(PageSize)))
            {
                builder.AppendLine("PageSize:" + PageSize.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(PageNum)))
            {
                builder.AppendLine("PageNum:" + PageNum.ToString());
            }
            return builder.ToString();
        }

    }
}