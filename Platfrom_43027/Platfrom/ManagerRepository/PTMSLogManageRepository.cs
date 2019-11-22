using Gsafety.PTMS.Common.Data.Enum;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
///// Guid: 9d8c2ee2-20b4-434b-ae6b-6f354a75cb63      
/////clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
///// Machine Name: PC-SHIHS
///// Author: TEST(ShiHS)
/////======================================================================
/////Project Name: Gsafety.PTMS.Manager.Repository
/////Project Description:    
/////Class Name: PTMSLogManageRepository
/////Class Version: v1.0.0.0
/////Create Time: 2013/8/19 16:57:29
/////Class Description:  
/////======================================================================
/////Modified Time: 2013/8/19 16:57:29
/////Modified by:(ShiHS)
/////Modified Description: 
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Manager.Contract;
using Gsafety.PTMS.Manager.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
namespace Gsafety.PTMS.Manager.Repository
{
    /// <summary>
    /// Log management
    /// </summary>
    public class PTMSLogManageRepository
    {
        public List<AlarmDealLogInfo> GetAlarmDealLog(PTMSEntities context, string clientID, string vehicleID, string userName, DateTime startTime, DateTime endTime, int pageSize, int pageIndex, out int totalCount)
        {
            DateTime dtEnd = endTime.AddDays(1);

            var result = from f in context.ALM_ALARM_DISPOSE
                         join j in context.ALM_ALARM_RECORD on f.ALARM_ID equals j.ID
                         join v in context.BSC_VEHICLE on j.VEHICLE_ID equals v.VEHICLE_ID
                         join t in context.BSC_VEHICLE_TYPE on v.VEHICLE_TYPE equals t.ID
                         where (string.IsNullOrEmpty(userName) ? true : f.DISPOSE_STAFF.Contains(userName))
                         &&(startTime != null ? f.DISPOSE_TIME >= startTime : true)
                         &&(dtEnd != null ? f.DISPOSE_TIME <= dtEnd : true)
                         && j.CLIENT_ID == clientID && (string.IsNullOrEmpty(vehicleID) ? true : j.VEHICLE_ID.ToUpper().Contains(vehicleID.ToUpper()))
                         orderby j.GPS_TIME descending
                         select new AlarmDealLogInfo
                         {
                             AlarmVihcleID = j.VEHICLE_ID,
                             AlarmTime = j.GPS_TIME,
                             DealPerson = f.DISPOSE_STAFF,
                             DealTime = f.DISPOSE_TIME.Value,
                             Description = f.CONTENT,
                             VehicleType = t.NAME
                         };
            if (pageSize == 0 && pageIndex == -1)
            {
                totalCount = result.Count();
                return result.ToList();
            }
            else
            {
                totalCount = result.Count();
                return result.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            }

        }

        public List<CarAlertLogInfo> GetCarAlertDealLog(PTMSEntities context, string vehicleId, string userName, DateTime startTime, DateTime endTime, int pageSize, int pageIndex, out int totalCount)
        {
            DateTime dtEnd = endTime.AddDays(1);

            var result = from f in context.ALT_BUSINESS_ALERT_HANDLE
                         join j in context.ALT_BUSINESS_ALERT on f.BUSINESS_ALERT_ID equals j.ID into tj
                         from tjm in tj.DefaultIfEmpty()
                         join v in context.BSC_VEHICLE on tjm.VEHICLE_ID equals v.VEHICLE_ID into tv
                         from tvm in tv.DefaultIfEmpty()
                         join t in context.BSC_VEHICLE_TYPE on tvm.VEHICLE_TYPE equals t.ID
                         where (string.IsNullOrEmpty(vehicleId) ? true : tjm.VEHICLE_ID.ToLower().Contains(vehicleId.ToLower())) &&
                               (string.IsNullOrEmpty(userName) ? true : f.HANDLE_USER.Contains(userName)) &&
                               (startTime != null ? f.HANDLE_TIME >= startTime : true) && 
                               (dtEnd != null ? f.HANDLE_TIME <= dtEnd : true) &&
                               (tjm.STATUS == 4)
                         orderby f.HANDLE_TIME descending
                         select new CarAlertLogInfo
                         {
                             CarNumber = tjm.VEHICLE_ID,
                             VihcleType = t.NAME,
                             AlertTime = tjm.ALERT_TIME,
                             AlertType = tjm.ALERT_TYPE,
                             DealTime = f.HANDLE_TIME,
                             DealPerson = f.HANDLE_USER,
                             DealContent = f.CONTENT
                         };
            if (pageSize == 0 && pageIndex == -1)
            {
                totalCount = result.Count();
                return result.ToList();
            }
            else
            {
                totalCount = result.Count();
                return result.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            }
        }

