/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 30f1acd0-0513-4d79-b3ff-132830883018      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(jiaoyx)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Contract
/////    Project Description:    
/////             Class Name: IDeviceInstallServiceA
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/1 14:14:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/21 14:14:41
/////            Modified by: BilongLiu
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Installation.Contract.Data;
using Gsafety.PTMS.Common.Data;

namespace Gsafety.PTMS.Installation.Contract
{
    [ServiceContract]
    public interface IDeviceInstallService
    {
        /// <summary>
        /// Get the current security suite license plate number of the upload being installed
        /// </summary>
        /// <param name="mdvrId">Mdvr chip number</param>
        /// <returns>Security Suite on the license plate number</returns>
        [OperationContract]
        SingleMessage<String> GetInstallingSuiteVehicleId(string mdvrId);
        /// <summary>
        /// Installation Record Results
        /// </summary>
        /// <param name="installationId">Installation Record Id</param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<InstallInfoResult> GetInstallationResult(string installationId);
        /// <summary>
        /// For installation information based on the primary key ID
        /// </summary>
        /// <param name="installationId">Install the primary key Id</param>
        /// <returns>Installation Information</returns>
        [OperationContract]
        SingleMessage<InstallationInfo> GetInstallationDetail(string installationId);
        /// <summary>
        /// For installation information based on the primary key ID
        /// </summary>
        /// <param name="installationId">Install the primary key Id</param>
        /// <returns>Installation Information</returns>
        [OperationContract]
        SingleMessage<InstallationInfo> GetGPSInstallationDetail(string installationId);
        /// <summary>
        /// Add a new installation record
        /// </summary>
        /// <param name="installation">Installation Record</param>
        /// <returns>True indicates success, False indicates failure</returns>
        [OperationContract]
        SingleMessage<Boolean> AddInstallation(InstallationInfo installation);
        /// <summary>
        /// Update installation log
        /// </summary>
        /// <param name="installation">Installation Record</param>
        /// <returns>True indicates success, False indicates failure</returns>
        [OperationContract]
        SingleMessage<Boolean> UpdateInstallation(InstallationInfo installation);
        /// <summary>
        /// Delete the corresponding primary key installation data according to the installation Id
        /// </summary>
        /// <param name="installationId">Install the primary key Id</param>
        /// <returns>true indicates deleted successfully, false representation delete failed</returns>
        [OperationContract]
        SingleMessage<Boolean> DeleteInstallation(string installationId);
        /// <summary>
        /// Add a new installation audit records
        /// </summary>
        /// <param name="audit">Id primary key</param>
        /// <returns>Return value of True indicates success, False indicates failure</returns>
        [OperationContract]
        SingleMessage<Boolean> AddInstallationAudit(InstallationAudit audit);
        /// <summary>
        /// Delete the installation audit records based on the primary key Id
        /// </summary>
        /// <param name="auditId">Id primary key installation audit records</param>
        /// <returns>Return value of True indicates success, False indicates failure</returns>
        [OperationContract]
        SingleMessage<bool> DeleteInstallationAudit(string auditId);
        /// <summary>
        /// Record a mount point for installation unfinished
        /// </summary>
        /// <param name="installStationId">Installation point numbers, which are all empty</param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<InstallationInfo> GetInstallationInProgress(string installStationId);
        /// <summary>
        /// Get the installation log unfinished (fuzzy query)
        /// </summary>
        /// <param name="carNumber">License plate number</param>
        /// <param name="suiteId">Security Suite No.</param>
        /// <param name="installer">Installer</param>
        /// <param name="installStationId">Mount point id</param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<InstallationInfo> GetInstallationInProgressEx(string clientID, string carNumber, string suiteId, string installer, List<string> installStationIds, DateTime? beginDate, DateTime? endDate, PagingInfo pageInfo);

