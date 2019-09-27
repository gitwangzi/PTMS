/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: b53a72a3-284e-419c-aafa-ce9a0d3514d7      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract
/////    Project Description:    
/////             Class Name: IVisitLogService
/////          Class Version: v1.0.0.0
/////            Create Time: 7/31/2013 10:33:32 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/27/2013 10:33:32 AM
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

namespace Gsafety.PTMS.Manager.Contract
{
    /// <summary>
    /// The Interface of System Visit Log
    /// </summary>
    [ServiceContract]
    public interface IVisitLogService
    {
        /// <summary>
        /// Add Visit Log
        /// </summary>
        /// <param name="visitLog">Visit Log</param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> AddVisitLog(VisitLogInfo visitLog);
        /// <summary>
        /// Get Visit Log
        /// </summary>
        /// <param name="visitor">Visitor</param>
        /// <param name="type">Visit Type</param>
        /// <param name="startTime">Start Time</param>
        /// <param name="endTime">End Time</param>
        /// <param name="pageInfo">Page Informaiton</param>
        /// <returns>Log List</returns>
        [OperationContract]
        MultiMessage<VisitLogInfo> GetVisitLog(string visitor, int type, DateTime startTime, DateTime endTime, PagingInfo pageInfo);
        /// <summary>
        /// Clear Visit Log
        /// </summary>
        /// <param name="startTime">Start Time</param>
        /// <param name="endTime">End Time</param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> ClearVisitLog(DateTime startTime, DateTime endTime);
    }
}
