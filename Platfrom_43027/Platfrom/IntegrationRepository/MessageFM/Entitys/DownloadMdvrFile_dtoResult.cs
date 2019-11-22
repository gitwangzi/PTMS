/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: adcb2cda-e668-408f-b0c0-24afeea77d9b      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Contract.VideoSrv
/////    Project Description:    
/////             Class Name: DownloadMdvrFileMessage
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 11:46:56
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 11:46:56
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Integration.Repository
{

    public class DownloadMdvrFile_dtoResult : VideoMessage
    {
        public DownloadMdvrFile_dtoResult()
            : base()
        {

        }

        /// <summary>
        /// -1：执行失败 0：执行成功 1：请求任务已下载完成
        /// </summary>
        
        public string result { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string url{get;set;}
        /// <summary>
        /// 下载文件总时长
        /// </summary>
        public string duration_time { get; set; }

    }
}