        public List<InstallLogInfo> GetInstallLog(PTMSEntities context, string clientID, string installStation, string installStaff, DateTime startTime, DateTime endTime, int pageSize, int pageIndex, out int totalCount)
        {
            DateTime dtEnd = endTime.AddDays(1);

            var suiteresult = from detail in context.MTN_INSTALLATION_DETAIL.Where(item => item.VALID == 1)
                              join v in context.BSC_VEHICLE on detail.VEHICLE_ID equals v.VEHICLE_ID
                              join station in context.BSC_SETUP_STATION on detail.STATION_ID equals station.ID
                              let suite = context.BSC_DEV_SUITE.FirstOrDefault(n => n.SUITE_INFO_ID == detail.SUITE_INFO_ID)
                              where (string.IsNullOrEmpty(installStaff) ? true : detail.INSTALL_STAFF.Contains(installStaff))
                              && (startTime != null ? detail.CREATE_TIME >= startTime : true)
                              && (dtEnd != null ? detail.CREATE_TIME <= dtEnd : true)
                              && v.CLIENT_ID == clientID
                              && (string.IsNullOrEmpty(installStation) ? true : detail.STATION_ID == installStation)
                              orderby detail.CREATE_TIME descending
                              select new InstallLogInfo
                              {
                                  Vechicle_ID = detail.VEHICLE_ID,
                                  InstalledTime = detail.CREATE_TIME.Value,
                                  RecordStaff = detail.RECORD_STAFF,
                                  SetupStaff = detail.INSTALL_STAFF,
                                  SetupStation = station.NAME,
                                  SuiteID = suite == null ? string.Empty : suite.SUITE_ID
                              };

            var gpsresult = from detail in context.MTN_GPS_INSTALLATION_DETAIL.Where(item => item.VALID == 1)
                            join v in context.BSC_VEHICLE on detail.VEHICLE_ID equals v.VEHICLE_ID
                            join station in context.BSC_SETUP_STATION on detail.STATION_ID equals station.ID
                            let gps = context.BSC_DEV_GPS.FirstOrDefault(n => n.ID == detail.GPS_ID)
                            where (string.IsNullOrEmpty(installStaff) ? true : detail.INSTALL_STAFF.Contains(installStaff))
                            && (startTime != null ? detail.CREATE_TIME >= startTime : true)
                            && (dtEnd != null ? detail.CREATE_TIME <= dtEnd : true) 
                             && v.CLIENT_ID == clientID
                             && (string.IsNullOrEmpty(installStation) ? true : detail.STATION_ID == installStation)
                            orderby detail.CREATE_TIME descending
                            select new InstallLogInfo
                            {
                                Vechicle_ID = detail.VEHICLE_ID,
                                InstalledTime = detail.CREATE_TIME.Value,
                                RecordStaff = detail.RECORD_STAFF,
                                SetupStaff = detail.INSTALL_STAFF,
                                SetupStation = station.NAME,
                                SuiteID = gps == null ? string.Empty : gps.GPS_SN
                            };
            var result = gpsresult.Union(suiteresult);

            if (pageSize == 0 && pageIndex == -1)
            {
                totalCount = result.Count();
                return result.ToList();
            }
            else
            {
                totalCount = result.Count();
                return result.OrderByDescending(x => x.InstalledTime).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            }

        }


