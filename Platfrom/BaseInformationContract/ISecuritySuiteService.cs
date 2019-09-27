/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: adc5f02c-4cca-4ebb-9a25-7e39cea9f512      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LANQ
/////                 Author: TEST(lanq)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Contract
/////    Project Description:    
/////             Class Name: ISecuritySuite
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 9:57:01
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/22 9:57:01
/////            Modified by: BilongLiu
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Base.Contract.Data;
using System.IO;
using Gsafety.PTMS.Common.Enum;

namespace Gsafety.PTMS.BaseInformation.Contract
{
    [ServiceContract]
    public interface ISecuritySuiteService
    {
        #region
        ///// <summary>
        ///// Get Security Suite By ID
        ///// </summary>
        ///// <param name="id">id</param>
        ///// <returns>DeviceSuite</returns>
        //[OperationContract]
        //SingleMessage<DeviceSuite> GetSecuritySuiteByID(string id);

        ///// <summary>
        ///// Get Vechile Numbe rBy SuiteId
        ///// </summary>
        ///// <param name="id">suiteId</param>
        ///// <returns>Vehicle</returns>
        //[OperationContract]
        //SingleMessage<Vehicle> GetVechileNumberBySuiteId(string suiteId);


        #endregion

        /// <summary>
        /// Add Security Suite
        /// </summary>
        /// <param name="securitySuite">securitySuite</param>
        /// <returns>Result</returns>
        [OperationContract]
        SingleMessage<Boolean> AddSecuritySuite(DeviceSuite securitySuite);

        /// <summary>
        /// Update Security Suite
        /// </summary>
        /// <param name="securitySuite">securitySuite</param>
        /// <returns>Result</returns>
        [OperationContract]
        SingleMessage<Boolean> UpdateSecuritySuite(DeviceSuite securitySuite);

        /// <summary>
        /// Get Security Suite By SuiteId
        /// </summary>
        /// <param name="suiteId">SuiteId</param>
        /// <returns>DeviceSuite</returns>
        [OperationContract]
        SingleMessage<DeviceSuite> GetSecuritySuiteBySuiteId(string suiteId, string mdvrid);

        /// <summary>
        /// Get Security Suites Fuzzy
        /// </summary>
        [OperationContract]
        MultiMessage<DeviceSuite> GetSecuritySuitesFuzzy(string vehicleId, string suiteId, string mdvrId, string mdvrCoreId, InstallStatusType? status, PagingInfo page);

        /// <summary>
        /// Updat Suite Info
        /// </summary>
        [OperationContract]
        SingleMessage<Boolean> UpdateSecuritySuiteStatusByID(string Id, DeviceSuiteStatus status);

        /// <summary>
        /// Check Suite Exist
        /// </summary>
        /// <param name="suiteID"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> CheckSecuritySuiteExistBySuiteID(string suiteID);

        /// <summary>
        /// Check SecuritySuite By MdvrID
        /// </summary>
        /// <param name="mdvrCoreId"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> CheckSecuritySuiteExistByMdvrCoreId(string mdvrCoreId);

        /// <summary>
        /// Check Security Suite Exist By MdvrId
        /// </summary>
        /// <param name="mdvrId"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> CheckSecuritySuiteExistByMdvrId(string mdvrId);

        ///// <summary>
        ///// Get Repair Suite By Suiteid
        ///// </summary>
        ///// <param name="id">suiteid</param>
        ///// <returns>DeviceSuiteRepair</returns>
        //[OperationContract]
        //SingleMessage<DeviceSuiteRepair> GetRepairSuiteBySuiteid(string suiteid);

        /// <summary>
        /// Check Install Detail By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> CheckInstallDetailById(string Id);

        /// <summary>
        /// Batch Add Device Suite
        /// </summary>
        /// <param name="suiteList"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> BatchAdd(List<DeviceSuite> suiteList);

        [OperationContract]
        SingleMessage<bool> DeleteSecuritySuite(DeviceSuite SecuritySuite);

        /// <summary>
        /// Check DeviceSuite exist 
        /// </summary>
        /// <param name="deviceSuiteList"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<bool> CheckSecuritySuiteExist(List<DeviceSuite> deviceSuiteList);
    }
}