        /// <summary>
        /// Get the installation log unfinished (fuzzy query)
        /// </summary>
        /// <param name="carNumber">License plate number</param>
        /// <param name="suiteId">Security Suite No.</param>
        /// <param name="installer">Installer</param>
        /// <param name="installStationId">Mount point id</param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<InstallationInfo> GetGPSInstallationInProgressEx(string carNumber, string gpsid, string installer, List<string> installStationIds, DateTime? beginDate, DateTime? endDate, PagingInfo pageInfo);
        /// <summary>
        /// For installation records have been completed
        /// </summary>
        /// <param name="installStationId">Mounting points</param>
        /// <param name="beginDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<InstallationInfo> GetInstallationFinished(string installStationId,
            DateTime beginDate, DateTime endDate);
        /// <summary>
        /// For installation records have been completed, fuzzy query
        /// </summary>
        /// <param name="carNumber">License plate number</param>
        /// <param name="suiteId">Security Suite No.</param>
        /// <param name="installer">Installer</param>
        /// <param name="installStationId">Mount point</param>
        /// <param name="beginDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<InstallationInfo> GetInstallationFinishedEx(string carNumber,
            string suiteId, string installer, string installStationId,
            DateTime beginDate, DateTime endDate);

        /// <summary>
        /// For installation records have been completed, for GPS
        /// </summary>
        /// <param name="carNumber">License plate number</param>
        /// <param name="gpsid">Security Suite No.</param>
        /// <param name="installer">Installer</param>
        /// <param name="installStationId">Mount point</param>
        /// <param name="beginDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<InstallationInfo> GetGPSInstallationFinished(string carNumber, string gpsid, string installer, List<string> installStationId, DateTime? beginDate, DateTime? endDate, PagingInfo pageInfo);
        /// <summary>
        /// For installation records have been completed, fuzzy query, paging query
        /// </summary>
        /// <param name="carNumber">License plate number</param>
        /// <param name="suiteId">Security Suite No.</param>
        /// <param name="installer">Installer</param>
        /// <param name="installStationId">Mounting points</param>
        /// <param name="beginDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="pageSize">The number of entries per page</param>
        /// <param name="pageIndex">Pages</param>
        /// <param name="totalCount">The total number of entries</param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<InstallationInfo> GetInstallationFinishedEx1(string clientID, string carNumber, string suiteId, string installer, List<string> installStationId, DateTime? beginDate, DateTime? endDate, PagingInfo pageInfo);
        /// <summary>
        /// For installation audit, the fuzzy query, paging query
        /// </summary>
        /// <param name="carNumber">License plate number</param>
        /// <param name="suiteId">Security Suite No.</param>
        /// <param name="installer">Installer</param>
        /// <param name="installStationId">Mounting points</param>
        /// <param name="beginDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="pageSize">The number of entries per page</param>
        /// <param name="pageIndex">Pages</param>
        /// <param name="totalCount">The total number of entries</param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<InstallationAuditCollect> GetInstallationAudit(string carNumber,
            string suiteId, string installer, string installStationId,
            DateTime? beginDate, DateTime? endDate, PagingInfo pageInfo, bool onlyWaitCheck);
        /// <summary>
        /// Get unfinished installation records, fuzzy query, paging query
        /// </summary>
        /// <param name="carNumber">License plate number</param>
        /// <param name="suiteId">Security Suite No.</param>
        /// <param name="installStationId">Mounting points</param>
        /// <param name="beginDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="pageInfo">Paging Information</param>
        /// Security Suite for "Installation Verification added by LanQian
        /// <returns></returns>
        [OperationContract]
        MultiMessage<InstallationInfo> GetInstallationInProgressFuzzy(string carNumber, string suiteId, string installStationId,
            DateTime? beginDate, DateTime? endDate, PagingInfo pageInfo);
        /// <summary>
        /// For installation records have been completed, fuzzy query, paging query
        /// </summary>
        /// <param name="carNumber">License plate number</param>
        /// <param name="suiteId">Security Suite No.</param>
        /// <param name="installStationId">Mounting points</param>
        /// <param name="beginDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="pageInfo">Paging Information</param>
        /// Security Suite for "Installation Verification added by LanQian
        /// <returns></returns>
        [OperationContract]
        MultiMessage<InstallationInfo> GetInstallationFinishedFuzzy(string carNumber, string suiteId, string installStationId, DateTime? beginDate, DateTime? endDate, PagingInfo pageInfo);
        /// <summary>
        /// Check the video file is passed, if by updating the database
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> CheckMediaFile(string InstallId, string size);

        /// <summary>
        /// submit for step 1 of installation
        /// </summary>
        /// <param name="step2"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<string> SubmitForStep1(Step1Package step);

        /// <summary>
        /// submit for step 2 of installation
        /// </summary>
        /// <param name="step2"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<string> SubmitForStep2(Step2Package step);

        [OperationContract]
        SingleMessage<string> SubmitForStep4(string installDetailID, List<CameraInfo> cameraInfos);
        /// <summary>
        /// submit for step 2 of installation
        /// </summary>
        /// <param name="step2"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<string> SubmitForStep6(Step6Package step);
        /// <summary>
        /// submit for maintenance of installation
        /// </summary>
        /// <param name="step2"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<string> SubmitForMaintenance(MaintenancePackage package);

        /// <summary>
        /// submit for delete of installation
        /// </summary>
        /// <param name="step2"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<string> SubmitForDelete(DeletePackage package);

        [OperationContract]
        MultiMessage<CameraInfo> GetCameraInfoByMdvrID(string suiteID);



        /// <summary>
        /// submit for step 1 of GPS installation
        /// </summary>
        /// <param name="step2"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<string> SubmitGPSForStep1(Step1Package step);

        [OperationContract]
        SingleMessage<string> SubmitGPSForStep2(Step2Package step);

        [OperationContract]
        SingleMessage<string> SubmitGPSForStep3(string installid);

        [OperationContract]
        SingleMessage<string> SubmitGPSForStep4(string installid);

        [OperationContract]
        SingleMessage<SetAlarmPara> GetAlarmParaCommandResult(string installationDetailID);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>获取</returns>
        [OperationContract]
        MultiMessage<InstallStatisticsView> GetInstallStatisticsViewList(string clientID, string organizationID, List<string> stations, string vehicleType, DateTime startTime, DateTime endTime);

        [OperationContract]
        MultiMessage<DeviceAlertStatistics> GetDeviceAlertStatisticsViewList(string clientID, string organizationID, string vehicleID, DateTime startTime, DateTime endTime, List<string> stations, int pageSize, int pageIndex);
    }
}
