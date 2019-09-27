/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: f70ef38d-32b3-4bc2-bf5d-94ec6d8805fb      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CommandManagement.Management
/////    Project Description:    
/////             Class Name: OverSpeedCommand
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/3 9:10:42
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/3 9:10:42
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.Common.Util;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.PTMS.Command.Repository;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.Common.Logging;
using Gsafety.MQ;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Common.Enum;

namespace Gsafety.PTMS.CommandManagement
{
    public class OverSpeedCommand : BasicInfoManager
    {
        static List<SaveSendUpModel> lstSaveGpsSendUpModel;
        static List<SaveVehicleSpeedModel> lstSaveVehicleSpeedModel;
        static OverSpeedCommandRepository overspeedCommandRepository;
        static Dictionary<string, string> dicExistMdvrID;
        static OverSpeedCommand()
        {
            lstSaveGpsSendUpModel = new List<SaveSendUpModel>();
            lstSaveVehicleSpeedModel = new List<SaveVehicleSpeedModel>();
            overspeedCommandRepository = new OverSpeedCommandRepository();
            dicExistMdvrID = overspeedCommandRepository.GetDicFenceID();
            LoggerManager.Logger.Info("dicExistMdvrID" + dicExistMdvrID);
        }



        public static void CommandSend(PTMSEntities context, byte[] bytes, string key)
        {
            try
            {
                var model = ConvertHelper.BytesToObject(bytes) as OverSpeedSendSettingModel;
                string guid = model.Setting.OverSpeedID.ToString();
                model.Setting.MsgId = "1";
                LoggerManager.Logger.Info("OverSpeed CommandSend model" + model);
                List<string> lstMdvrs = new List<string>();
                if (model.OperationType == RuleOperationType.Add)
                {
                    lstMdvrs = GetLstMdvrs(context,model.Value);
                    LoggerManager.Logger.Info("OverSpeed CommandSend lstMdvrs" + lstMdvrs);
                }
                else if (model.OperationType == RuleOperationType.Delete)
                {
                    overspeedCommandRepository = new OverSpeedCommandRepository();
                    dicExistMdvrID = overspeedCommandRepository.GetDicFenceID();
                    model.Value = GetMDVRList(model.Setting.RuleName, dicExistMdvrID);
                    lstMdvrs = GetLstMdvrs(model.Setting.RuleName, model.Value, dicExistMdvrID);
                }

                if (lstMdvrs != null && lstMdvrs.Count > 0)
                {
                    foreach (string mdvrid in lstMdvrs)
                    {
                        if (model.Setting.OperType != 2 && model.Setting.OperType != 0)
                        {
                            bool isRepeatCommand = JudgeRepeatCommand(mdvrid, model.Setting.RuleName, dicExistMdvrID);
                            if (isRepeatCommand)
                            {
                                LoggerManager.Logger.Info("Repeat Command Message No Send");
                                continue;
                            }
                        }
                        SaveSendUpModel saveGpsSendUpModel;
                        model.Setting.DvId = mdvrid;
                        SuiteStatusInfo suiteStatusInfo = lstSuiteStatusInfo.Find(c => c.MdvrCoreId.Equals(mdvrid));
                        LoggerManager.Logger.Info("OverSpeedCommand   model.Setting " + model.Setting);
                        saveGpsSendUpModel = GetSaveGpsSendUpModel(model.Setting, suiteStatusInfo.VehicleId, CommandSendStatus.Sending);
                        if (suiteStatusInfo.OnlineFlag)
                        {
                            LoggerManager.Logger.Info(suiteStatusInfo.VehicleId + " is online");
                            var commandContent = System.Text.UTF8Encoding.UTF8.GetBytes(model.Setting.ToString(model.OperationType));
                            SendingCommand sendingCommand = ToSendingEntity(saveGpsSendUpModel);
                            sendingCommand.TimeoutAction = ReplyTimeout;
                            SendingManager.AddSendCommmand(sendingCommand);
                            CommandManager.PublishMessage(Gsafety.MQ.Constdefine.MDVREXCHANGE, string.Format("{0}{1}", MonitorRoute.HandleSettingOverSpeedCMDKey, mdvrid), commandContent);
                        }
                        else
                        {
                            saveGpsSendUpModel.Status = (int)CommandSendStatus.Wait;
                            LoggerManager.Logger.Info(suiteStatusInfo.VehicleId + " is offline");
                            WaitSendCommand waiteSendCommand = ToWaitSendEntity(saveGpsSendUpModel);
                            waiteSendCommand.TimeoutAction = WaitingTimeout;
                            WaitSendManager.AddWaitSendCommand(waiteSendCommand);
                        }
                        lstSaveGpsSendUpModel.Add(saveGpsSendUpModel);
                    }
                }
                StartUpTask(guid);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("OverSpeed CommmandSend" + ex);
            }
        }