        // Clear the login log
        public Boolean ClearLoginLog(PTMSEntities context, string clientID, string userID, string username, string content)
        {
            TransactionOptions optons = new TransactionOptions();
            optons.IsolationLevel = IsolationLevel.ReadCommitted;
            var scope = new TransactionScope(TransactionScopeOption.Required, optons);
            bool suc = false;
            try
            {
                List<LOG_ACCESS> list = context.LOG_ACCESS.Where(n => n.CLIENT_ID == clientID).ToList();
                foreach (var item in list)
                {
                    context.LOG_ACCESS.Remove(item);
                }

                LOG_OPERATE data = new LOG_OPERATE();
                data.CLIENT_ID = clientID;
                data.OPERATE_CONTENT = content;
                data.OPERATE_TIME = DateTime.Now.ToUniversalTime();
                data.OPERATE_TYPE = (short)OperateTypeEnum.UserManage;
                data.OPERATOR_ID = userID;
                data.OPERATOR_NAME = username;
                data.ID = Guid.NewGuid().ToString();
                context.LOG_OPERATE.Add(data);
                if (context.SaveChanges() > 0)
                {
                    scope.Complete();
                    suc = true;
                }
            }
            catch
            {
                suc = false;
            }
            finally
            {
                scope.Dispose();
            }
            return suc;
        }

        /// <summary>
        /// get video download log
        /// </summary>
        /// <param name="downloader"></param>
        /// <param name="type"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<VideoLogInfo> GetVideoDownloadLog(PTMSEntities context, string downloader, string type, DateTime startTime, DateTime endTime, int pageSize, int pageIndex, out int totalCount)
        {
            DateTime dtEnd = endTime.AddDays(1);
            //var result = from log in context.VIDEO_LOG.Where(x => x.INVOKE_TYPE.Trim() == "DownloadMdvrVideo" || x.INVOKE_TYPE == "SaveVideoToLocal")
            //             //join j in context.SECURITY_SUITE_WORKING on f.MDVR_CORE_SN.Trim() equals j.MDVR_CORE_SN.Trim() into tj
            //             //from tjm in tj.DefaultIfEmpty()
            //             join working in context.SECURITY_SUITE_WORKING on log.MDVR_CORE_SN equals working.MDVR_CORE_SN
            //             join info in context.DEV_SUITE on working.SUITE_INFO_ID equals info.SUITE_INFO_ID
            //             where (string.IsNullOrEmpty(downloader) ? true : log.USER_NAME.Contains(downloader))
            //                && ((string.IsNullOrEmpty(type) || type == "All") ? true : log.INVOKE_TYPE == type)
            //                && (startTime != endTime ? log.START_TIME >= startTime && dtEnd >= log.START_TIME : log.START_TIME.Value.Year == startTime.Year && log.START_TIME.Value.Month == startTime.Month && log.START_TIME.Value.Day == startTime.Day)
            //             //orderby f.START_TIME descending
            //             select new VideoLogInfo
            //             {
            //                 Player = log.USER_NAME,
            //                 ActionTime = log.START_TIME,
            //                 SubType = log.SUB_TYPE,
            //                 Ivoke_Type = log.INVOKE_TYPE,
            //                 MDVRId = log.MDVR_CORE_SN,
            //                 VehicleID = working.VEHICLE_ID,
            //                 ChannelId = log.CHANNEL,
            //                 VideoEndtime = log.END_TIME,
            //                 VideoStartTime = log.START_TIME,
            //                 VideoFileName = log.FILE_NAME,

            //             };
            //if (pageSize == 0 && pageIndex == -1)
            //{
            //    totalCount = result.Count();
            //    return result.ToList();
            //}
            //else
            //{
            //    totalCount = result.Count();
            //    return result.OrderBy(x => x.ActionTime).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            //}
            totalCount = 0;
            return null;
        }

