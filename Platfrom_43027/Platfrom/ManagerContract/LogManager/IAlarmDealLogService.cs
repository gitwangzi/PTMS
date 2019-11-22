/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: d567de96-2c20-48ba-908b-324200522bf6      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract
/////    Project Description:    
/////             Class Name: IAlarmDealLog
/////          Class Version: v1.0.0.0
/////            Create Time: 7/31/2013 3:06:52 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/31/2013 3:06:52 PM
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
    /// AlarmDealLog
    /// </summary>
    [ServiceContract]
    public interface IAlarmDealLogService
    {
        [OperationContract]
        MultiMessage<AlarmDealLogInfo> GetAlarmDealLog(string clientID, string vehicleID, string userName, DateTime startTime, DateTime endTime, PagingInfo pageInfo);
    }
}