        public static void CommandReply(byte[] bytes, string key)
        {
            try
            {
                string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
                SettingOverSpeedReply overSpeedReplay = new SettingOverSpeedReply(str);
                SendingCommand sendingCommand = SendingManager.GetSendCommand(overSpeedReplay.AssociationSetID, CommandType.OverSpeed, overSpeedReplay.MdvrCoreId);
                if (sendingCommand != null)
                {
                    LoggerManager.Logger.Info("OverSpeed CommandReply sendingCommand is not null" + sendingCommand);
                    lock (lockobj)
                    {
                        if (overSpeedReplay.ReplyResult == 1)
                        {
                            sendingCommand.commandSendStatus = CommandSendStatus.Success;
                            if (overSpeedReplay.OperType.Equals("0"))
                            {
                                RemoveSpeedID(sendingCommand.DeviceID, "");
                                AddSpeedID(sendingCommand.DeviceID, sendingCommand.OperationID);
                            }
                            else
                            {
                                RemoveSpeedID(sendingCommand.DeviceID, sendingCommand.OperationID);
                            }
                        }
                        else if (overSpeedReplay.ReplyResult == 0)
                        {
                            sendingCommand.commandSendStatus = CommandSendStatus.Failure;
                        }
                        lstWaitUpdateCommand.Add(sendingCommand);
                        SendingManager.RemoveSendCommmand(sendingCommand);
                    }
                }
                else
                {
                    LoggerManager.Logger.Info("OverSpeed CommandReply sendingCommand is null");
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("OverSpeed CommandReply" + ex);
            }
        }

        private static bool JudgeRepeatCommand(string mdvrid, string RuleName, Dictionary<string, List<string>> dicExistMdvrID)
        {
            List<string> RuleList = new List<string>();
            bool IsHandMdvrid = dicExistMdvrID.TryGetValue(mdvrid, out RuleList);
            if (IsHandMdvrid)
            {
                foreach (var rule in RuleList)
                {
                    if (rule.Equals(RuleName))
                    {
                        return true;
                    }
                }
            }
            else
            {
                return false;
            }
            return false;
        }


        protected static SendingCommand ToSendingEntity(SaveSendUpModel saveSendUpModel)
        {
            SendingCommand sendingCommand = new SendingCommand();
            sendingCommand.CommandType = saveSendUpModel.Cmd_Type;
            sendingCommand.OperationID = saveSendUpModel.Operation_id;
            sendingCommand.DeviceID = saveSendUpModel.Mdvr_core_sn;
            sendingCommand.RecordID = saveSendUpModel.ID;
            sendingCommand.RequestTime = saveSendUpModel.Send_Time;
            sendingCommand.SendingTime = DateTime.Now;
            sendingCommand.SendingTimeout = sendingCommand.SendingTime.AddSeconds(ConfigInfo.CommandTimeout);
            sendingCommand.TimeoutAction = ReplyTimeout;
            sendingCommand.SendCommandBytes = saveSendUpModel.Cmd_content;
            sendingCommand.RuleID = saveSendUpModel.RuleID;
            sendingCommand.UserName = saveSendUpModel.UserName;
            sendingCommand.VehicleID = saveSendUpModel.Vehicle_id;
            sendingCommand.OperateType = saveSendUpModel.Cmd_Sub_Type;
            return sendingCommand;
        }

        protected static WaitSendCommand ToWaitSendEntity(SaveSendUpModel saveSendUpModel)
        {
            WaitSendCommand waiteSendCommand = new WaitSendCommand();
            waiteSendCommand.CommandType = saveSendUpModel.Cmd_Type;
            waiteSendCommand.RecordID = saveSendUpModel.ID;
            waiteSendCommand.DeviceID = saveSendUpModel.Mdvr_core_sn;
            waiteSendCommand.OperationID = saveSendUpModel.Operation_id;
            waiteSendCommand.CommandContent = saveSendUpModel.Cmd_content;
            waiteSendCommand.Exchange = saveSendUpModel.Cmd_Exchange;
            waiteSendCommand.RequestSendTime = saveSendUpModel.Send_Time;
            waiteSendCommand.RequestTimeout = waiteSendCommand.RequestSendTime.AddSeconds(ConfigInfo.TrafficWaitTimeout);
            waiteSendCommand.TimeoutAction = WaitingTimeout;
            waiteSendCommand.RouteKey = saveSendUpModel.Cmd_Route;
            return waiteSendCommand;
        }



        private static void StartUpTask(string guid)
        {
            if (lstSaveGpsSendUpModel != null && lstSaveGpsSendUpModel.Count > 0)
            {
                TaskHelper taskHelper = new TaskHelper();
                taskHelper.TaskId = guid;
                taskHelper.TaskMethod = Task.Factory.StartNew(() => { SaveGpsSendUpModelInDB(); });
                lstTaskHelper.Add(taskHelper);
            }
        }

        private static void SaveGpsSendUpModelInDB()
        {
            if (lstSaveGpsSendUpModel != null && lstSaveGpsSendUpModel.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (SaveSendUpModel saveGpsSendUpModel in lstSaveGpsSendUpModel)
                {
                    builder.Append(saveGpsSendUpModel.ToString());
                }
                if (builder.Length > 0)
                {
                    builder.Insert(0, " BEGIN ");
                    builder.Append(" END; ");
                    OracleHelper.ExecuteSql(builder.ToString());
                    builder.Clear();
                }
                lstSaveGpsSendUpModel.Clear();
            }
        }

        private static SaveSendUpModel GetSaveGpsSendUpModel(SettingOverSpeedCMD Setting, string vehicleid, CommandSendStatus status)
        {
            SaveSendUpModel saveGpsSendUpModel = new SaveSendUpModel();
            saveGpsSendUpModel.ID = Guid.NewGuid().ToString();
            saveGpsSendUpModel.Mdvr_core_sn = Setting.DvId;
            saveGpsSendUpModel.Operation_id = Setting.OverSpeedID;
            saveGpsSendUpModel.Vehicle_id = vehicleid;
            saveGpsSendUpModel.Send_Time = Setting.SendTime;
            saveGpsSendUpModel.Create_Time = Setting.SendTime;
            saveGpsSendUpModel.Status = (int)status;
            saveGpsSendUpModel.Cmd_content = System.Text.UTF8Encoding.UTF8.GetBytes(Setting.ToString());
            saveGpsSendUpModel.Cmd_Exchange = Gsafety.MQ.Constdefine.MDVREXCHANGE;
            saveGpsSendUpModel.Cmd_Route = string.Format("{0}{1}", MonitorRoute.HandleSettingOverSpeedCMDKey, Setting.DvId);
            saveGpsSendUpModel.Cmd_Type = CommandType.OverSpeed;
            saveGpsSendUpModel.RuleID = Setting.RuleName;
            saveGpsSendUpModel.UserName = Setting.UserName;
            LoggerManager.Logger.Info("OverSpeedCommand  Setting.MsgId" + "   " + Setting.MsgId + "Operation_id" + saveGpsSendUpModel.Operation_id + "SaveSendUpModel GetSaveGpsSendUpModel" + saveGpsSendUpModel.Operation_id);
            return saveGpsSendUpModel;
        }

        private static SettingOverSpeedCMD SetCMD(string mdvrid, string guid, string ruleID)
        {
            SettingOverSpeedCMD cmd = new SettingOverSpeedCMD();
            cmd.DvId = mdvrid;
            cmd.CmType = CommandType.PolygonsRegion;
            cmd.OverSpeedID = ruleID;
            cmd.RuleName = ruleID;
            cmd.MsgId = guid;
            cmd.SendTime = DateTime.Now;
            cmd.OperType = 0;
            return cmd;
        }

        private static void AddSpeedID(string mdvrID, string fenceID)
        {
            if (dicExistMdvrID.ContainsKey(mdvrID))
            {
                dicExistMdvrID[mdvrID] = fenceID;
            }
        }

        private static void RemoveSpeedID(string mdvrID, string fenceID)
        {
            if (dicExistMdvrID.ContainsKey(mdvrID))
            {
                dicExistMdvrID.Remove(mdvrID);
            }
        }

        public static void WaitingTimeout(PTMSEntities context, WaitSendCommand waiteSendCommand)
        {
            UpdateStatusTimeout(context, waiteSendCommand.RecordID);
            overspeedCommandRepository.UpdateStatusTimeout(context, waiteSendCommand.RecordID);
        }

        public static void ReplyTimeout(PTMSEntities context, SendingCommand sendingCommand)
        {
            UpdateStatusTimeout(context, sendingCommand.RecordID);
            overspeedCommandRepository.UpdateStatusTimeout(context, sendingCommand.RecordID);
        }
        public static List<SelectInfoModel> GetMDVRList(string ruleID, Dictionary<string, string> MdvrList)
        {

            List<SelectInfoModel> MdvrListInfo = new List<SelectInfoModel>();
            SelectInfoModel MdvrInfo = new SelectInfoModel();
            foreach (KeyValuePair<string, string> item in MdvrList)
            {
                if (item.Value.Equals(ruleID))
                {
                    MdvrInfo.Code = item.Key;
                    MdvrListInfo.Add(MdvrInfo);
                }
            }
            return MdvrListInfo;
        }

    }
}