        ////add play video log
        //public VideoLogInfo AddVideoLog(VideoLogInfo videoLog)
        //{
        //    //detil：VideoLogRepository
        //    return null;
        //}
        /// <summary>
        /// get play video log
        /// </summary>
        /// <param name="player"></param>
        /// <param name="type"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<VideoLogInfo> GetVideoLog(PTMSEntities context, string player, string type, DateTime startTime, DateTime endTime, int pageSize, int pageIndex, out int totalCount)
        {
            //DateTime dtEnd = endTime.AddDays(1);
            //var result = from log in context.VIDEO_LOG//.Where(x => x.INVOKE_TYPE.Trim() != "DownloadMdvrVideo" && x.INVOKE_TYPE.Trim() != "SaveVideoToLocal")
            //             //join j in context.SECURITY_SUITE_WORKING on f.MDVR_CORE_SN equals j.MDVR_CORE_SN into tj
            //             //from tmj in tj.DefaultIfEmpty()
            //             from working in context.SECURITY_SUITE_WORKING
            //             from info in context.DEV_SUITE

            //             //from j in context.SECURITY_SUITE_WORKING
            //             where (log.MDVR_CORE_SN == working.MDVR_CORE_SN)
            //                && (working.SUITE_INFO_ID == info.SUITE_INFO_ID)
            //                && (log.INVOKE_TYPE.Trim() != "DownloadMdvrVideo" || log.INVOKE_TYPE != "SaveVideoToLocal")
            //                 //&& (string.IsNullOrEmpty(player) ? true : f.USER_NAME.Contains(player))
            //                && (string.IsNullOrEmpty(player) ? true : log.USER_NAME.Contains(player))
            //                && ((string.IsNullOrEmpty(type) || type == "All") ? true : log.INVOKE_TYPE == type)
            //                && (startTime != endTime ? log.ACTION_TIME >= startTime && dtEnd >= log.ACTION_TIME : log.ACTION_TIME.Value.Year == startTime.Year && log.ACTION_TIME.Value.Month == startTime.Month && log.ACTION_TIME.Value.Day == startTime.Day)
            //             //orderby f.ACTION_TIME descending
            //             select new VideoLogInfo
            //             {
            //                 Player = log.USER_NAME,
            //                 ActionTime = log.ACTION_TIME,
            //                 SubType = log.SUB_TYPE,
            //                 Ivoke_Type = log.INVOKE_TYPE,
            //                 MDVRId = log.MDVR_CORE_SN,
            //                 VehicleID = working.VEHICLE_ID,
            //                 ChannelId = log.CHANNEL,
            //                 VideoEndtime = log.END_TIME,
            //                 VideoStartTime = log.START_TIME,
            //                 VideoFileName = log.FILE_NAME,

            //             };

            ////result = result.Where(x => !string.IsNullOrWhiteSpace(x.MDVRId) && !string.IsNullOrWhiteSpace(x.VehicleID));
            //if (pageSize == 0 && pageIndex == -1)
            //{
            //    totalCount = result.Count();
            //    return result.ToList();
            //}
            //else
            //{
            //    totalCount = result.Count();
            //    return result.OrderBy(x => x.ActionTime).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            //}
            totalCount = 0;
            return null;
        }

