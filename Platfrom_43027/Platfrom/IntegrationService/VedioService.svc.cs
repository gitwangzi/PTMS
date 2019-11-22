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
using Gsafety.PTMS.Integration.Contract;
using Gsafety.PTMS.Integration.Contract.Data;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.Integration.Service
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

        public SingleMessage<bool> CheckAlarmVideo(CheckAlarmVideoArgs arg)
        {
            try
            {
                Info("CheckAlarmVideo");
                Info("arg:" + Convert.ToString(arg));
                var userInfo = this.GetUserInfo();

                SingleMessage<bool> result = new SingleMessage<bool>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = _helper.CheckAlarmVideo(context, arg, userInfo.UserName);
                }

                Log<bool>(result);
                return result;
            }
            catch (Exception exp)
            {
                Error(exp);
                return new SingleMessage<bool> { ErrorMsg = exp.Message, ErrorDetailMsg = exp.StackTrace, IsSuccess = false, };
            }
        }

        public MultiMessage<QueryServerDownloadFileListMessage> QueryServerDownloadFileList(QueryServerDownloadFileListArgs arg)
        {
            try
            {
                Info("QueryServerDownloadFileList");
                Info("arg:" + Convert.ToString(arg));
                var userInfo = this.GetUserInfo();

                MultiMessage<QueryServerDownloadFileListMessage> result = _helper.QueryServerDownloadFileList(arg, userInfo.UserName);
                Log<QueryServerDownloadFileListMessage>(result);
                return result;
            }
            catch (Exception exp)
            {
                Error(exp);
                return new MultiMessage<QueryServerDownloadFileListMessage> { ErrorMsg = exp.Message, ErrorDetailMsg = exp.StackTrace, IsSuccess = false, };
            }
        }

        public MultiMessage<QueryServerFileListMessage> QueryServerFileListByVehicleSN(string vehiclesn, string starttime, string endtime, string pagesize, string pageindex)
        {
            try
            {
                Info("QueryServerFileListByVehicleSN");
                Info("vehiclesn:" + Convert.ToString(vehiclesn) + ";starttime:" + Convert.ToString(starttime) + ";endtime:" + Convert.ToString(endtime));

                QueryServerFileListArgs arg = new QueryServerFileListArgs();

                using (PTMSEntities context = new PTMSEntities())
                {
                    arg.Mdvr_Id = _helper.GetMdvrSNByVehicleSN(context, vehiclesn);
                }

                arg.Start_Time = DateTime.Parse(starttime);
                arg.End_Time = DateTime.Parse(endtime);
                arg.PageNum = int.Parse(pageindex);
                arg.PageSize = int.Parse(pagesize);

                MultiMessage<QueryServerFileListMessage> result = _helper.QueryServerFileList(arg, string.Empty);
                Log<QueryServerFileListMessage>(result);
                return result;
            }
            catch (Exception exp)
            {
                Error(exp);
                return new MultiMessage<QueryServerFileListMessage> { ErrorMsg = exp.Message, ErrorDetailMsg = exp.StackTrace, IsSuccess = false, };
            }
        }

        public MultiMessage<RealTimeChannelInfo> GetChannelByVehicleSN(string vehcilesn)
        {
            try
            {
                Info("GetChannelByVehicleSN");
                MultiMessage<RealTimeChannelInfo> result = new MultiMessage<RealTimeChannelInfo>();

                string mdvr = string.Empty;
                using (PTMSEntities context = new PTMSEntities())
                {
                    mdvr = _helper.GetMdvrSNByVehicleSN(context, vehcilesn);
                }

                result.IsSuccess = true;

                List<RealTimeChannelInfo> rtcis = new List<RealTimeChannelInfo>();
                RealTimeChannelInfo rtci1 = new RealTimeChannelInfo();
                rtci1.MdvrID = mdvr;
                rtci1.VehicleSN = vehcilesn;
                rtci1.ChannelID = "0";
                rtcis.Add(rtci1);

                RealTimeChannelInfo rtci2 = new RealTimeChannelInfo();
                rtci2.MdvrID = mdvr;
                rtci2.VehicleSN = vehcilesn;
                rtci2.ChannelID = "1";
                rtcis.Add(rtci2);

                RealTimeChannelInfo rtci3 = new RealTimeChannelInfo();
                rtci3.MdvrID = mdvr;
                rtci3.VehicleSN = vehcilesn;
                rtci3.ChannelID = "2";
                rtcis.Add(rtci3);

                RealTimeChannelInfo rtci4 = new RealTimeChannelInfo();
                rtci4.MdvrID = mdvr;
                rtci4.VehicleSN = vehcilesn;
                rtci4.ChannelID = "3";
                rtcis.Add(rtci4);
                return result;
            }
            catch (Exception exp)
            {
                Error(exp);
                return new MultiMessage<RealTimeChannelInfo> { ErrorMsg = exp.Message, ErrorDetailMsg = exp.StackTrace, IsSuccess = false, };
            }
        }

        public MultiMessage<QueryServerFileListMessage> QueryServerFileListByMDVR(string mdvr, string starttime, string endtime, string pagesize, string pageindex)
        {
            try
            {
                Info("QueryServerFileListByMDVR");
                Info("vehiclesn:" + Convert.ToString(mdvr) + ";starttime:" + Convert.ToString(starttime) + ";endtime:" + Convert.ToString(endtime));

                QueryServerFileListArgs arg = new QueryServerFileListArgs();
                arg.Mdvr_Id = mdvr;
                arg.Start_Time = DateTime.Parse(starttime);
                arg.End_Time = DateTime.Parse(endtime);
                arg.PageNum = int.Parse(pageindex);
                arg.PageSize = int.Parse(pagesize);

                MultiMessage<QueryServerFileListMessage> result = _helper.QueryServerFileList(arg, string.Empty);
                Log<QueryServerFileListMessage>(result);
                return result;
            }
            catch (Exception exp)
            {
                Error(exp);
                return new MultiMessage<QueryServerFileListMessage> { ErrorMsg = exp.Message, ErrorDetailMsg = exp.StackTrace, IsSuccess = false, };
            }
        }

        public MultiMessage<RealTimeChannelInfo> GetChannelByMDVR(string mdvr)
        {
            try
            {
                Info("GetChannelByMDVR");
                Info("vehiclesn:" + Convert.ToString(mdvr));
                MultiMessage<RealTimeChannelInfo> result = new MultiMessage<RealTimeChannelInfo>();

                string vehiclesn = string.Empty;
                using (PTMSEntities context = new PTMSEntities())
                {
                    vehiclesn = _helper.GetVehicleSnByMdvr(context, mdvr);
                }


                result.IsSuccess = true;

                List<RealTimeChannelInfo> rtcis = new List<RealTimeChannelInfo>();
                RealTimeChannelInfo rtci1 = new RealTimeChannelInfo();
                rtci1.MdvrID = mdvr;
                rtci1.VehicleSN = vehiclesn;
                rtci1.ChannelID = "0";
                rtcis.Add(rtci1);

                RealTimeChannelInfo rtci2 = new RealTimeChannelInfo();
                rtci2.MdvrID = mdvr;
                rtci2.VehicleSN = vehiclesn;
                rtci2.ChannelID = "1";
                rtcis.Add(rtci2);

                RealTimeChannelInfo rtci3 = new RealTimeChannelInfo();
                rtci3.MdvrID = mdvr;
                rtci3.VehicleSN = vehiclesn;
                rtci3.ChannelID = "2";
                rtcis.Add(rtci3);

                RealTimeChannelInfo rtci4 = new RealTimeChannelInfo();
                rtci4.MdvrID = mdvr;
                rtci4.VehicleSN = vehiclesn;
                rtci4.ChannelID = "3";
                rtcis.Add(rtci4);
                return result;
            }
            catch (Exception exp)
            {
                Error(exp);
                return new MultiMessage<RealTimeChannelInfo> { ErrorMsg = exp.Message, ErrorDetailMsg = exp.StackTrace, IsSuccess = false, };
            }
        }


        public MultiMessage<QueryServerFileListMessage> QueryServerFileListBySN(string sn, string starttime, string endtime, string pagesize, string pageindex)
        {
            try
            {
                Info("QueryServerFileListBySN");
                Info("vehiclesn:" + Convert.ToString(sn) + ";starttime:" + Convert.ToString(starttime) + ";endtime:" + Convert.ToString(endtime));
                string mdvr = string.Empty;
                using (PTMSEntities context = new PTMSEntities())
                {
                    mdvr = _helper.GetMdvrSNByVehicleSN(context, sn);
                }

                QueryServerFileListArgs arg = new QueryServerFileListArgs();
                if (!string.IsNullOrEmpty(mdvr))
                {
                    //传过来的为vehcilesn
                    arg.Mdvr_Id = mdvr;
                }
                else
                {
                    arg.Mdvr_Id = sn;
                }

                arg.Start_Time = DateTime.Parse(starttime);
                arg.End_Time = DateTime.Parse(endtime);
                arg.PageNum = int.Parse(pageindex);
                arg.PageSize = int.Parse(pagesize);

                MultiMessage<QueryServerFileListMessage> result = _helper.QueryServerFileList(arg, string.Empty);
                Log<QueryServerFileListMessage>(result);
                return result;
            }
            catch (Exception exp)
            {
                Error(exp);
                return new MultiMessage<QueryServerFileListMessage> { ErrorMsg = exp.Message, ErrorDetailMsg = exp.StackTrace, IsSuccess = false, };
            }
        }

        public MultiMessage<RealTimeChannelInfo> GetChannelBySN(string sn)
        {
            try
            {
                Info("GetChannelBySN");
                Info("vehiclesn:" + Convert.ToString(sn));
                MultiMessage<RealTimeChannelInfo> result = new MultiMessage<RealTimeChannelInfo>();
                List<RealTimeChannelInfo> rtcis = new List<RealTimeChannelInfo>();
                result.IsSuccess = true;
                string mdvr = string.Empty;
                using (PTMSEntities context = new PTMSEntities())
                {
                    mdvr = _helper.GetMdvrSNByVehicleSN(context, sn);
                }

                if (!string.IsNullOrEmpty(mdvr))
                {
                    RealTimeChannelInfo rtci1 = new RealTimeChannelInfo();
                    rtci1.MdvrID = mdvr;
                    rtci1.VehicleSN = sn;
                    rtci1.ChannelID = "0";
                    rtci1.OneKey15Enabled = true;
                    rtcis.Add(rtci1);

                    RealTimeChannelInfo rtci2 = new RealTimeChannelInfo();
                    rtci2.MdvrID = mdvr;
                    rtci2.VehicleSN = sn;
                    rtci2.ChannelID = "1";
                    rtci2.OneKey15Enabled = false;
                    rtcis.Add(rtci2);

                    RealTimeChannelInfo rtci3 = new RealTimeChannelInfo();
                    rtci3.MdvrID = mdvr;
                    rtci3.VehicleSN = sn;
                    rtci3.ChannelID = "2";
                    rtci3.OneKey15Enabled = false;
                    rtcis.Add(rtci3);

                    RealTimeChannelInfo rtci4 = new RealTimeChannelInfo();
                    rtci4.MdvrID = mdvr;
                    rtci4.VehicleSN = sn;
                    rtci4.ChannelID = "3";
                    rtci4.OneKey15Enabled = false;
                    rtcis.Add(rtci4);
                    result.Result = rtcis;
                    return result;
                }
                else
                {
                    string vehicle = string.Empty;
                    using (PTMSEntities context = new PTMSEntities())
                    {
                        vehicle = _helper.GetVehicleSnByMdvr(context, sn);
                    }


                    RealTimeChannelInfo rtci1 = new RealTimeChannelInfo();
                    rtci1.MdvrID = sn;
                    rtci1.VehicleSN = vehicle;
                    rtci1.ChannelID = "0";
                    rtci1.OneKey15Enabled = true;
                    rtcis.Add(rtci1);

                    RealTimeChannelInfo rtci2 = new RealTimeChannelInfo();
                    rtci2.MdvrID = sn;
                    rtci2.VehicleSN = vehicle;
                    rtci2.ChannelID = "1";
                    rtci2.OneKey15Enabled = false;
                    rtcis.Add(rtci2);

                    RealTimeChannelInfo rtci3 = new RealTimeChannelInfo();
                    rtci3.MdvrID = sn;
                    rtci3.VehicleSN = vehicle;
                    rtci3.OneKey15Enabled = false;
                    rtci3.ChannelID = "2";
                    rtcis.Add(rtci3);

                    RealTimeChannelInfo rtci4 = new RealTimeChannelInfo();
                    rtci4.MdvrID = sn;
                    rtci4.VehicleSN = vehicle;
                    rtci4.ChannelID = "3";
                    rtci4.OneKey15Enabled = false;
                    rtcis.Add(rtci4);

                    result.Result = rtcis;
                    return result;
                }
            }
            catch (Exception exp)
            {
                Error(exp);
                return new MultiMessage<RealTimeChannelInfo> { ErrorMsg = exp.Message, ErrorDetailMsg = exp.StackTrace, IsSuccess = false, };
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




        public SingleMessage<string> GetAlarmFiftyVideoAppeal(string AlarmId)
        {
            try
            {
                Info("GetAlarmFiftyVideoAppeal");
                Info("AlarmId:" + Convert.ToString(AlarmId));
                var result = new SingleMessage<string>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = _helper.GetAlarmFiftyVideoAppeal(context, AlarmId);
                }

                Log<string>(result);
                return result;
            }
            catch (Exception exp)
            {
                Error(exp);
                return new SingleMessage<string> { ErrorMsg = exp.Message, ErrorDetailMsg = exp.StackTrace, IsSuccess = false, };
            }

        }


        public SingleMessage<bool> IsDownloading(string mdvr)
        {
            try
            {
                Info("IsDownloading");
                Info("mdvr:" + Convert.ToString(mdvr));
                var result = _helper.IsDownloading(mdvr);
                Log<bool>(result);
                return result;
            }
            catch (Exception exp)
            {
                Error(exp);
                return new SingleMessage<bool> { ErrorMsg = exp.Message, ErrorDetailMsg = exp.StackTrace, IsSuccess = false, };
            }
        }
    }
}
