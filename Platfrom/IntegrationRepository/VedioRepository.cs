using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Data.Video;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Integration.Contract;
using Gsafety.PTMS.Integration.Contract.Data;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7dd7c366-94d8-4ca9-8c8b-cb2ddbd86562      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Repository
/////    Project Description:    
/////             Class Name: VedioRepository
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 17:22:59
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 17:22:59
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

//using Gsafety.PTMS.Installation.Repository;

namespace Gsafety.PTMS.Integration.Repository
{
    public class VedioRepository
    {
        public MultiMessage<QueryServerFileListMessage> QueryServerFileList(QueryServerFileListArgs arg, string userName)
        {
            //改用新方法
            var result = new MultiMessage<QueryServerFileListMessage> { IsSuccess = true };

            using (var entity = new PTMSEntities())
            {
                string vechilesn = string.Empty;
                vechilesn = this.GetVehicleSnByMdvr(entity, arg.MdvrCoreSn);
                //var cameraLocations = new InstallationRepository().GetCameraInfoBySuiteID(entity, arg.MdvrCoreSn);

                if (arg.PageNum > 0)
                {
                    var alarmids = from A in entity.MDI_ALARM_VIDEO
                                   where A.MDVR_CORE_SN == arg.MdvrCoreSn
                                   && A.START_TIME >= arg.Start_Time && A.START_TIME <= arg.End_Time
                                   && A.START_TIME < A.END_TIME
                                   select new SearchResult
                                   {
                                       Uuid = A.UUID,
                                       CreateTime = A.START_TIME,
                                   };
                    var aliveids = from B in entity.MDI_LIVE_VIDEO
                                   where B.MDVR_CORE_SN == arg.MdvrCoreSn
                                   && B.START_TIME >= arg.Start_Time && B.START_TIME <= arg.End_Time
                                   && B.START_TIME < B.END_TIME
                                   && B.IS_FINISH == 1
                                   select new SearchResult
                                   {
                                       Uuid = B.UUID,
                                       CreateTime = B.START_TIME,
                                   };
                    var downloadids = from D in entity.MDI_DOWNLOAD_VIDEO
                                      where D.MDVR_CORE_SN == arg.MdvrCoreSn
                                      && D.START_TIME >= arg.Start_Time && D.START_TIME <= arg.End_Time
                                      && D.START_TIME < D.END_TIME
                                      select new SearchResult
                                      {
                                          Uuid = D.UUID,
                                          CreateTime = D.START_TIME,
                                      };
                    var test = alarmids.Concat(aliveids).Concat(downloadids);

                    result.TotalRecord = test.Count();
                    var uuids = test.OrderBy(v => v.CreateTime)
                              .Skip(arg.PageSize * (arg.PageNum - 1))
                              .Take(arg.PageSize)
                              .Select(n => n.Uuid)
                              .ToList();

                    var tempalarm = from A in entity.MDI_ALARM_VIDEO
                                    where A.MDVR_CORE_SN == arg.MdvrCoreSn && A.START_TIME >= arg.Start_Time && A.START_TIME <= arg.End_Time
                                    && uuids.Any(c => c == A.UUID)
                                    select new QueryServerFileListMessage
                                    {
                                        Channel = A.CHANNEL_ID,
                                        StartTime = A.START_TIME == null ? DateTime.MinValue : (DateTime)A.START_TIME,
                                        EndTime = A.END_TIME == null ? DateTime.MinValue : (DateTime)A.END_TIME,
                                        FileID = A.VIDEO_URL,
                                        FileSize = A.VIDEO_SIZE,
                                        UUID = A.UUID,
                                        VehicleSN = vechilesn,
                                        VideoType = (int)VideoTypeEnum.AlarmVideo,
                                        DownloadStatus = (int)DownloadStatus.DownloadFinished,
                                        Note = A.NOTE
                                    };
                    var tempalive = from B in entity.MDI_LIVE_VIDEO
                                    where B.MDVR_CORE_SN == arg.MdvrCoreSn && B.START_TIME != null && B.START_TIME >= arg.Start_Time && B.START_TIME <= arg.End_Time && B.IS_FINISH == 1
                                     && uuids.Any(c => c == B.UUID)
                                    select new QueryServerFileListMessage
                                    {
                                        Channel = B.CHANNEL_ID,
                                        StartTime = B.START_TIME == null ? DateTime.MinValue : (DateTime)B.START_TIME,
                                        EndTime = B.END_TIME == null ? DateTime.MinValue : (DateTime)B.END_TIME,
                                        FileID = B.VIDEO_URL,
                                        FileSize = B.VIDEO_SIZE,
                                        UUID = B.UUID,
                                        VehicleSN = vechilesn,
                                        VideoType = (int)VideoTypeEnum.CommonVideo,
                                        DownloadStatus = (int)DownloadStatus.DownloadFinished,
                                        Note = B.NOTE
                                    };
                    var tempdown = from D in entity.MDI_DOWNLOAD_VIDEO
                                   where D.MDVR_CORE_SN == arg.MdvrCoreSn && D.START_TIME != null && D.START_TIME >= arg.Start_Time && D.START_TIME <= arg.End_Time && uuids.Any(c => c == D.UUID)
                                   select new QueryServerFileListMessage
                                   {
                                       Channel = D.CHANNEL_ID.HasValue ? D.CHANNEL_ID.Value : 0,
                                       StartTime = D.START_TIME == null ? DateTime.MinValue : (DateTime)D.START_TIME,
                                       EndTime = D.END_TIME == null ? DateTime.MinValue : (DateTime)D.END_TIME,
                                       FileID = D.VIDEO_URL,
                                       FileSize = D.SOURCE_SIZE.Value,
                                       UUID = D.UUID,
                                       VehicleSN = vechilesn,
                                       VideoType = (int)VideoTypeEnum.CommonVideo,
                                       Note = D.NOTE,
                                       DownloadStatus = (int)D.DOWNLOAD_STATUS
                                   };

                    result.Result = tempalarm.Concat(tempalive).Concat(tempdown).ToList();
                }
                else
                {
                    var tempalarm = from A in entity.MDI_ALARM_VIDEO
                                    where A.MDVR_CORE_SN == arg.MdvrCoreSn && A.START_TIME >= arg.Start_Time && A.START_TIME <= arg.End_Time
                                    && A.START_TIME < A.END_TIME
                                    select new QueryServerFileListMessage
                                    {
                                        Channel = A.CHANNEL_ID,
                                        StartTime = A.START_TIME == null ? DateTime.MinValue : (DateTime)A.START_TIME,
                                        EndTime = A.END_TIME == null ? DateTime.MinValue : (DateTime)A.END_TIME,
                                        FileID = A.VIDEO_URL,
                                        FileSize = A.VIDEO_SIZE,
                                        UUID = A.UUID,
                                        VehicleSN = vechilesn,
                                        VideoType = (int)VideoTypeEnum.AlarmVideo,
                                        DownloadStatus = (int)DownloadStatus.DownloadFinished,
                                        Note = A.NOTE
                                    };

                    //select from alive video
                    var tempalive = from B in entity.MDI_LIVE_VIDEO
                                    where B.MDVR_CORE_SN == arg.MdvrCoreSn && B.START_TIME >= arg.Start_Time && B.START_TIME <= arg.End_Time
                                    && B.START_TIME < B.END_TIME
                                    && B.IS_FINISH == 1
                                    select new QueryServerFileListMessage
                                    {
                                        Channel = B.CHANNEL_ID,
                                        StartTime = B.START_TIME == null ? DateTime.MinValue : (DateTime)B.START_TIME,
                                        EndTime = B.END_TIME == null ? DateTime.MinValue : (DateTime)B.END_TIME,
                                        FileID = B.VIDEO_URL,
                                        FileSize = B.VIDEO_SIZE,
                                        UUID = B.UUID,
                                        VehicleSN = vechilesn,
                                        VideoType = (int)VideoTypeEnum.CommonVideo,
                                        DownloadStatus = (int)DownloadStatus.DownloadFinished,
                                        Note = B.NOTE
                                    };
                    var tempdown = from D in entity.MDI_DOWNLOAD_VIDEO
                                   where D.MDVR_CORE_SN == arg.MdvrCoreSn && D.START_TIME != null && D.START_TIME >= arg.Start_Time && D.START_TIME <= arg.End_Time
                                    && D.START_TIME < D.END_TIME
                                   select new QueryServerFileListMessage
                                   {
                                       Channel = D.CHANNEL_ID.HasValue ? D.CHANNEL_ID.Value : 0,
                                       StartTime = D.START_TIME == null ? DateTime.MinValue : (DateTime)D.START_TIME,
                                       EndTime = D.END_TIME == null ? DateTime.MinValue : (DateTime)D.END_TIME,
                                       FileID = D.VIDEO_URL,
                                       FileSize = D.SOURCE_SIZE.Value,
                                       UUID = D.UUID,
                                       VehicleSN = vechilesn,
                                       VideoType = (int)VideoTypeEnum.CommonVideo,
                                       DownloadStatus = (int)D.DOWNLOAD_STATUS,
                                       Note = D.NOTE
                                   };
                    result.Result = tempalarm.Concat(tempalive).Concat(tempdown).ToList();
                    result.TotalRecord = result.Result.Count;
                }

                return result;
            }

        }