        /// <summary>
        /// add access log
        /// </summary>
        /// <param name="VisitLog"></param>
        /// <returns></returns>
        public VisitLogInfo AddVisitLog(PTMSEntities context, VisitLogInfo VisitLog)
        {
            //LOG_ACCESS entity = new VISIT_LOG();
            //entity.ID = Guid.NewGuid().ToString();
            //entity.VISITOR = VisitLog.Visitor;
            //entity.VISIT_TIME = VisitLog.VisitTime;
            //entity.VISIT_CONTENT = VisitLog.VisiterContent;
            //entity.CONTENT_TYPE = (short)VisitLog.CONTENT_TYPE;
            //context.VISIT_LOG.Add(entity);
            context.SaveChanges();
            //VisitLog.Id = entity.ID;
            //return VisitLog;
            return null;
        }
        /// <summary>
        /// get access log
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="type"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<VisitLogInfo> GetVisitLog(PTMSEntities context, string visitor, int type, DateTime startTime, DateTime endTime, int pageSize, int pageIndex, out int totalCount)
        {
            DateTime dtEnd = endTime.AddDays(1);
            //var result = from f in context.VISIT_LOG
            //             where (string.IsNullOrEmpty(visitor) ? true : f.VISITOR.Contains(visitor))
            //             && (type == 0 ? true : f.CONTENT_TYPE == type)
            //             && (startTime != endTime ? f.VISIT_TIME >= startTime && dtEnd >= f.VISIT_TIME : f.VISIT_TIME.Value.Year == startTime.Year && f.VISIT_TIME.Value.Month == startTime.Month && f.VISIT_TIME.Value.Day == startTime.Day)
            //             orderby f.VISIT_TIME descending
            //             select new VisitLogInfo
            //             {
            //                 Id = f.ID,
            //                 Visitor = f.VISITOR,
            //                 VisitTime = f.VISIT_TIME.Value,
            //                 CONTENT_TYPE = f.CONTENT_TYPE,
            //                 TargetVihcle = f.VEHICLE_ID,
            //                 OrgCode = f.ECU911_CENTER,
            //                 VisiterContent = f.VISIT_CONTENT
            //             };
            //if (pageSize == 0 && pageIndex == -1)
            //{
            //    totalCount = result.Count();
            //    return result.ToList();
            //}
            //else
            //{
            //    totalCount = result.Count();
            //    return result.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            //}
            totalCount = 0;
            return null;
        }
        public Boolean ClearVisitLog(PTMSEntities context, DateTime startTime, DateTime endTime)
        {
            var suc = false;
            //var result = (from i in context.VISIT_LOG
            //              where i.VISIT_TIME > startTime && i.VISIT_TIME < endTime
            //              select i).FirstOrDefault();
            //if (result != null)
            //{
            //    context.VISIT_LOG.Remove(result);
            //    context.SaveChanges();
            //    suc = true;
            //}
            return suc;

        }

        public List<SuiteInfoLog> GetSuiteInfo(PTMSEntities context, string suiteID, DateTime startTime, DateTime endTime, int pageSize, int pageIndex, out int totalCount)
        {
            //var source = from change in context.SUITE_STATUS_CHANGE
            //             join suite in context.SECURITY_SUITE_INFO
            //                 on change.SUITE_INFO_ID equals suite.SUITE_INFO_ID
            //             where
            //                 change.OPERATING_TIME < endTime && change.OPERATING_TIME > startTime
            //                 && string.IsNullOrEmpty(suiteID) ? true : suite.SUITE_ID.Contains(suiteID)
            //             orderby suite.SUITE_ID descending
            //             select new
            //                    {
            //                        Suite_ID = suite.SUITE_ID,
            //                        CurrentStatus = change.OLD_STATUS,
            //                        ChangedStatus = change.NEW_STATUS,
            //                        OperatingTime = (DateTime)change.OPERATING_TIME,
            //                        Operator = change.OPERATING_PERSON,
            //                        OperatingReason = change.CHANGE_REASON,
            //                    };

            //if (pageIndex == -1 && pageSize == 0)
            //{
            //    totalCount = source.Count();
            //    totalCount = source.Count();
            //    var db = source.ToList();
            //    var result = from f in db
            //                 select new SuiteInfoLog
            //                 {
            //                     Suite_ID = f.Suite_ID,
            //                     CurrentStatus = Enum.GetName(typeof(SuiteStatus), f.CurrentStatus.Value),
            //                     ChangedStatus = Enum.GetName(typeof(SuiteStatus), f.ChangedStatus.Value),
            //                     OperatingTime = (DateTime)f.OperatingTime,
            //                     Operator = f.Operator,
            //                     OperatingReason = f.OperatingReason,
            //                 };
            //    return result.ToList();
            //}

            //else
            //{
            //    totalCount = source.Count();
            //    var db = source.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            //    var result = from f in db
            //                 select new SuiteInfoLog
            //                 {
            //                     Suite_ID = f.Suite_ID,
            //                     CurrentStatus = Enum.GetName(typeof(SuiteStatus), f.CurrentStatus.Value),
            //                     ChangedStatus = Enum.GetName(typeof(SuiteStatus), f.ChangedStatus.Value),
            //                     OperatingTime = (DateTime)f.OperatingTime,
            //                     Operator = f.Operator,
            //                     OperatingReason = f.OperatingReason,
            //                 };
            //    return result.ToList();
            //}
            totalCount = 0;
            return null;
        }
    }
}
