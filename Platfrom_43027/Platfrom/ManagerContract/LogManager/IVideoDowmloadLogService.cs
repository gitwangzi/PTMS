/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0048572a-19ac-418f-869b-63f5bde6923f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract
/////    Project Description:    
/////             Class Name: IVideoDowmLoadLog
/////          Class Version: v1.0.0.0
/////            Create Time: 7/31/2013 11:37:50 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/31/2013 11:37:50 AM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Manager.Contract.Data;

namespace Gsafety.PTMS.Manager.Contract
{
    [ServiceContract]
    public interface IVideoDowmloadLogService
    {
       // /// <summary>
       // /// Add Videodownload Log
       // /// </summary>
       // /// <param name="videoDownloadLog"></param>
       // /// <returns></returns>
       // [OperationContract]
       //SingleMessage< VideoDowmloadInfo> AddVideoDownloadLog(VideoDowmloadInfo videoDownloadLog);
        /// <summary>
        /// Get Videodownload Log
        /// </summary>
        /// <param name="downloader"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<VideoLogInfo> GetVideoDownloadLog(string downloader, string type, DateTime startTime, DateTime endTime, PagingInfo pageInfo);
    }
}