        public MultiMessage<QueryServerFileListMessage> QueryHistoryVideoList(string vehicleid, DateTime startTime, DateTime endTime, int pageSize, int pageValue)
        {
            //改用新方法
            var result = new MultiMessage<QueryServerFileListMessage> { IsSuccess = true };

            using (var entity = new PTMSEntities())
            {
                var working = entity.RUN_SUITE_WORKING.FirstOrDefault(n => n.VEHICLE_ID == vehicleid);
                if (working != null)
                {
                    string mdvrsn = working.MDVR_CORE_SN;

                    var vehicle = entity.BSC_VEHICLE.FirstOrDefault(n => n.VEHICLE_ID == vehicleid && n.VALID == 1);
                    string vehiclesn = vehicle.VEHICLE_SN;
                    if (vehicle != null)
                    {
                        if (pageSize > 0)
                        {
                            var alarmids = from A in entity.MDI_ALARM_VIDEO
                                           where A.MDVR_CORE_SN == mdvrsn
                                           && A.START_TIME >= startTime && A.START_TIME <= endTime
                                           && A.START_TIME < A.END_TIME
                                           select new SearchResult
                                           {
                                               Uuid = A.UUID,
                                               CreateTime = A.START_TIME,
                                           };
                            var aliveids = from B in entity.MDI_LIVE_VIDEO
                                           where B.MDVR_CORE_SN == mdvrsn
                                           && B.START_TIME >= startTime && B.START_TIME <= endTime
                                           && B.START_TIME < B.END_TIME
                                           && B.IS_FINISH == 1
                                           select new SearchResult
                                           {
                                               Uuid = B.UUID,
                                               CreateTime = B.START_TIME,
                                           };
                            var downloadids = from D in entity.MDI_DOWNLOAD_VIDEO
                                              where D.MDVR_CORE_SN == mdvrsn
                                              && D.START_TIME >= startTime && D.START_TIME <= endTime
                                              && D.START_TIME < D.END_TIME
                                              select new SearchResult
                                              {
                                                  Uuid = D.UUID,
                                                  CreateTime = D.START_TIME,
                                              };
                            var test = alarmids.Concat(aliveids).Concat(downloadids);

                            result.TotalRecord = test.Count();
                            var uuids = test.OrderBy(v => v.CreateTime)
                                      .Skip(pageSize * (pageValue - 1))
                                      .Take(pageSize)
                                      .Select(n => n.Uuid)
                                      .ToList();

                            var tempalarm = from A in entity.MDI_ALARM_VIDEO
                                            where A.MDVR_CORE_SN == mdvrsn && A.START_TIME >= startTime && A.START_TIME <= endTime
                                            && uuids.Any(c => c == A.UUID)
                                            select new QueryServerFileListMessage
                                            {
                                                Channel = A.CHANNEL_ID,
                                                StartTime = A.START_TIME == null ? DateTime.MinValue : (DateTime)A.START_TIME,
                                                EndTime = A.END_TIME == null ? DateTime.MinValue : (DateTime)A.END_TIME,
                                                FileID = A.VIDEO_URL,
                                                FileSize = A.VIDEO_SIZE,
                                                UUID = A.UUID,
                                                VehicleSN = vehiclesn,
                                                VideoType = (int)VideoTypeEnum.AlarmVideo,
                                                DownloadStatus = (int)DownloadStatus.DownloadFinished,
                                                Note = A.NOTE
                                            };
                            var tempalive = from B in entity.MDI_LIVE_VIDEO
                                            where B.MDVR_CORE_SN == mdvrsn && B.START_TIME != null && B.START_TIME >= startTime && B.START_TIME <= endTime && B.IS_FINISH == 1
                                             && uuids.Any(c => c == B.UUID)
                                            select new QueryServerFileListMessage
                                            {
                                                Channel = B.CHANNEL_ID,
                                                StartTime = B.START_TIME == null ? DateTime.MinValue : (DateTime)B.START_TIME,
                                                EndTime = B.END_TIME == null ? DateTime.MinValue : (DateTime)B.END_TIME,
                                                FileID = B.VIDEO_URL,
                                                FileSize = B.VIDEO_SIZE,
                                                UUID = B.UUID,
                                                VehicleSN = vehiclesn,
                                                VideoType = (int)VideoTypeEnum.CommonVideo,
                                                DownloadStatus = (int)DownloadStatus.DownloadFinished,
                                                Note = B.NOTE
                                            };
                            var tempdown = from D in entity.MDI_DOWNLOAD_VIDEO
                                           where D.MDVR_CORE_SN == mdvrsn && D.START_TIME != null && D.START_TIME >= startTime && D.START_TIME <= endTime && uuids.Any(c => c == D.UUID)
                                           select new QueryServerFileListMessage
                                           {
                                               Channel = D.CHANNEL_ID.HasValue ? D.CHANNEL_ID.Value : 0,
                                               StartTime = D.START_TIME == null ? DateTime.MinValue : (DateTime)D.START_TIME,
                                               EndTime = D.END_TIME == null ? DateTime.MinValue : (DateTime)D.END_TIME,
                                               FileID = D.VIDEO_URL,
                                               FileSize = D.SOURCE_SIZE.Value,
                                               UUID = D.UUID,
                                               VehicleSN = vehiclesn,
                                               VideoType = (int)VideoTypeEnum.CommonVideo,
                                               DownloadStatus = (int)D.DOWNLOAD_STATUS,
                                               Note = D.NOTE
                                           };

                            result.Result = tempalarm.Concat(tempalive).Concat(tempdown).ToList();
                        }
                        else
                        {
                            var tempalarm = from A in entity.MDI_ALARM_VIDEO
                                            where A.MDVR_CORE_SN == mdvrsn && A.START_TIME >= startTime && A.START_TIME <= endTime
                                            && A.START_TIME < A.END_TIME
                                            select new QueryServerFileListMessage
                                            {
                                                Channel = A.CHANNEL_ID,
                                                StartTime = A.START_TIME == null ? DateTime.MinValue : (DateTime)A.START_TIME,
                                                EndTime = A.END_TIME == null ? DateTime.MinValue : (DateTime)A.END_TIME,
                                                FileID = A.VIDEO_URL,
                                                FileSize = A.VIDEO_SIZE,
                                                UUID = A.UUID,
                                                VehicleSN = vehiclesn,
                                                VideoType = (int)VideoTypeEnum.AlarmVideo,
                                                DownloadStatus = (int)DownloadStatus.DownloadFinished,
                                                Note = A.NOTE
                                            };

                            //select from alive video
                            var tempalive = from B in entity.MDI_LIVE_VIDEO
                                            where B.MDVR_CORE_SN == mdvrsn && B.START_TIME >= startTime && B.START_TIME <= endTime
                                            && B.START_TIME < B.END_TIME
                                            && B.IS_FINISH == 1
                                            select new QueryServerFileListMessage
                                            {
                                                Channel = B.CHANNEL_ID,
                                                StartTime = B.START_TIME == null ? DateTime.MinValue : (DateTime)B.START_TIME,
                                                EndTime = B.END_TIME == null ? DateTime.MinValue : (DateTime)B.END_TIME,
                                                FileID = B.VIDEO_URL,
                                                FileSize = B.VIDEO_SIZE,
                                                UUID = B.UUID,
                                                VehicleSN = vehiclesn,
                                                VideoType = (int)VideoTypeEnum.CommonVideo,
                                                DownloadStatus = (int)DownloadStatus.DownloadFinished,
                                                Note = B.NOTE
                                            };
                            var tempdown = from D in entity.MDI_DOWNLOAD_VIDEO
                                           where D.MDVR_CORE_SN == mdvrsn && D.START_TIME != null && D.START_TIME >= startTime && D.START_TIME <= endTime
                                            && D.START_TIME < D.END_TIME
                                           select new QueryServerFileListMessage
                                           {
                                               Channel = D.CHANNEL_ID.HasValue ? D.CHANNEL_ID.Value : 0,
                                               StartTime = D.START_TIME == null ? DateTime.MinValue : (DateTime)D.START_TIME,
                                               EndTime = D.END_TIME == null ? DateTime.MinValue : (DateTime)D.END_TIME,
                                               FileID = D.VIDEO_URL,
                                               FileSize = D.SOURCE_SIZE.Value,
                                               UUID = D.UUID,
                                               VehicleSN = vehiclesn,
                                               VideoType = (int)VideoTypeEnum.CommonVideo,
                                               DownloadStatus = (int)D.DOWNLOAD_STATUS,
                                               Note = D.NOTE
                                           };
                            result.Result = tempalarm.Concat(tempalive).Concat(tempdown).ToList();
                            result.TotalRecord = result.Result.Count;
                        }
                    }
                }

                return result;
            }

        }


