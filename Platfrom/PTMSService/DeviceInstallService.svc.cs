using Gsafety.PTMS.Installation.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.Installation.Repository;
using Gsafety.PTMS.BaseInfo;
using Gsafety.Common.Logging;
using System.Diagnostics;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Installation.Contract.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Common.Data;

namespace Gs.PTMS.Service
{
    // Note: Use the "reconstruction" "Rename" command on the menu, you can change the code simultaneously, svc and config file class name "DeviceInstallService".
    // Note: To start the WCF Test Client to test this service, please select DeviceInstallService.svc or DeviceInstallService.svc.cs in Solution Explorer, and then start debugging.
    public class DeviceInstallService : BaseService, IDeviceInstallService
    {

        private InstallationRepository Repository = new InstallationRepository();

        /// <summary>
        /// Test equipment currently installed license plate number to upload
        /// </summary>
        /// <param name="installationId">Equipment installation unique number</param>
        /// <returns>Calibration results</returns>
        public SingleMessage<String> GetInstallingSuiteVehicleId(string mdvrId)
        {
            try
            {
                Info("GetInstallingSuiteVehicleId");
                Info("mdvrId:" + Convert.ToString(mdvrId));
                var result = string.Empty;
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = Repository.GetInstallingSuiteVehicleIdCheckSuiteCar(context, mdvrId);
                }

                if (result != null)
                {
                    Info("GetInstallingSuiteVehicleId return value:" + result.ToString());
                }
                else
                {
                    Info("GetInstallingSuiteVehicleId return value:null");
                }
                return new SingleMessage<String>() { Result = result, IsSuccess = true };
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<String>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        /// <summary>
        /// Installation Record Results
        /// </summary>
        /// <param name="installationId">Installation Record ID</param>
        /// <returns></returns>
        public SingleMessage<InstallInfoResult> GetInstallationResult(string installationId)
        {
            try
            {
                Info("GetInstallationResult");
                Info("installationId:" + Convert.ToString(installationId));
                var temp = new InstallInfoResult();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.GetInstallationResult(context, installationId);
                }

                SingleMessage<InstallInfoResult> result = new SingleMessage<InstallInfoResult>() { Result = temp, IsSuccess = true };
                Log<InstallInfoResult>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<InstallInfoResult>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }


        /// <summary>
        /// For installation information based on the primary key ID
        /// </summary>
        /// <param name="installationId">Install the primary key Id</param>
        /// <returns>Installation Information</returns>
        public SingleMessage<InstallationInfo> GetInstallationDetail(string installationId)
        {
            try
            {
                Info("GetInstallationDetail");
                Info("installationId:" + Convert.ToString(installationId));
                var temp = new InstallationInfo();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.GetInstallationDetail(context, installationId);
                }

                SingleMessage<InstallationInfo> result = new SingleMessage<InstallationInfo>() { Result = temp, IsSuccess = true };
                Log<InstallationInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<InstallationInfo>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        /// <summary>
        /// For installation information based on the primary key ID
        /// </summary>
        /// <param name="installationId">Install the primary key Id</param>
        /// <returns>Installation Information</returns>
        public SingleMessage<InstallationInfo> GetGPSInstallationDetail(string installationId)
        {
            try
            {
                Info("GetInstallationDetail");
                Info("installationId:" + Convert.ToString(installationId));
                var temp = new InstallationInfo();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.GetGPSInstallationDetail(context, installationId);
                }

                SingleMessage<InstallationInfo> result = new SingleMessage<InstallationInfo>() { Result = temp, IsSuccess = true };
                Log<InstallationInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<InstallationInfo>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        /// <summary>
        /// Add a new installation record
        /// </summary>
        /// <param name="installation">Installation Record</param>
        /// <returns>True indicates success, False indicates failure</returns>
        public SingleMessage<bool> AddInstallation(InstallationInfo installation)
        {
            try
            {
                Info("AddInstallation");
                Info("installation:" + Convert.ToString(installation));
                var temp = false;
                installation.CreateTime = DateTime.UtcNow;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.AddInstallation(context, installation);
                }

                SingleMessage<bool> result = new SingleMessage<bool>() { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        /// <summary>
        /// Update Installation Record
        /// </summary>
        /// <param name="installation">Installation Record</param>
        /// <returns>True indicates success, False indicates failure</returns>
        public SingleMessage<bool> UpdateInstallation(InstallationInfo installation)
        {
            try
            {
                Info("UpdateInstallation");
                Info("installation:" + Convert.ToString(installation));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.UpdateInstallation(context, installation);
                }

                SingleMessage<bool> result = new SingleMessage<bool>() { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        /// <summary>
        /// Delete the corresponding primary key installation data according to the installation Id
        /// </summary>
        /// <param name="installationId">Install the primary key Id</param>
        /// <returns>true indicates deleted successfully, false representation delete failed</returns>
        public SingleMessage<bool> DeleteInstallation(string installationId)
        {
            try
            {
                Info("DeleteInstallation");
                Info("installationId:" + Convert.ToString(installationId));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.DeleteInstallation(context, installationId);
                }

                SingleMessage<bool> result = new SingleMessage<bool>() { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        /// <summary>
        /// Add a new installation audit records
        /// </summary>
        /// <param name="audit">Id primary key</param>
        /// <returns>True indicates success, False indicates failure</returns>
        [Obsolete]
        public SingleMessage<bool> AddInstallationAudit(InstallationAudit audit)
        {
            try
            {
                Info("AddInstallationAudit");
                Info("audit:" + Convert.ToString(audit));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.AddInstallationAudit(context, audit);
                }

                SingleMessage<bool> result = new SingleMessage<bool>() { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        /// <summary>
        /// Delete the installation audit records based on the primary key Id
        /// </summary>
        /// <param name="auditId">Id primary key installation audit records</param>
        /// <returns>True indicates success, False indicates failure</returns>
        [Obsolete]
        public SingleMessage<bool> DeleteInstallationAudit(string auditId)
        {
            try
            {
                Info("DeleteInstallationAudit");
                Info("auditId:" + Convert.ToString(auditId));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.DeleteInstallationAudit(context, auditId);
                }

                SingleMessage<bool> result = new SingleMessage<bool>() { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        /// <summary>
        /// Record a mount point for installation unfinished
        /// </summary>
        /// <param name="installStationId">Installation point numbers, which are all empty</param>
        /// <returns></returns>
        public MultiMessage<InstallationInfo> GetInstallationInProgress(string installStationId)
        {
            try
            {
                Info("GetInstallationInProgress");
                Info("installStationId:" + Convert.ToString(installStationId));
                var temp = new List<InstallationInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.GetInstallationInProgress(context, installStationId);
                }

                MultiMessage<InstallationInfo> result = new MultiMessage<InstallationInfo>() { Result = temp, TotalRecord = temp.Count, IsSuccess = true };
                Log<InstallationInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallationInfo>() { ExceptionMessage = ex, TotalRecord = 0, IsSuccess = false };
            }
        }



        /// <summary>
        /// Get the installation log unfinished (fuzzy query)
        /// </summary>
        /// <param name="carNumber">License plate number</param>
        /// <param name="suiteId">Security Suite No.</param>
        /// <param name="installer">Installer</param>
        /// <param name="installStationId">Mounting pointsid</param>
        /// <returns></returns>
        public MultiMessage<InstallationInfo> GetInstallationInProgressEx(string clientID, string carNumber, string suiteId, string installer, List<string> installStationIds, DateTime? beginDate, DateTime? endDate, PagingInfo pageInfo)
        {
            try
            {
                Info("GetInstallationInProgressEx");
                Info("carNumber:" + Convert.ToString(carNumber) + ";" + "suiteId:" + Convert.ToString(suiteId) + ";" + "installer:" + Convert.ToString(installer) + ";" + "installStationId:" + Convert.ToString(installStationIds) + ";" + "beginDate:" + Convert.ToString(beginDate) + ";" + "endDate:" + Convert.ToString(endDate) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                int totalCount = 0;
                var temp = new List<InstallationInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.GetInstallationInProgressEx(context, clientID, carNumber, suiteId, installer, installStationIds, beginDate, endDate, pageInfo, out totalCount);
                }

                MultiMessage<InstallationInfo> result = new MultiMessage<InstallationInfo>() { Result = temp, TotalRecord = totalCount, IsSuccess = true };
                Log<InstallationInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallationInfo>() { ExceptionMessage = ex, TotalRecord = 0, IsSuccess = false };
            }
        }

        public MultiMessage<InstallationInfo> GetGPSInstallationInProgressEx(string carNumber, string gpsid, string installer, List<string> installStationIds, DateTime? beginDate, DateTime? endDate, PagingInfo pageInfo)
        {
            try
            {
                Info("GetInstallationInProgressEx");
                Info("carNumber:" + Convert.ToString(carNumber) + ";" + "suiteId:" + Convert.ToString(gpsid) + ";" + "installer:" + Convert.ToString(installer) + ";" + "installStationId:" + Convert.ToString(installStationIds) + ";" + "beginDate:" + Convert.ToString(beginDate) + ";" + "endDate:" + Convert.ToString(endDate) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                int totalCount = 0;
                var temp = new List<InstallationInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.GetGPSInstallationInProgressEx(context, carNumber, gpsid, installer, installStationIds, beginDate, endDate, pageInfo, out totalCount);
                }

                MultiMessage<InstallationInfo> result = new MultiMessage<InstallationInfo>() { Result = temp, TotalRecord = totalCount, IsSuccess = true };
                Log<InstallationInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallationInfo>() { ExceptionMessage = ex, TotalRecord = 0, IsSuccess = false };
            }
        }

        /// <summary>
        /// Get unfinished installation records, fuzzy query, paging query
        /// </summary>
        /// <param name="carNumber">License plate number</param>
        /// <param name="suiteId">Security Suite No.</param>
        /// <param name="installStationId">Mounting points</param>
        /// <param name="beginDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="pageInfo">Paging Information</param>
        /// <returns></returns>
        public MultiMessage<InstallationInfo> GetInstallationInProgressFuzzy(string carNumber, string suiteId, string installStationId,
            DateTime? beginDate, DateTime? endDate, PagingInfo pageInfo)
        {
            try
            {
                Info("GetInstallationInProgressFuzzy");
                Info("carNumber:" + Convert.ToString(carNumber) + ";" + "suiteId:" + Convert.ToString(suiteId) + ";" + "installStationId:" + Convert.ToString(installStationId) + ";" + "beginDate:" + Convert.ToString(beginDate) + ";" + "endDate:" + Convert.ToString(endDate) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                UserInfoMessageHeader userInfo = GetUserInfo();
                var result = new MultiMessage<InstallationInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = Repository.GetInstallationInProgressFuzzy(context, userInfo, carNumber, suiteId, installStationId, beginDate, endDate, pageInfo);
                }


                Log<InstallationInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallationInfo>() { ExceptionMessage = ex, TotalRecord = 0 };
            }
        }

        /// <summary>
        /// For installation records have been completed
        /// </summary>
        /// <param name="installStationId">Mounting points</param>
        /// <param name="beginDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <returns></returns>
        public MultiMessage<InstallationInfo> GetInstallationFinished(string installStationId, DateTime beginDate, DateTime endDate)
        {
            try
            {
                Info("GetInstallationFinished");
                Info("installStationId:" + Convert.ToString(installStationId) + ";" + "beginDate:" + Convert.ToString(beginDate) + ";" + "endDate:" + Convert.ToString(endDate));
                var temp = new List<InstallationInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.GetInstallationFinished(context, installStationId, beginDate, endDate);
                }

                MultiMessage<InstallationInfo> result = new MultiMessage<InstallationInfo>() { Result = temp, TotalRecord = temp.Count, IsSuccess = true };
                Log<InstallationInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallationInfo>() { ExceptionMessage = ex, TotalRecord = -1, IsSuccess = false };
            }
        }

        /// <summary>
        /// For installation records have been completed, fuzzy query
        /// </summary>
        /// <param name="carNumber">License plate number</param>
        /// <param name="suiteId">Security Suite No.</param>
        /// <param name="installer">Installer</param>
        /// <param name="installStationId">Mounting points</param>
        /// <param name="beginDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <returns></returns>
        public MultiMessage<InstallationInfo> GetInstallationFinishedEx(string carNumber, string suiteId, string installer, string installStationId, DateTime beginDate, DateTime endDate)
        {
            try
            {
                Info("GetInstallationFinishedEx");
                Info("carNumber:" + Convert.ToString(carNumber) + ";" + "suiteId:" + Convert.ToString(suiteId) + ";" + "installer:" + Convert.ToString(installer) + ";" + "installStationId:" + Convert.ToString(installStationId) + ";" + "beginDate:" + Convert.ToString(beginDate) + ";" + "endDate:" + Convert.ToString(endDate));
                var temp = new List<InstallationInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.GetInstallationFinishedEx(context, carNumber, suiteId, installer, installStationId, beginDate, endDate);
                }

                MultiMessage<InstallationInfo> result = new MultiMessage<InstallationInfo>() { Result = temp, TotalRecord = temp.Count, IsSuccess = true };
                Log<InstallationInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallationInfo>() { ExceptionMessage = ex, TotalRecord = 0, IsSuccess = false };
            }
        }

        /// <summary>
        /// For installation records have been completed, fuzzy query, paging query
        /// </summary>
        /// <param name="carNumber">License plate number</param>
        /// <param name="suiteId">Security Suite No.</param>
        /// <param name="installStationId">Mounting points</param>
        /// <param name="beginDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="pageInfo">Paging Information</param>
        /// <returns></returns>
        public MultiMessage<InstallationInfo> GetInstallationFinishedFuzzy(string carNumber, string suiteId, string installStationId, DateTime? beginDate, DateTime? endDate, PagingInfo pageInfo)
        {
            try
            {
                Info("GetInstallationFinishedFuzzy");
                Info("carNumber:" + Convert.ToString(carNumber) + ";" + "suiteId:" + Convert.ToString(suiteId) + ";" + "installStationId:" + Convert.ToString(installStationId) + ";" + "beginDate:" + Convert.ToString(beginDate) + ";" + "endDate:" + Convert.ToString(endDate) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                UserInfoMessageHeader userInfo = GetUserInfo();
                MultiMessage<InstallationInfo> result = new MultiMessage<InstallationInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = Repository.GetInstallationFinishedFuzzy(context, userInfo, carNumber, suiteId, installStationId, beginDate, endDate, pageInfo);
                }


                Log<InstallationInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallationInfo>() { ExceptionMessage = ex, TotalRecord = 0 };
            }
        }

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
        public MultiMessage<InstallationInfo> GetInstallationFinishedEx1(string clientID, string carNumber, string suiteId, string installer, List<string> installStationId, DateTime? beginDate, DateTime? endDate, PagingInfo pageInfo)
        {
            try
            {
                Info("GetInstallationFinishedEx1");
                Info("carNumber:" + Convert.ToString(carNumber) + ";" + "suiteId:" + Convert.ToString(suiteId) + ";" + "installer:" + Convert.ToString(installer) + ";" + "installStationId:" + Convert.ToString(installStationId) + ";" + "beginDate:" + Convert.ToString(beginDate) + ";" + "endDate:" + Convert.ToString(endDate) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                int totalCount = 0;
                var temp = new List<InstallationInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.GetInstallationFinishedEx1(context, clientID, carNumber, suiteId, installer, installStationId, beginDate, endDate, pageInfo, out totalCount);
                }

                MultiMessage<InstallationInfo> result = new MultiMessage<InstallationInfo>() { Result = temp, TotalRecord = totalCount, IsSuccess = true };
                Log<InstallationInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallationInfo>() { ExceptionMessage = ex, TotalRecord = 0, IsSuccess = false };
            }
        }

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
        public MultiMessage<InstallationInfo> GetGPSInstallationFinished(string carNumber, string gpsid, string installer, List<string> installStationId, DateTime? beginDate, DateTime? endDate, PagingInfo pageInfo)
        {
            try
            {
                Info("GetGPSInstallationFinished");
                Info("carNumber:" + Convert.ToString(carNumber) + ";" + "suiteId:" + Convert.ToString(gpsid) + ";" + "installer:" + Convert.ToString(installer) + ";" + "installStationId:" + Convert.ToString(installStationId) + ";" + "beginDate:" + Convert.ToString(beginDate) + ";" + "endDate:" + Convert.ToString(endDate) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                int totalCount = 0;
                var temp = new List<InstallationInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.GetGPSInstallationFinished(context, carNumber, gpsid, installer, installStationId, beginDate, endDate, pageInfo, out totalCount);
                }

                MultiMessage<InstallationInfo> result = new MultiMessage<InstallationInfo>() { Result = temp, TotalRecord = totalCount, IsSuccess = true };
                Log<InstallationInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallationInfo>() { ExceptionMessage = ex, TotalRecord = 0, IsSuccess = false };
            }
        }

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
        public MultiMessage<InstallationAuditCollect> GetInstallationAudit(string carNumber, string suiteId, string installer, string installStationId, DateTime? beginDate, DateTime? endDate, PagingInfo pageInfo, bool onlyWaitCheck)
        {
            try
            {
                Info("GetInstallationAudit");
                Info("carNumber:" + Convert.ToString(carNumber) + ";" + "suiteId:" + Convert.ToString(suiteId) + ";" + "installer:" + Convert.ToString(installer) + ";" + "installStationId:" + Convert.ToString(installStationId) + ";" + "beginDate:" + Convert.ToString(beginDate) + ";" + "endDate:" + Convert.ToString(endDate) + ";" + "pageInfo:" + Convert.ToString(pageInfo) + ";" + "onlyWaitCheck:" + Convert.ToString(onlyWaitCheck));
                int totalCount = 0;
                var temp = new List<InstallationAuditCollect>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.GetInstallationAudit(context, carNumber, suiteId, installer, installStationId, beginDate, endDate, pageInfo, onlyWaitCheck, out totalCount);
                }

                MultiMessage<InstallationAuditCollect> result = new MultiMessage<InstallationAuditCollect>() { Result = temp, TotalRecord = totalCount, IsSuccess = true };
                Log<InstallationAuditCollect>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallationAuditCollect>() { ExceptionMessage = ex, TotalRecord = 0, IsSuccess = false };
            }
        }

        /// <summary>
        /// Check the video file is passed, if by updating the database
        /// </summary>
        /// <param name="size">File Size</param>
        public SingleMessage<bool> CheckMediaFile(string InstallId, string size)
        {
            try
            {
                Info("CheckMediaFile");
                Info("InstallId:" + Convert.ToString(InstallId) + ";" + "size:" + Convert.ToString(size));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.CheckMediaFile(context, InstallId, size);
                }

                SingleMessage<bool> result = new SingleMessage<bool>() { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<string> SubmitForStep1(Step1Package step)
        {
            try
            {
                Info("SubmitForStepOne");
                Info("step:" + Convert.ToString(step));

                SingleMessage<string> result = new SingleMessage<string>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = Repository.SubmitForStep1(context, step);
                }

                Log<string>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<string>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<string> SubmitForStep2(Step2Package step)
        {
            try
            {
                Info("SubmitForStep2");

                SingleMessage<string> result = new SingleMessage<string>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = Repository.SubmitForStep2(context, step);
                }

                Log<string>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<string>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<string> SubmitForStep4(string installDetailID, List<CameraInfo> cameraInfos)
        {
            try
            {
                Info("SubmitForStep4");

                SingleMessage<string> result = new SingleMessage<string>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = Repository.SubmitForStep4(context, installDetailID, cameraInfos);
                }

                Log<string>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<string>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<string> SubmitForStep6(Step6Package step)
        {
            try
            {
                Info("SubmitForStep6");

                SingleMessage<string> result = new SingleMessage<string>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = Repository.SubmitForStep6(context, step);
                }

                Log<string>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<string>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<string> SubmitForMaintenance(MaintenancePackage package)
        {
            try
            {
                Info("SubmitForMaintenance");

                SingleMessage<string> result = new SingleMessage<string>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = Repository.SubmitForMaintenance(context, package);
                }

                Log<string>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<string>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<string> SubmitForDelete(DeletePackage package)
        {
            try
            {
                Info("SubmitForDelete");

                SingleMessage<string> result = new SingleMessage<string>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = Repository.SubmitForDelete(context, package);
                }

                Log<string>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<string>() { ExceptionMessage = ex, IsSuccess = false };
            }

        }

        public SingleMessage<string> SubmitGPSForStep1(Step1Package step)
        {
            try
            {
                Info("SubmitGPSForStep1");
                Info("step:" + Convert.ToString(step));

                SingleMessage<string> result = new SingleMessage<string>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = Repository.SubmitGPSForStep1(context, step);
                }

                Log<string>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<string>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<string> SubmitGPSForStep2(Step2Package step)
        {
            try
            {
                Info("SubmitForStep2");

                SingleMessage<string> result = new SingleMessage<string>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = Repository.SubmitGPSForStep2(context, step);
                }

                Log<string>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<string>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<string> SubmitGPSForStep3(string installid)
        {
            try
            {
                Info("SubmitGPSForStep3");

                SingleMessage<string> result = new SingleMessage<string>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = Repository.SubmitGPSForStep3(context, installid);
                }

                Log<string>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<string>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<string> SubmitGPSForStep4(string installid)
        {
            try
            {
                Info("SubmitGPSForStep4");

                SingleMessage<string> result = new SingleMessage<string>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = Repository.SubmitGPSForStep4(context, installid);
                }

                Log<string>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<string>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public MultiMessage<CameraInfo> GetCameraInfoByMdvrID(string suiteID)
        {
            try
            {
                Info("GetCameraInfoByMdvrID");

                MultiMessage<CameraInfo> result = new MultiMessage<CameraInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = Repository.GetCameraInfoByMdvrID(context, suiteID);
                }

                Log<CameraInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<CameraInfo>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<SetAlarmPara> GetAlarmParaCommandResult(string installationDetailID)
        {
            try
            {
                Info("GetVideoAutoUploadCommandResult");

                SingleMessage<SetAlarmPara> result = new SingleMessage<SetAlarmPara>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = Repository.GetAlarmParaCommandResult(context, installationDetailID);
                }

                Log<SetAlarmPara>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<SetAlarmPara>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }


        public MultiMessage<InstallStatisticsView> GetInstallStatisticsViewList(string clientID, string organizationID, List<string> stations, string vehicleType, DateTime startTime, DateTime endTime)
        {
            Info("GetInstallStatisticsViewList");

            try
            {
                MultiMessage<InstallStatisticsView> result = null;
                using (var context = new PTMSEntities())
                {
                    result = Repository.GetInstallStatisticsViewList(context, clientID, organizationID, stations, vehicleType, startTime, endTime);
                }
                Log<InstallStatisticsView>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallStatisticsView>(false, ex);
            }
        }


        public MultiMessage<DeviceAlertStatistics> GetDeviceAlertStatisticsViewList(string clientID, string organizationID, string vehicleID, DateTime startTime, DateTime endTime, List<string> stations, int pageSize, int pageIndex)
        {
            Info("GetDeviceAlertStatisticsViewList");

            try
            {
                MultiMessage<DeviceAlertStatistics> result = null;
                using (var context = new PTMSEntities())
                {
                    result = Repository.GetDeviceAlertStatisticsViewList(context, clientID, organizationID, vehicleID, startTime, endTime, stations, pageSize, pageIndex);
                }
                Log<DeviceAlertStatistics>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<DeviceAlertStatistics>(false, ex);
            }
        }
    }
}
