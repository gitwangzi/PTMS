using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Manager.Contract;
using Gsafety.PTMS.Manager.Contract.Data;
using Gsafety.PTMS.Manager.Repository;
using Gsafety.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.Manager.Service
{
    public class PTMSLogManageService : BaseService, IAlarmDealLogService, ICarAlertDealLogService, IInstallLogService, ILoginLogService, IVideoDowmloadLogService, IVideoLogSerVice, IVisitLogService, IGetSuiteInfoService
    {
        PTMSLogManageRepository logManageRepository = new PTMSLogManageRepository();

        /// <summary>
        /// Get Alarm Deal Log
        /// </summary>
        /// <param name="userName">userName</param>
        /// <param name="startTime">startTime</param>
        /// <param name="endTime">endTime</param>
        /// <param name="pageInfo">pageInfo</param>
        /// <returns></returns>
        public MultiMessage<AlarmDealLogInfo> GetAlarmDealLog(string userName, DateTime startTime, DateTime endTime, PagingInfo pageInfo)
        {
            try
            {
                Info("GetAlarmDealLog");
                Info("userName:" + Convert.ToString(userName) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                int totalCount = 0;
                var temp = new List<AlarmDealLogInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = logManageRepository.GetAlarmDealLog(context, userName, startTime, endTime, pageInfo.PageSize, pageInfo.PageIndex, out totalCount);
                }

                MultiMessage<AlarmDealLogInfo> result = new MultiMessage<AlarmDealLogInfo> { Result = temp, TotalRecord = totalCount };
                Log<AlarmDealLogInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<AlarmDealLogInfo>() { Result = null, TotalRecord = 0, ExceptionMessage = ex };
            }
        }

        public MultiMessage<CarAlertLogInfo> GetCarAlertDealLog(string vehicleId, string userName, DateTime startTime, DateTime endTime, PagingInfo pageInfo)
        {
            try
            {
                Info("GetCarAlertDealLog");
                Info("vehicleId:" + Convert.ToString(vehicleId) + ";" + "userName:" + Convert.ToString(userName) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                int totalCount = 0;
                var temp = new List<CarAlertLogInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = logManageRepository.GetCarAlertDealLog(context, vehicleId, userName, startTime, endTime, pageInfo.PageSize, pageInfo.PageIndex, out totalCount);
                }

                MultiMessage<CarAlertLogInfo> result = new MultiMessage<CarAlertLogInfo> { Result = temp, TotalRecord = totalCount };
                Log<CarAlertLogInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<CarAlertLogInfo>() { Result = null, TotalRecord = 0, ExceptionMessage = ex };
            }
        }
        public MultiMessage<InstallLogInfo> GetInstallLog(string userName, DateTime startTime, DateTime endTime, PagingInfo pageInfo)
        {
            try
            {
                Info("GetInstallLog");
                Info("userName:" + Convert.ToString(userName) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                int totalCount = 0;
                var temp = new List<InstallLogInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = logManageRepository.GetInstallLog(context, userName, startTime, endTime, pageInfo.PageSize, pageInfo.PageIndex, out totalCount);
                }

                MultiMessage<InstallLogInfo> result = new MultiMessage<InstallLogInfo> { Result = temp, TotalRecord = totalCount };
                Log<InstallLogInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallLogInfo>() { Result = null, ExceptionMessage = ex, TotalRecord = 0 };
            }
        }

        public SingleMessage<string> AddLoginLog(string userName, short type, string orgname)
        {
            try
            {
                Info("AddLoginLog");
                Info("userName:" + Convert.ToString(userName) + ";" + "type:" + Convert.ToString(type) + ";" + "orgname:" + Convert.ToString(orgname));
                var temp = string.Empty;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = logManageRepository.AddLoginLog(context, userName, type, orgname);
                }

                SingleMessage<string> result = new SingleMessage<string> { Result = temp, IsSuccess = true };
                Log<string>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                Console.WriteLine(ex);
                Console.Read();
                return new SingleMessage<string>() { IsSuccess = false, ErrorDetailMsg = ex.StackTrace, ErrorMsg = ex.Message };
            }
        }

        public SingleMessage<LoginLogInfo> AddLogoutLog(string userName, string id)
        {
            try
            {
                Info("AddLogoutLog");
                Info("userName:" + Convert.ToString(userName) + ";" + "id:" + Convert.ToString(id));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = logManageRepository.AddLogoutLog(context, userName, id);
                }

                SingleMessage<LoginLogInfo> result = new SingleMessage<LoginLogInfo> { IsSuccess = temp };
                Log<LoginLogInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);

                return new SingleMessage<LoginLogInfo>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public MultiMessage<LoginLogInfo> GetUserOnline(string userName, DateTime startTime, DateTime endTime, PagingInfo pageInfo)
        {
            try
            {
                Info("GetUserOnline");
                Info("userName:" + Convert.ToString(userName) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                int totalCount = 0;
                var temp = new List<LoginLogInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = logManageRepository.GetUserOnline(context, userName, startTime, endTime, pageInfo.PageSize, pageInfo.PageIndex, out totalCount);
                }

                MultiMessage<LoginLogInfo> result = new MultiMessage<LoginLogInfo> { Result = temp, TotalRecord = totalCount };
                Log<LoginLogInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<LoginLogInfo>() { Result = null, TotalRecord = 0, ExceptionMessage = ex };
            }
        }

        public SingleMessage<Boolean> ClearLoginLog(DateTime startTime, DateTime endTime)
        {
            try
            {
                Info("ClearLoginLog");
                Info("startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = logManageRepository.ClearLoginLog(context, startTime, endTime);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<Boolean>() { Result = false, IsSuccess = false, ExceptionMessage = ex };
            }
        }
        public MultiMessage<VideoLogInfo> GetVideoDownloadLog(string downloader, string type, DateTime startTime, DateTime endTime, PagingInfo pageInfo)
        {
            try
            {
                Info("GetVideoDownloadLog");
                Info("downloader:" + Convert.ToString(downloader) + ";" + "type:" + Convert.ToString(type) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                int totalCount = 0;
                var temp = new List<VideoLogInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = logManageRepository.GetVideoDownloadLog(context, downloader, type, startTime, endTime, pageInfo.PageSize, pageInfo.PageIndex, out totalCount);
                }

                MultiMessage<VideoLogInfo> result = new MultiMessage<VideoLogInfo> { Result = temp, TotalRecord = totalCount };
                Log<VideoLogInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<VideoLogInfo>() { Result = null, TotalRecord = 0, ExceptionMessage = ex };
            }
        }

        public MultiMessage<VideoLogInfo> GetVideoLog(string player, string type, DateTime startTime, DateTime endTime, PagingInfo pageInfo)
        {
            try
            {
                Info("GetVideoLog");
                Info("player:" + Convert.ToString(player) + ";" + "type:" + Convert.ToString(type) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                int totalCount = 0;
                var temp = new List<VideoLogInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = logManageRepository.GetVideoLog(context, player, type, startTime, endTime, pageInfo.PageSize, pageInfo.PageIndex, out totalCount);
                }

                MultiMessage<VideoLogInfo> result = new MultiMessage<VideoLogInfo> { Result = temp, TotalRecord = totalCount };
                Log<VideoLogInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<VideoLogInfo>() { Result = null, TotalRecord = 0, ExceptionMessage = ex };
            }
        }

        #region
        public SingleMessage<Boolean> AddVisitLog(VisitLogInfo visitLog)
        {
            try
            {
                Info("AddVisitLog");
                Info("visitLog:" + Convert.ToString(visitLog));
                var temp = new VisitLogInfo();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = logManageRepository.AddVisitLog(context, visitLog);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = true, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<Boolean>() { Result = false, IsSuccess = false, ExceptionMessage = ex };
            }
        }
        public MultiMessage<VisitLogInfo> GetVisitLog(string visitor, int type, DateTime startTime, DateTime endTime, PagingInfo pageInfo)
        {
            try
            {
                Info("GetVisitLog");
                Info("visitor:" + Convert.ToString(visitor) + ";" + "type:" + Convert.ToString(type) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                int totalCount = 0;
                var temp = new List<VisitLogInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = logManageRepository.GetVisitLog(context, visitor, type, startTime, endTime, pageInfo.PageSize, pageInfo.PageIndex, out totalCount);
                }

                MultiMessage<VisitLogInfo> result = new MultiMessage<VisitLogInfo> { Result = temp, TotalRecord = totalCount };
                Log<VisitLogInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<VisitLogInfo>() { Result = null, TotalRecord = 0, ExceptionMessage = ex };
            }
        }
        public SingleMessage<Boolean> ClearVisitLog(DateTime startTime, DateTime endTime)
        {
            try
            {
                Info("ClearVisitLog");
                Info("startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = logManageRepository.ClearVisitLog(context, startTime, endTime);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<Boolean>() { Result = false, IsSuccess = false, ExceptionMessage = ex };
            }
        }
        public MultiMessage<LoginLogInfo> GetLoginLog(string userName, DateTime startTime, DateTime endTime, PagingInfo pageInfo)
        {
            try
            {
                Info("GetLoginLog");
                Info("userName:" + Convert.ToString(userName) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                int totalCount = 0;
                var temp = new List<LoginLogInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = logManageRepository.GetLoginLog(context, userName, startTime, endTime, pageInfo.PageSize, pageInfo.PageIndex, out totalCount);
                }

                MultiMessage<LoginLogInfo> result = new MultiMessage<LoginLogInfo> { Result = temp, TotalRecord = totalCount };
                Log<LoginLogInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<LoginLogInfo>() { Result = null, TotalRecord = 0, ExceptionMessage = ex };
            }
        }

        public MultiMessage<SuiteInfoLog> GetSuitInfo(string suiteID, DateTime startTime, DateTime endTime, PagingInfo pageInfo)
        {
            try
            {
                Info("GetSuitInfo");
                Info("suiteID:" + Convert.ToString(suiteID) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                int totalCount = 0;
                var temp = new List<SuiteInfoLog>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = logManageRepository.GetSuiteInfo(context, suiteID, startTime, endTime, pageInfo.PageSize, pageInfo.PageIndex, out totalCount);
                }

                MultiMessage<SuiteInfoLog> result = new MultiMessage<SuiteInfoLog> { Result = temp, TotalRecord = totalCount };
                Log<SuiteInfoLog>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<SuiteInfoLog>() { Result = null, ExceptionMessage = ex };
            }
        }
        #endregion
    }
}