        public MultiMessage<QueryDownloadStatusMessage> QueryDownloadStatus(QueryDownloadStatusArgs arg, string userName)
        {
            var result = new MultiMessage<QueryDownloadStatusMessage> { IsSuccess = true };
            List<QueryDownloadStatusMessage> videoList = new List<QueryDownloadStatusMessage>();

            using (var entity = new PTMSEntities())
            {
                var temp = from D in entity.MDI_DOWNLOAD_VIDEO
                           where arg.FileIDs.Any(n => n == D.UUID)
                           select new QueryDownloadStatusMessage
                           {
                               Status = D.DOWNLOAD_STATUS.HasValue ? D.DOWNLOAD_STATUS.Value : 0,
                               FileID = D.UUID,
                               Url = D.VIDEO_URL,
                               Percent = D.SOURCE_DOWNLOAD_SIZE.HasValue ? D.SOURCE_DOWNLOAD_SIZE.Value : 0,
                           };

                result.Result = temp.ToList();
            }

            return result;
        }

        public SingleMessage<Dictionary<int, bool>> CheckAlarmVideo(PTMSEntities context, CheckAlarmVideoArgs arg, string userName)
        {
            var detail = context.MTN_INSTALLATION_DETAIL.FirstOrDefault(n => n.ID == arg.Alarm_Id);
            if (detail == null)
            {
                return new SingleMessage<Dictionary<int, bool>>(false, "MTN_INSTALLATION_DETAILNotExist");
            }

            var videos = context.MDI_ALARM_VIDEO.Where(n => n.MDVR_CORE_SN == detail.MDVR_CORE_SN && n.START_TIME >= detail.CREATE_TIME && n.START_TIME >= arg.Date)
                    .Select(t => t.CHANNEL_ID).ToList();

            var cameras = context.MTN_PART_AUDIT.Where(t => t.INSTALL_AUDIT_ID == detail.ID).ToList();

            var resultDic = new Dictionary<int, bool>();
            var isSuccess = true;

            foreach (var item in cameras)
            {
                if (videos.Contains(item.CHANNEL_ID))
                {
                    item.RESULT = 1;
                    resultDic[item.CHANNEL_ID] = true;
                }
                else
                {
                    item.RESULT = 0;
                    isSuccess = false;
                    resultDic[item.CHANNEL_ID] = false;
                }
            }

            if (isSuccess)
            {
                var mainAudit = context.MTN_INSTALLATION_AUDIT.FirstOrDefault(n => n.INSTALL_ID == detail.ID);
                mainAudit.VIDEO_CHECK = 1;
                if (mainAudit.VIDEO_CHECK == 1 && mainAudit.GPS_CHECK == 1 && mainAudit.ALARM_CHECK == 1)
                {
                    mainAudit.AUDIT_FLAG = 1;
                }

                context.SaveChanges();
            }

            return new SingleMessage<Dictionary<int, bool>>(resultDic);
        }

