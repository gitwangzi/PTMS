/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: fb555cae-3d91-490b-bf65-1ca95b19f257      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract
/////    Project Description:    
/////             Class Name: ILoginLog
/////          Class Version: v1.0.0.0
/////            Create Time: 7/31/2013 10:02:05 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/27/2013 10:02:05 AM
/////            Modified by: BilongLiu
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
using Gsafety.PTMS.Common.Data;

namespace Gsafety.PTMS.Manager.Contract
{
    /// <summary>
    /// LoginLogService
    /// </summary>
    [ServiceContract]
    public interface ILoginLogService
    {
        /// <summary>
        /// AddLoginLog
        /// </summary>
        /// <param name="userName">userName</param>
        /// <returns>value</returns>
        [OperationContract]
        SingleMessage<bool> AddLoginLog(LogAccess access);

        /// <summary>
        /// GetLoginLog
        /// </summary>
        /// <param name="userName">userName</param>
        /// <param name="startTime">startTime</param>
        /// <param name="endTime">endTime</param>
        /// <param name="pageInfo">pageInfo</param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<LogAccess> GetLoginLog(string clientID, string userName, DateTime? startTime, DateTime? endTime, PagingInfo pageInfo);

        /// <summary>
        /// ClearLoginLog
        /// </summary>
        /// <param name="startTime">startTime</param>
        /// <param name="endTime">endTime</param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> ClearLoginLog(string clientID, string userID, string username, string content);
    }
}
