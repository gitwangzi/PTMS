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

namespace Gsafety.PTMS.Installation.Service
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
        /// A unique number to get information based on self-test self-test
        /// </summary>
        /// <param name="inspectId">Self Test unique number</param>
        /// <returns>Self-test information</returns>
        public SingleMessage<SelfInspectInfo> GetSelfInspect(string inspectId)
        {
            try
            {
                Info("GetSelfInspect");
                Info("inspectId:" + Convert.ToString(inspectId));
                var temp = new SelfInspectInfo();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.GetSelfInspect(context, inspectId);
                }

                SingleMessage<SelfInspectInfo> result = new SingleMessage<SelfInspectInfo>() { Result = temp, IsSuccess = true };
                Log<SelfInspectInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<SelfInspectInfo>() { ExceptionMessage = ex, IsSuccess = false };
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
        public MultiMessage<InstallationInfo> GetInstallationInProgressEx(string carNumber, string suiteId, string installer, string installStationId, DateTime? beginDate, DateTime? endDate, PagingInfo pageInfo)
        {
            try
            {
                Info("GetInstallationInProgressEx");
                Info("carNumber:" + Convert.ToString(carNumber) + ";" + "suiteId:" + Convert.ToString(suiteId) + ";" + "installer:" + Convert.ToString(installer) + ";" + "installStationId:" + Convert.ToString(installStationId) + ";" + "beginDate:" + Convert.ToString(beginDate) + ";" + "endDate:" + Convert.ToString(endDate) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                int totalCount = 0;
                var temp = new List<InstallationInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.GetInstallationInProgressEx(context, carNumber, suiteId, installer, installStationId, beginDate, endDate, pageInfo, out totalCount);
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
        public MultiMessage<InstallationInfo> GetInstallationFinishedEx1(string carNumber, string suiteId, string installer, string installStationId, DateTime? beginDate, DateTime? endDate, PagingInfo pageInfo)
        {
            try
            {
                Info("GetInstallationFinishedEx1");
                Info("carNumber:" + Convert.ToString(carNumber) + ";" + "suiteId:" + Convert.ToString(suiteId) + ";" + "installer:" + Convert.ToString(installer) + ";" + "installStationId:" + Convert.ToString(installStationId) + ";" + "beginDate:" + Convert.ToString(beginDate) + ";" + "endDate:" + Convert.ToString(endDate) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                int totalCount = 0;
                var temp = new List<InstallationInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.GetInstallationFinishedEx1(context, carNumber, suiteId, installer, installStationId, beginDate, endDate, pageInfo, out totalCount);
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

        public MultiMessage<SelfInspectDetail> GetSuiteInspectFuzzy(string vehicleId, string suiteId, DateTime beginDate, DateTime endDate, PagingInfo pageInfo)
        {
            try
            {
                Info("GetSuiteInspectFuzzy");
                Info("vehicleId:" + Convert.ToString(vehicleId) + ";" + "suiteId:" + Convert.ToString(suiteId) + ";" + "beginDate:" + Convert.ToString(beginDate) + ";" + "endDate:" + Convert.ToString(endDate) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                int TotalRecord = 0;
                var temp = new List<SelfInspectDetail>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    //temp = Repository.GetSuiteInspectFuzzy(context, vehicleId, suiteId, beginDate, endDate, pageInfo, out TotalRecord, GetUserInfo());
                }

                MultiMessage<SelfInspectDetail> result = new MultiMessage<SelfInspectDetail>() { Result = temp, TotalRecord = TotalRecord, IsSuccess = true };
                Log<SelfInspectDetail>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<SelfInspectDetail>() { ExceptionMessage = ex, TotalRecord = 0, IsSuccess = false };
            }
        }


        public SingleMessage<string> SubmitForStepOne(Step1Package step)
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
    }
}