        public string GetVehicleSnByMdvr(PTMSEntities _context, string mdvrCoreSn)
        {
            var result = from s in _context.RUN_SUITE_WORKING
                         where s.MDVR_CORE_SN == mdvrCoreSn
                         select s.VEHICLE_ID;

            string vechilesn = result.FirstOrDefault();

            return vechilesn;

        }

        public MultiMessage<HistoryItemForVideoAppeal> GetHistoryItemForVideoAppeal(PTMSEntities _context, string starttime, string endtime, int timethreshold)
        {
            MultiMessage<HistoryItemForVideoAppeal> result = new MultiMessage<HistoryItemForVideoAppeal>();
            List<HistoryItemForVideoAppeal> hivas = new List<HistoryItemForVideoAppeal>();
            DateTime begin = DateTime.Parse(starttime);
            DateTime end = DateTime.Parse(endtime);

            var alarms = from a in _context.ALM_ALARM_RECORD
                         where a.CREATE_TIME >= begin && a.CREATE_TIME < end
                         select a;

            List<ALM_ALARM_RECORD> lsAlarms = alarms.ToList();
            List<string> vehiclesn = lsAlarms.Select(n => n.VEHICLE_ID).Distinct().ToList();
            var temp = from v in _context.BSC_VEHICLE
                       from a in alarms
                       where v.VEHICLE_ID == a.VEHICLE_ID
                       select v;
            List<BSC_VEHICLE> lsvehicles = temp.ToList();
            var ls911s = from d in _context.ALM_911_DISPOSE
                         where alarms.Any(n => n.ID == d.ALARM_ID)
                         select d;

            List<ALM_911_DISPOSE> ls911 = ls911s.ToList();
            List<QueryServerFileListMessage> files = new List<QueryServerFileListMessage>();

            List<string> mdvrs = lsAlarms.Select(n => n.VEHICLE_ID).Distinct().ToList();

            using (var entity = new PTMSEntities())
            {
                var tempalarm = from A in entity.MDI_ALARM_VIDEO
                                where mdvrs.Any(n => n == A.MDVR_CORE_SN)
                                && A.START_TIME >= begin && A.START_TIME <= end
                                select new QueryServerFileListMessage
                                {
                                    Channel = A.CHANNEL_ID,
                                    StartTime = A.START_TIME == null ? DateTime.MinValue : (DateTime)A.START_TIME,
                                    EndTime = A.END_TIME == null ? DateTime.MinValue : (DateTime)A.END_TIME,
                                    FileID = A.VIDEO_URL,
                                    VehicleSN = A.MDVR_CORE_SN,
                                    FileSize = 0,

                                };

                files.AddRange(tempalarm);


                var tempalive = from B in entity.MDI_LIVE_VIDEO
                                where mdvrs.Any(n => n == B.MDVR_CORE_SN)
                                && B.START_TIME >= begin && B.START_TIME <= end
                                select new QueryServerFileListMessage
                                       {
                                           Channel = B.CHANNEL_ID,
                                           StartTime =
                                               B.START_TIME == null ? DateTime.MinValue : (DateTime)B.START_TIME,
                                           EndTime =
                                               B.END_TIME == null ? DateTime.MinValue : (DateTime)B.END_TIME,
                                           FileID = B.VIDEO_URL,
                                           VehicleSN = B.MDVR_CORE_SN,
                                           FileSize = 1,
                                       };
                files.AddRange(tempalive);

            }

            foreach (ALM_ALARM_RECORD record in lsAlarms)
            {
                DateTime recordstart = record.CREATE_TIME.Value.AddMinutes(-timethreshold);
                DateTime recordend = record.CREATE_TIME.Value.AddMinutes(timethreshold);
                List<QueryServerFileListMessage> recordfiles = files.Where(n => n.StartTime >= recordstart && n.StartTime < recordend && n.VehicleSN == record.VEHICLE_ID).ToList();

                foreach (QueryServerFileListMessage recordfile in recordfiles)
                {
                    HistoryItemForVideoAppeal item = new HistoryItemForVideoAppeal();
                    item.AppealTime = record.CREATE_TIME == null ? DateTime.MinValue : (DateTime)record.CREATE_TIME;
                    item.EndTime = recordfile.EndTime;
                    //字符串截取

                    item.FilePath = recordfile.FileID;



                    item.MDVR = record.VEHICLE_ID;
                    item.StartTime = recordfile.StartTime;
                    item.Channel = (int)recordfile.Channel;
                    if (!string.IsNullOrEmpty(record.LATITUDE))
                    {
                        item.Latitude = Convert.ToDecimal(record.LATITUDE);
                    }

                    if (!string.IsNullOrEmpty(record.LONGITUDE))
                    {
                        item.Longitude = Convert.ToDecimal(record.LONGITUDE);
                    }

                    ALM_911_DISPOSE ecu911 = ls911.FirstOrDefault(n => n.ALARM_ID == record.ID);
                    if (ecu911 != null)
                    {
                        item.Dealer = ecu911.APPREAL_STAFF;
                        item.IsTrueAlarm = ecu911.ALARM_FLAG == 1 ? "true" : "false";
                    }

                    BSC_VEHICLE vehicle = lsvehicles.FirstOrDefault(n => n.VEHICLE_SN == record.VEHICLE_ID);

                    if (vehicle != null)
                    {
                        item.Device = vehicle.VEHICLE_SN;
                        //item.Mobile = vehicle.OWNER_PHONE;
                        item.Reportor = vehicle.OWNER;
                    }
                    item.VideoType = recordfile.FileSize == 0 ? "15S" : "AliveVideo";
                    hivas.Add(item);

                }
            }


            result.Result = hivas;

            return result;
        }

