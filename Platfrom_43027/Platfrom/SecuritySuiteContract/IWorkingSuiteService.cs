/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 41ef3923-a7da-4deb-bc4e-90098dd78402      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(BilongLiu)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Contract
/////    Project Description:    
/////             Class Name: IVehicleAlarmService
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/26 11:03:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/26 11:03:41
/////            Modified by: 
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.SecuritySuite.Contract.Data;

namespace Gsafety.PTMS.SecuritySuite.Contract
{
    /// <summary>
    /// SecuritySuite maintenance record
    /// </summary>
    [ServiceContract]
    public interface IWorkingSuiteService
    {
        /// <summary>
        /// Add workingSuite
        /// </summary>
        /// <param name="workingSuite">workingSuite</param>
        /// <returns>result</returns>
        [OperationContract]
        SingleMessage<int> AddWorkingSuite(WorkingSuite workingSuite);
        /// <summary>
        /// refresh workingSuite
        /// </summary>
        /// <param name="workingSuite">workingSuite</param>
        /// <returns>result</returns>
        [OperationContract]
        SingleMessage<Boolean> UpdateWorkingSuite(WorkingSuite workingSuite);
        /// <summary>
        /// delete workingSuite
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>result</returns>
        [OperationContract]
        SingleMessage<Boolean> DeleteWorkingSuite(string id);
    }
}
