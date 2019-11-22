/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0fd920ac-02ea-479c-8716-cb7ebf2d68fb      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract
/////    Project Description:    
/////             Class Name: IInstallLog
/////          Class Version: v1.0.0.0
/////            Create Time: 7/31/2013 3:09:33 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/31/2013 3:09:33 PM
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
    public interface IInstallLogService
    {
        /// <summary>
        /// GetInstallLog
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<InstallLogInfo> GetInstallLog(string clientID, string installStation, string installStaff, DateTime startTime, DateTime endTime, PagingInfo pageInfo);
    }
}