        public MultiMessage<HistoryItemForVideoAppeal> GetHistoryVideoAppeal(PTMSEntities _context, string starttime, string endtime)
        {
            MultiMessage<HistoryItemForVideoAppeal> result = new MultiMessage<HistoryItemForVideoAppeal>();
            List<HistoryItemForVideoAppeal> hivas = new List<HistoryItemForVideoAppeal>();
            DateTime begin = DateTime.Parse(starttime);
            DateTime end = DateTime.Parse(endtime).AddHours(23).AddMinutes(59).AddSeconds(59);


            var alarm = from a in _context.ALM_ALARM_RECORD
                        where
                            a.CREATE_TIME > begin && a.CREATE_TIME < end
                        select a;
            using (var bigDb = new PTMSEntities())
            {
                foreach (var record in alarm)
                {
                    DateTime startTime = record.GPS_TIME.Value.AddSeconds(-30);
                    DateTime endTime = record.GPS_TIME.Value.AddSeconds(30);
                    string MdvrID = record.VEHICLE_ID;
                    string Uid = record.ALARM_UID;
                    var User = from user in _context.BSC_VEHICLE
                               where user.VEHICLE_ID == record.VEHICLE_ID
                               select user;
                    string owner = User.FirstOrDefault().OWNER;
                    string Mobile = User.FirstOrDefault().CONTACT_PHONE;
                    var AlarmVideo = from alarmVideo in bigDb.MDI_ALARM_VIDEO
                                     where alarmVideo.ALARM_ID == Uid &&
                                           alarmVideo.MDVR_CORE_SN == MdvrID &&
                                           alarmVideo.START_TIME > startTime &&
                                           alarmVideo.START_TIME < endTime
                                     select new HistoryItemForVideoAppeal
                                            {
                                                MDVR = MdvrID,
                                                AppealTime = record.CREATE_TIME.Value,
                                                Channel = 0,
                                                StartTime = alarmVideo.START_TIME,
                                                EndTime = alarmVideo.END_TIME.Value,
                                                FilePath = alarmVideo.VIDEO_URL,
                                                VideoType = "15S",
                                                Device = record.VEHICLE_ID,
                                                Reportor = owner,
                                                Mobile = Mobile
                                            };
                    if (AlarmVideo.FirstOrDefault() != null)
                    {
                        hivas.Add(AlarmVideo.FirstOrDefault());
                    }
                }
                var AliveVideo = from b in bigDb.MDI_LIVE_VIDEO
                                 where b.START_TIME > begin && b.END_TIME < end
                                 select b;
                foreach (var record in AliveVideo)
                {
                    string MdvrID = record.MDVR_CORE_SN;

                    var ll = from mdvrWork in _context.RUN_SUITE_WORKING
                             from Vehicle in _context.BSC_VEHICLE
                             where mdvrWork.VEHICLE_ID == MdvrID && Vehicle.VEHICLE_ID == mdvrWork.VEHICLE_ID
                             select new HistoryItemForVideoAppeal
                                    {
                                        MDVR = MdvrID,
                                        Channel = (int)record.CHANNEL_ID,
                                        EndTime = record.END_TIME,

                                        StartTime = record.START_TIME,
                                        FilePath = record.VIDEO_URL,
                                        VideoType = "AliveVideo",
                                        Device = mdvrWork.VEHICLE_ID,
                                        Reportor = Vehicle.OWNER,
                                        Mobile = Vehicle.CONTACT_PHONE

                                    };
                    if (ll.FirstOrDefault() != null)
                    {
                        hivas.Add(ll.FirstOrDefault());
                    }
                }
            }

            result.Result = hivas;

            return result;
        }

