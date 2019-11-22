/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a8796c23-ff6b-41bb-8b28-86d6d0f75971      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract
/////    Project Description:    
/////             Class Name: ICarAlertDealLog
/////          Class Version: v1.0.0.0
/////            Create Time: 7/31/2013 3:07:10 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/31/2013 3:07:10 PM
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
    /// <summary>
    /// GetCarAlertDealLog
    /// </summary>
    [ServiceContract]
    public interface ICarAlertDealLogService
    {
        [OperationContract]
        MultiMessage<CarAlertLogInfo> GetCarAlertDealLog(string clientID, string vehicleId, string userName, DateTime startTime, DateTime endTime, PagingInfo pageInfo);
    }
}
