/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 3a3800fb-345f-44de-8375-a1da55283f23      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract
/////    Project Description:    
/////             Class Name: IVideoLog
/////          Class Version: v1.0.0.0
/////            Create Time: 7/31/2013 10:58:26 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/31/2013 10:58:26 AM
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
    public interface IVideoLogSerVice
    {
       // /// <summary>
       // /// Add Video Playrecord Log
       // /// </summary>
       // /// <param name="videoLog"></param>
       // /// <returns></returns>
       // [OperationContract]
       //SingleMessage<VideoLogInfo> AddVideoLog(VideoLogInfo videoLog);
        /// <summary>
        /// Get Video Playrecord Log
        /// </summary>
        /// <param name="player"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<VideoLogInfo> GetVideoLog(string player,string type, DateTime startTime, DateTime endTime, PagingInfo pageInfo);
    }
}