        public MultiMessage<QueryServerFileListMessage> GetAlarmFiftyVideoAppeal(PTMSEntities context, string alarmId)
        {
            var alarm = (from a in context.ALM_ALARM_RECORD
                         where a.ID == alarmId
                         select a).FirstOrDefault();

            if (alarm != null)
            {
                DateTime starTime = alarm.GPS_TIME.Value.AddMinutes(-2);
                DateTime endTime = alarm.GPS_TIME.Value.AddMinutes(2);
                var files = (from f in context.MDI_ALARM_VIDEO
                             where alarm.MDVR_CORE_SN == f.MDVR_CORE_SN && f.START_TIME > starTime && f.START_TIME < endTime
                             select f).ToList();

                var result = files.GroupBy(t => t.CHANNEL_ID)
                    .Select(g => g.OrderBy(t => Math.Abs(alarm.GPS_TIME.Value.Subtract(t.START_TIME).TotalSeconds)).FirstOrDefault()).ToList();

                var tempalarm = (from A in result
                                 select new QueryServerFileListMessage
                                 {
                                     Channel = A.CHANNEL_ID,
                                     StartTime = A.START_TIME == null ? DateTime.MinValue : (DateTime)A.START_TIME,
                                     EndTime = A.END_TIME == null ? DateTime.MinValue : (DateTime)A.END_TIME,
                                     FileID = A.VIDEO_URL,
                                     FileSize = A.VIDEO_SIZE,
                                     UUID = A.UUID,
                                     VideoType = (int)VideoTypeEnum.AlarmVideo,
                                     DownloadStatus = (int)DownloadStatus.DownloadFinished
                                 }).ToList();

                return new MultiMessage<QueryServerFileListMessage>(tempalarm, tempalarm.Count);
            }

            return new MultiMessage<QueryServerFileListMessage>(new List<QueryServerFileListMessage>(), 0);
        }

