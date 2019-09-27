using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Manager.Contract;
using Gsafety.PTMS.Manager.Contract.Data;
using Gsafety.PTMS.Manager.Repository;
using LogServiceContract;
using LogServiceRepository;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace Gs.PTMS.Service
{
    public class LogManageService : BaseService, IAlarmDealLogService, ICarAlertDealLogService, IInstallLogService, ILoginLogService, IVideoDowmloadLogService, IVideoLogSerVice, IVisitLogService, IGetSuiteInfoService, ILogOperate, ILogVideo
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
        public MultiMessage<AlarmDealLogInfo> GetAlarmDealLog(string clientID, string vehicleID, string userName, DateTime startTime, DateTime endTime, PagingInfo pageInfo)
        {
            try
            {
                Info("GetAlarmDealLog");
                Info("userName:" + Convert.ToString(userName) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                int totalCount = 0;
                var temp = new List<AlarmDealLogInfo>();
                using(PTMSEntities context = new PTMSEntities())
                {
                    temp = logManageRepository.GetAlarmDealLog(context, clientID, vehicleID, userName, startTime, endTime, pageInfo.PageSize, pageInfo.PageIndex, out totalCount);
                }

                MultiMessage<AlarmDealLogInfo> result = new MultiMessage<AlarmDealLogInfo> { Result = temp, TotalRecord = totalCount, IsSuccess = true };
                Log<AlarmDealLogInfo>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new MultiMessage<AlarmDealLogInfo>() { Result = null, TotalRecord = 0, ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public MultiMessage<CarAlertLogInfo> GetCarAlertDealLog(string clientID, string vehicleId, string userName, DateTime startTime, DateTime endTime, PagingInfo pageInfo)
        {
            try
            {
                Info("GetCarAlertDealLog");
                Info("vehicleId:" + Convert.ToString(vehicleId) + ";" + "userName:" + Convert.ToString(userName) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                int totalCount = 0;
                var temp = new List<CarAlertLogInfo>();
                using(PTMSEntities context = new PTMSEntities())
                {
                    temp = logManageRepository.GetCarAlertDealLog(context, vehicleId, userName, startTime, endTime, pageInfo.PageSize, pageInfo.PageIndex, out totalCount);
                }

                MultiMessage<CarAlertLogInfo> result = new MultiMessage<CarAlertLogInfo> { Result = temp, TotalRecord = totalCount };
                Log<CarAlertLogInfo>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new MultiMessage<CarAlertLogInfo>() { Result = null, TotalRecord = 0, ExceptionMessage = ex };
            }
        }
        public MultiMessage<InstallLogInfo> GetInstallLog(string clientID, string installStation, string installStaff, DateTime startTime, DateTime endTime, PagingInfo pageInfo)
        {
            try
            {
                Info("GetInstallLog");
                Info("userName:" + Convert.ToString(installStaff) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                int totalCount = 0;
                var temp = new List<InstallLogInfo>();
                using(PTMSEntities context = new PTMSEntities())
                {
                    temp = logManageRepository.GetInstallLog(context, clientID, installStation, installStaff, startTime, endTime, pageInfo.PageSize, pageInfo.PageIndex, out totalCount);
                }

                MultiMessage<InstallLogInfo> result = new MultiMessage<InstallLogInfo> { Result = temp, TotalRecord = totalCount };
                Log<InstallLogInfo>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallLogInfo>() { Result = null, ExceptionMessage = ex, TotalRecord = 0 };
            }
        }

        public SingleMessage<bool> AddLoginLog(Gsafety.PTMS.Common.Data.LogAccess access)
        {
            Info("InsertLogAccess");
            Info(access.ToString());
            try
            {
                SingleMessage<bool> result = null;
                access.LoginTime = DateTime.UtcNow;
                using(var context = new PTMSEntities())
                {
                    result = LogAccessRepository.InsertLogAccess(context, access);
                }
                Log<bool>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        public MultiMessage<LogAccess> GetLoginLog(string clientID, string userName, DateTime? startTime, DateTime? endTime, PagingInfo pageInfo)
        {
            try
            {
                Info("GetLoginLog");
                Info("userName:" + Convert.ToString(userName) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                int totalCount = 0;
                var temp = new List<LogAccess>();
                using(PTMSEntities context = new PTMSEntities())
                {
                    temp = LogAccessRepository.GetLoginLog(context, clientID, userName, startTime, endTime, pageInfo.PageSize, pageInfo.PageIndex, out totalCount);
                }

                MultiMessage<LogAccess> result = new MultiMessage<LogAccess> { Result = temp, TotalRecord = totalCount };
                Log<LogAccess>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new MultiMessage<LogAccess>() { Result = null, TotalRecord = 0, ExceptionMessage = ex };
            }
        }

        public SingleMessage<Boolean> ClearLoginLog(string clientID, string userID, string username, string content)
        {
            try
            {
                Info("ClearLoginLog");

                var temp = false;
                using(PTMSEntities context = new PTMSEntities())
                {
                    TransactionOptions optons = new TransactionOptions();
                    optons.IsolationLevel = IsolationLevel.ReadCommitted;
                    var scope = new TransactionScope(TransactionScopeOption.Required, optons);
                    try
                    {
                        temp = logManageRepository.ClearLoginLog(context, clientID, userID, username, content);
                        if(temp)
                        {
                            scope.Complete();
                        }
                    }
                    catch(Exception ex)
                    {
                        throw;
                    }
                    finally
                    {
                        scope.Dispose();
                    }

                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch(Exception ex)
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
                using(PTMSEntities context = new PTMSEntities())
                {
                    temp = logManageRepository.GetVideoDownloadLog(context, downloader, type, startTime, endTime, pageInfo.PageSize, pageInfo.PageIndex, out totalCount);
                }

                MultiMessage<VideoLogInfo> result = new MultiMessage<VideoLogInfo> { Result = temp, TotalRecord = totalCount };
                Log<VideoLogInfo>(result);
                return result;
            }
            catch(Exception ex)
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
                using(PTMSEntities context = new PTMSEntities())
                {
                    temp = logManageRepository.GetVideoLog(context, player, type, startTime, endTime, pageInfo.PageSize, pageInfo.PageIndex, out totalCount);
                }

                MultiMessage<VideoLogInfo> result = new MultiMessage<VideoLogInfo> { Result = temp, TotalRecord = totalCount };
                Log<VideoLogInfo>(result);
                return result;
            }
            catch(Exception ex)
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
                visitLog.VisitTime = DateTime.UtcNow;
                using(PTMSEntities context = new PTMSEntities())
                {
                    temp = logManageRepository.AddVisitLog(context, visitLog);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = true, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch(Exception ex)
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
                using(PTMSEntities context = new PTMSEntities())
                {
                    temp = logManageRepository.GetVisitLog(context, visitor, type, startTime, endTime, pageInfo.PageSize, pageInfo.PageIndex, out totalCount);
                }

                MultiMessage<VisitLogInfo> result = new MultiMessage<VisitLogInfo> { Result = temp, TotalRecord = totalCount };
                Log<VisitLogInfo>(result);
                return result;
            }
            catch(Exception ex)
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
                using(PTMSEntities context = new PTMSEntities())
                {
                    temp = logManageRepository.ClearVisitLog(context, startTime, endTime);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new SingleMessage<Boolean>() { Result = false, IsSuccess = false, ExceptionMessage = ex };
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
                using(PTMSEntities context = new PTMSEntities())
                {
                    temp = logManageRepository.GetSuiteInfo(context, suiteID, startTime, endTime, pageInfo.PageSize, pageInfo.PageIndex, out totalCount);
                }

                MultiMessage<SuiteInfoLog> result = new MultiMessage<SuiteInfoLog> { Result = temp, TotalRecord = totalCount };
                Log<SuiteInfoLog>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new MultiMessage<SuiteInfoLog>() { Result = null, ExceptionMessage = ex };
            }
        }
        #endregion


        public MultiMessage<LogOperate> GetOperationLogList(string userName, string clientID, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            try
            {
                Info("GetSuitInfo");
                Info("suiteID:" + Convert.ToString(clientID) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime) + ";" + "pageIndex:" + Convert.ToString(pageIndex) + "pageSize" + pageSize.ToString());

                MultiMessage<LogOperate> result = null;
                using(PTMSEntities context = new PTMSEntities())
                {
                    result = LogOperateRepository.GetLogOperateList(context, userName, clientID, startTime, endTime, pageIndex, pageSize);
                }

                Log<LogOperate>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new MultiMessage<LogOperate>() { Result = null, ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> ClearOperateLog(string clientID, LogOperate log)
        {
            try
            {
                Info("ClearOperateLog");

                var temp = false;
                using(PTMSEntities context = new PTMSEntities())
                {
                    TransactionOptions optons = new TransactionOptions();
                    optons.IsolationLevel = IsolationLevel.ReadCommitted;
                    var scope = new TransactionScope(TransactionScopeOption.Required, optons);

                    try
                    {
                        temp = LogOperateRepository.ClearOperateLog(context, clientID, log);
                        if(temp)
                        {
                            scope.Complete();
                        }

                    }
                    catch(Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        scope.Dispose();
                        scope = null;
                    }
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new SingleMessage<Boolean>() { Result = false, IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> InsertVideoDownloadLog(List<LogVideo> models)
        {
            Info("InsertVideoDownloadLog");
            foreach(var model in models)
            {
                Info(model.ToString());
                foreach(var item in models)
                {
                    item.OperateTime = DateTime.UtcNow;
                }
                model.LogType = (short)VideoLogTypeEnum.Download;
            }

            try
            {
                SingleMessage<bool> result = null;
                using(var context = new PTMSEntities())
                {
                    result = LogVideoRepository.InsertLogVideo(context, models);
                }
                Log<bool>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        public SingleMessage<bool> InsertVideoPlayLog(List<LogVideo> models)
        {
            Info("InsertVideoDownloadLog");
            foreach(var model in models)
            {
                Info(model.ToString());
                model.LogType = (short)VideoLogTypeEnum.Play;
                model.OperateTime = DateTime.UtcNow;
            }
            try
            {
                SingleMessage<bool> result = null;
                using(var context = new PTMSEntities())
                {
                    result = LogVideoRepository.InsertLogVideo(context, models);
                }
                Log<bool>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        public SingleMessage<bool> ClearVideoDownloadLog(string ClientID, LogOperate log)
        {
            try
            {
                Info("ClearVideoDownloadLog");

                var temp = false;
                using(PTMSEntities context = new PTMSEntities())
                {
                    TransactionOptions optons = new TransactionOptions();
                    optons.IsolationLevel = IsolationLevel.ReadCommitted;
                    var scope = new TransactionScope(TransactionScopeOption.Required, optons);

                    try
                    {
                        temp = LogVideoRepository.ClearVideoDownloadLog(context, ClientID, log);
                        if(temp)
                        {
                            scope.Complete();
                        }
                    }
                    catch(Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        scope.Dispose();
                        scope = null;
                    }
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new SingleMessage<Boolean>() { Result = false, IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> ClearVideoPlayLog(string ClientID, LogOperate log)
        {
            try
            {
                Info("ClearVideoDownloadLog");

                var temp = false;
                using(PTMSEntities context = new PTMSEntities())
                {
                    TransactionOptions optons = new TransactionOptions();
                    optons.IsolationLevel = IsolationLevel.ReadCommitted;
                    var scope = new TransactionScope(TransactionScopeOption.Required, optons);

                    try
                    {
                        temp = LogVideoRepository.ClearVideoPlayLog(context, ClientID, log);
                        if(temp)
                        {
                            scope.Complete();
                        }
                    }
                    catch(Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        scope.Dispose();
                        scope = null;
                    }
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new SingleMessage<Boolean>() { Result = false, IsSuccess = false, ExceptionMessage = ex };
            }
        }


        public MultiMessage<LogVideo> GetVideoPlayLogList(string clientID, string vehicleID, string userName, string mdvrCoreSN, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            try
            {
                Info("GetVideoPlayLogList");
                Info("clientID:" + Convert.ToString(clientID) + ";" + "vehicleID:" + Convert.ToString(vehicleID) + ";" + "userName:" + Convert.ToString(userName) + ";" + "mdvrCoreSN:" + Convert.ToString(mdvrCoreSN) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime) + ";" + "pageIndex:" + Convert.ToString(pageIndex) + "pageSize" + pageSize.ToString());

                MultiMessage<LogVideo> result = null;
                using(PTMSEntities context = new PTMSEntities())
                {
                    result = LogVideoRepository.GetVideoPlayLogList(context, clientID, vehicleID, userName, mdvrCoreSN, startTime, endTime, pageIndex, pageSize);
                }

                Log<LogVideo>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new MultiMessage<LogVideo>() { Result = null, ExceptionMessage = ex };
            }
        }

        public MultiMessage<LogVideo> GetVideoDownloadLogList(string clientID, string vehicleID, string userName, string mdvrCoreSN, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            try
            {
                Info("GetVideoDownloadLogList");
                Info("clientID:" + Convert.ToString(clientID) + ";" + "vehicleID:" + Convert.ToString(vehicleID) + ";" + "userName:" + Convert.ToString(userName) + ";" + "mdvrCoreSN:" + Convert.ToString(mdvrCoreSN) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime) + ";" + "pageIndex:" + Convert.ToString(pageIndex) + "pageSize" + pageSize.ToString());

                MultiMessage<LogVideo> result = null;
                using(PTMSEntities context = new PTMSEntities())
                {
                    result = LogVideoRepository.GetVideoDownloadLogList(context, clientID, vehicleID, userName, mdvrCoreSN, startTime, endTime, pageIndex, pageSize);
                }

                Log<LogVideo>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new MultiMessage<LogVideo>() { Result = null, ExceptionMessage = ex };
            }
        }
    }
}