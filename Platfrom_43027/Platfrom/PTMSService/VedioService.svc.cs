using Gsafety.PTMS.Alarm.Contract;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Integration.Repository;
using Gsafety.Common.Logging;
using Gsafety.PTMS.Base.Contract.Data;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2ffc0b54-982b-45c8-a603-a8fc9186c5b7      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Service
/////    Project Description:    
/////             Class Name: VedioService
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 19:06:13
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 19:06:13
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gs.PTMS.Common.Data.Enum;
using Gsafety.PTMS.Integration.Contract;
using Gsafety.PTMS.Integration.Contract.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Data.Video;
using Gsafety.PTMS.Common.Data.Enum;

namespace Gs.PTMS.Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“VedioService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 VedioService.svc 或 VedioService.svc.cs，然后开始调试。
    public class VedioService : BaseService, IVedioService
    {
        private VedioRepository _helper;

        public VedioService()
            : base()
        {
            _helper = new VedioRepository();
        }

        public MultiMessage<QueryServerFileListMessage> QueryServerFileList(QueryServerFileListArgs arg)
        {
            try
            {
                Info("QueryServerFileList");
                Info("arg:" + Convert.ToString(arg));
                var userInfo = this.GetUserInfo();
                MultiMessage<QueryServerFileListMessage> result = _helper.QueryServerFileList(arg, userInfo.UserName);
                Log<QueryServerFileListMessage>(result);
                return result;
            }
            catch (Exception exp)
            {
                Error(exp);
                return new MultiMessage<QueryServerFileListMessage> { ErrorMsg = exp.Message, ErrorDetailMsg = exp.StackTrace, IsSuccess = false, };
            }
        }

        public MultiMessage<QueryDownloadStatusMessage> QueryDownloadStatus(QueryDownloadStatusArgs arg)
        {
            try
            {
                Info("QueryDownloadStatus");
                Info("arg:" + Convert.ToString(arg));
                var userInfo = this.GetUserInfo();

                MultiMessage<QueryDownloadStatusMessage> result = _helper.QueryDownloadStatus(arg, userInfo.UserName);
                Log<QueryDownloadStatusMessage>(result);
                return result;
            }
            catch (Exception exp)
            {
                Error(exp);
                return new MultiMessage<QueryDownloadStatusMessage> { ErrorMsg = exp.Message, ErrorDetailMsg = exp.StackTrace, IsSuccess = false, };
            }
        }

        /// <summary>
        /// 安装流程中检查视频
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public SingleMessage<Dictionary<int, bool>> CheckAlarmVideo(CheckAlarmVideoArgs arg)
        {
            try
            {
                Info("CheckAlarmVideo");
                Info("arg:" + Convert.ToString(arg));
                var userInfo = this.GetUserInfo();

                var result = new SingleMessage<Dictionary<int, bool>>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = _helper.CheckAlarmVideo(context, arg, userInfo.UserName);
                }

                Log<Dictionary<int, bool>>(result);
                return result;
            }
            catch (Exception exp)
            {
                Error(exp);
                return new SingleMessage<Dictionary<int, bool>> { ErrorMsg = exp.Message, ErrorDetailMsg = exp.StackTrace, IsSuccess = false, };
            }
        }

        public MultiMessage<HistoryItemForVideoAppeal> GetHistoryItemForVideoAppeal(string starttime, string endtime)
        {
            try
            {
                Info("GetHistoryItemForVideoAppeal");
                Info("starttime:" + Convert.ToString(starttime) + ";endtime:" + Convert.ToString(endtime));

                MultiMessage<HistoryItemForVideoAppeal> result = new MultiMessage<HistoryItemForVideoAppeal>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = _helper.GetHistoryVideoAppeal(context, starttime, endtime);
                }

                Log<HistoryItemForVideoAppeal>(result);
                return result;
            }
            catch (Exception exp)
            {
                Error(exp);
                return new MultiMessage<HistoryItemForVideoAppeal> { ErrorMsg = exp.Message, ErrorDetailMsg = exp.StackTrace, IsSuccess = false, };
            }
        }

        public MultiMessage<QueryServerFileListMessage> GetAlarmFiftyVideoAppeal(string AlarmId)
        {
            try
            {
                Info("GetAlarmFiftyVideoAppeal");
                Info("AlarmId:" + Convert.ToString(AlarmId));
                var result = new MultiMessage<QueryServerFileListMessage>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = _helper.GetAlarmFiftyVideoAppeal(context, AlarmId);
                }

                Log<QueryServerFileListMessage>(result);
                return result;
            }
            catch (Exception exp)
            {
                Error(exp);
                return new MultiMessage<QueryServerFileListMessage> { ErrorMsg = exp.Message, ErrorDetailMsg = exp.StackTrace, IsSuccess = false, };
            }
        }

        public SingleMessage<bool> UpdateVideoNote(string videoID, string note)
        {
            try
            {
                Info("UpdateVideoNote");
                Info("videoID:" + videoID);
                Info("note:" + note);
                using (var context = new PTMSEntities())
                {
                    SingleMessage<bool> result = _helper.UpdateVideoNote(context, videoID, note);
                    Log<bool>(result);
                    return result;
                }
            }
            catch (Exception exp)
            {
                Error(exp);
                return new SingleMessage<bool> { ErrorMsg = exp.Message, ErrorDetailMsg = exp.StackTrace, IsSuccess = false, };
            }
        }

        public MultiMessage<Photo> GetPhotoList(QueryPhotoFileListArgs arg)
        {
            try
            {
                Info("GetPhotoList");
                Info("MdvrCoreSn:" + arg.MdvrCoreSn);
                using (var context = new PTMSEntities())
                {
                    MultiMessage<Photo> result = _helper.GetPhotoList(context, arg);
                    Log<Photo>(result);
                    return result;
                }
            }
            catch (Exception exp)
            {
                Error(exp);
                return new MultiMessage<Photo> { ErrorMsg = exp.Message, ErrorDetailMsg = exp.StackTrace, IsSuccess = false, };
            }
        }

        public SingleMessage<bool> SetPhotoMark(List<string> list, int status, string note)
        {
            try
            {
                Info("SetPhotoMark");
                Info("note:" + note);
                using (var context = new PTMSEntities())
                {
                    SingleMessage<bool> result = _helper.SetPhotoMark(context, list, status, note);
                    Log<bool>(result);
                    return result;
                }
            }
            catch (Exception exp)
            {
                Error(exp);
                return new SingleMessage<bool> { ErrorMsg = exp.Message, ErrorDetailMsg = exp.StackTrace, IsSuccess = false, };
            }
        }

        public SingleMessage<bool> DeletePhoto(List<string> list)
        {
            try
            {
                Info("DeletePhoto");
                Info("List:" + list);
                using (var context = new PTMSEntities())
                {
                    SingleMessage<bool> result = _helper.DeletePhoto(context, list);
                    Log<bool>(result);
                    return result;
                }
            }
            catch (Exception exp)
            {
                Error(exp);
                return new SingleMessage<bool> { ErrorMsg = exp.Message, ErrorDetailMsg = exp.StackTrace, IsSuccess = false, };
            }
        }

        public MultiMessage<RealTimeChannelInfo> GetChannelByVehicleSN(string vehcilesn)
        {
            try
            {
                Info("GetChannelByVehicleSN");
                MultiMessage<RealTimeChannelInfo> result = new MultiMessage<RealTimeChannelInfo>();
                using (PTMSEntities _context = new PTMSEntities())
                {
                    var w = from s in _context.RUN_SUITE_WORKING
                            join d in _context.MTN_INSTALLATION_DETAIL on s.VEHICLE_ID equals d.VEHICLE_ID
                            join a in _context.MTN_PART_AUDIT on d.ID equals a.INSTALL_AUDIT_ID
                            where s.VEHICLE_ID == vehcilesn
                            select new
                            {
                                ChannelID = a.CHANNEL_ID,
                                MdvrID = s.MDVR_CORE_SN,
                                VehicleSN = vehcilesn
                            };

                    var tempResult = w.ToList();

                    result.Result = tempResult.Select(a => new RealTimeChannelInfo()
                    {
                        ChannelID = a.ChannelID.ToString(),
                        MdvrID = a.MdvrID,
                        VehicleSN = a.VehicleSN
                    }).ToList();

                    result.IsSuccess = true;

                    return result;
                }
            }
            catch (Exception exp)
            {
                Error(exp);
                return new MultiMessage<RealTimeChannelInfo> { ErrorMsg = exp.Message, ErrorDetailMsg = exp.StackTrace, IsSuccess = false, };
            }
        }


        public MultiMessage<QueryServerFileListMessage> QueryHistoryVideoList(string vehicleid, DateTime startTime, DateTime endTime, int pageSize, int pageValue)
        {
            try
            {
                Info("QueryHistoryVideoList");
                MultiMessage<QueryServerFileListMessage> result = _helper.QueryHistoryVideoList(vehicleid, startTime, endTime, pageSize, pageValue);
                Log<QueryServerFileListMessage>(result);
                return result;
            }
            catch (Exception exp)
            {
                Error(exp);
                return new MultiMessage<QueryServerFileListMessage> { ErrorMsg = exp.Message, ErrorDetailMsg = exp.StackTrace, IsSuccess = false, };
            }
        }
    }
}