        public SingleMessage<bool> UpdateVideoNote(PTMSEntities context, string videoID, string note)
        {
            var downloadVideo = context.MDI_DOWNLOAD_VIDEO.FirstOrDefault(t => t.UUID == videoID);
            if (downloadVideo != null)
            {
                downloadVideo.NOTE = note;
                context.Entry(downloadVideo).State = EntityState.Modified;

                return context.Save();
            }

            var alarmVideo = context.MDI_ALARM_VIDEO.FirstOrDefault(t => t.UUID == videoID);
            if (alarmVideo != null)
            {
                alarmVideo.NOTE = note;
                context.Entry(alarmVideo).State = EntityState.Modified;

                return context.Save();
            }


            var liveVideo = context.MDI_LIVE_VIDEO.FirstOrDefault(t => t.UUID == videoID);
            if (liveVideo != null)
            {
                liveVideo.NOTE = note;
                context.Entry(liveVideo).State = EntityState.Modified;

                return context.Save();
            }

            return new SingleMessage<bool>(false, "NotExist");
        }

        public MultiMessage<Photo> GetPhotoList(PTMSEntities context, QueryPhotoFileListArgs arg)
        {
            List<Photo> list = new List<Photo>();
            List<MDI_PHOTOGRAPH> tempList = new List<MDI_PHOTOGRAPH>();
            Expression<Func<MDI_PHOTOGRAPH, bool>> filter =
                photo => photo.DEVICE_SN == arg.MdvrCoreSn && photo.DEVICE_TYPE == arg.DeviceType
                    && photo.CREATE_TIME >= arg.Start_Time && photo.CREATE_TIME <= arg.End_Time;
            if (arg.Status != PhotoStatusEnum.All)
            {
                filter = filter.And(t => t.STATUS == (short)arg.Status);
            }

            if (arg.ChannelID != 99)
            {
                filter = filter.And(t => t.CHANNEL_ID == arg.ChannelID);
            }

            if (!string.IsNullOrEmpty(arg.Note))
            {
                filter = filter.And(t => t.NOTE.Contains(arg.Note));
            }

            int totalCount;
            if (arg.OrderBy == 0)
            {
                tempList = context.MDI_PHOTOGRAPH.Where(filter).Page(out totalCount, arg.PageNum, arg.PageSize, true, t => t.CREATE_TIME, false).ToList();
            }
            else
            {
                tempList = context.MDI_PHOTOGRAPH.Where(filter).Page(out totalCount, arg.PageNum, arg.PageSize, true, t => t.CHANNEL_ID, true).ToList();
            }

            foreach (var item in tempList)
            {
                Photo photo = new Photo();
                photo.ID = item.ID;
                photo.ClientID = item.CLIENT_ID;
                photo.DeviceSn = item.DEVICE_SN;
                photo.DeviceType = item.DEVICE_TYPE;
                photo.ChannelID = (int)item.CHANNEL_ID;
                photo.MiniImg_Url = item.MINIIMG_URL;
                photo.Img_Url = item.IMG_URL;
                photo.Img_Size = (decimal)item.IMGSIZE;
                photo.Create_Time = (DateTime)item.CREATE_TIME;
                photo.Longitude = item.LONGITUDE;
                photo.Latitude = item.LATITUDE;
                if ((int)item.STATUS == 0)
                {
                    photo.Status = false;
                }
                else
                {
                    photo.Status = true;
                }

                photo.IsChecked = false;
                photo.Note = item.NOTE;
                list.Add(photo);
            }

            return new MultiMessage<Photo>(list, totalCount);
        }

        public SingleMessage<bool> SetPhotoMark(PTMSEntities context, List<string> list, int status, string note)
        {
            try
            {
                if (list.Count == 0)
                {
                    return new SingleMessage<bool>(true);
                }
                string photoId = null;
                for (int i = 0; i < list.Count; i++)
                {
                    photoId = list[i];
                    var photo = context.MDI_PHOTOGRAPH.FirstOrDefault(t => t.ID == photoId);
                    photo.STATUS = (short)status;
                    photo.NOTE = note;
                    context.Entry(photo).State = EntityState.Modified;
                }
                return context.Save();
            }
            catch (Exception)
            {
                return new SingleMessage<bool>(false);
            }
        }

        public SingleMessage<bool> DeletePhoto(PTMSEntities context, List<string> list)
        {
            try
            {
                string photoId = null;
                for (int i = 0; i < list.Count; i++)
                {
                    photoId = list[i];
                    var photo = context.MDI_PHOTOGRAPH.FirstOrDefault(t => t.ID == photoId);
                    if (photo != null)
                    {
                        context.MDI_PHOTOGRAPH.Remove(photo);
                        context.SaveChanges();
                    }
                }

                return new SingleMessage<bool>(true);
            }
            catch (Exception)
            {
                return new SingleMessage<bool>(false);
            }
        }
    }
}
