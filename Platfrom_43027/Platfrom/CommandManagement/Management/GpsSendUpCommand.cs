/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 245da088-8b7c-4da0-8512-7645420ab9d0      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CommandManagement.Management
/////    Project Description:    
/////             Class Name: GpsSendUpCommand
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/23 15:40:37
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/23 15:40:37
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Command.Repository;
using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.MQ;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Message.Contract.Data;

namespace Gsafety.PTMS.CommandManagement
{
    public class GpsSendUpCommand : BasicInfoManager
    {
        static List<SaveSendUpModel> lstSaveGpsSendUpModel;
        static IUpDateRuleStatus c30CommandRepository;
        static Dictionary<string, string> dicRuleResultInfo;
        static GpsSendUpCommand()
        {
            lstSaveGpsSendUpModel = new List<SaveSendUpModel>();
            c30CommandRepository = new C30CommandRepository();
            dicRuleResultInfo = c30CommandRepository.GetRuleResultInfo();
        }

        public static void CommandSend(byte[] bytes, string key)
        {
            //string guid = Guid.NewGuid().ToString();
            string guid = DateTime.Now.ToString("yyyyMMddHHmmssffff");

            var model = ConvertHelper.BytesToObject(bytes) as GpsSendUpModel;
            model.Setting.CmType = CommandType.C30;
            List<string> lstMdvrs = new List<string>();
            if (model.OperationType == RuleOperationType.Add)
            {
                lstMdvrs = GetLstMdvrs(model.Value);
            }
            else if (model.OperationType == RuleOperationType.Default)
            {
                lstMdvrs = GetLstMdvrs(model.Setting.RuleName, model.Value, dicRuleResultInfo);
                model.Setting.RuleName = c30CommandRepository.GetDefaultRuleID();
            }
            if (lstMdvrs != null && lstMdvrs.Count > 0)
            {
                foreach(string mdvrid in lstMdvrs)
                {
                    bool isRepeatCommand = JudgeRepeatCommand(mdvrid, model.Setting.RuleName, dicRuleResultInfo);
                    if (isRepeatCommand)
                    {
                        LoggerManager.Logger.Info("Repeat Command Message No Send");
                        continue;
                    }
                    SaveSendUpModel saveGpsSendUpModel;
                    model.Setting.DvId = mdvrid;
                    model.Setting.MsgId = guid;
                    SuiteStatusInfo suiteStatusInfo = lstSuiteStatusInfo.Find(c=>c.MdvrCoreId.Equals(mdvrid));
                    saveGpsSendUpModel = GetSaveGpsSendUpModel(model.Setting, suiteStatusInfo.VehicleId, CommandSendStatus.Sending);
                    if (suiteStatusInfo.OnlineFlag)
                    {
                        LoggerManager.Logger.Info(suiteStatusInfo.VehicleId + " is online");
                        var commandContent = System.Text.UTF8Encoding.UTF8.GetBytes(model.Setting.ToString(model.OperationType));
                        SendingCommand sendingCommand = ToSendingEntity(saveGpsSendUpModel);
                        sendingCommand.TimeoutAction = ReplyTimeout;
                        SendingManager.AddSendCommmand(sendingCommand);
                        LoggerManager.Logger.Info("Command is :" + model.Setting.ToString(model.OperationType));
                        CommandManager.PublishMessage(Gsafety.MQ.Constdefine.MDVREXCHANGE, string.Format("{0}{1}", MonitorRoute.HandleGPSMonitorKey, mdvrid), commandContent);
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

        public static void CommandReply(byte[] bytes, string key)
        {
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            LoggerManager.Logger.Info("reply command is :" + str);
            C30Reply c30Replay = new C30Reply(str);
            SendingCommand sendingCommand = SendingManager.GetSendCommand(c30Replay.AssociationSetID, CommandType.C30, c30Replay.MdvrCoreId);
            if (sendingCommand != null)
            {
                lock (lockobj)
                {
                    if (c30Replay.ReplyResult == 1)
                    {
                        sendingCommand.commandSendStatus = CommandSendStatus.Success;
                        RuleHelper ruleHelper = ToRuleHelperEntity(sendingCommand.DeviceID, sendingCommand.RuleID, c30CommandRepository);
                        lstSuccessRuleID.Add(ruleHelper);
                        if (c30Replay.OperType.Equals("0"))
                        {
                            AddRuleResultInfo(sendingCommand.DeviceID, sendingCommand.RuleID, dicRuleResultInfo);
                        }
                        else
                        {
                            RemoveRuleResultInfo(sendingCommand.DeviceID, dicRuleResultInfo);
                        }
                    }
                    else if (c30Replay.ReplyResult == 0)
                    {
                        sendingCommand.commandSendStatus = CommandSendStatus.Failure;
                    }
                    lstWaitUpdateCommand.Add(sendingCommand);
                    SendingManager.RemoveSendCommmand(sendingCommand);
                }
                //reply app info
                //CommandManager.PublishMessage(Gsafety.MQ.Constdefine.APPEXCHANGE, string.Empty, null);
            }
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

        private static SettingGpsSendUpCMD SetCMD(string mdvrid, string guid, string ruleID)
        {
            SettingGpsSendUpCMD cmd = new SettingGpsSendUpCMD();
            cmd.DvId = mdvrid;
            cmd.CmType = CommandType.C30;
            cmd.IsUsing = 0;
            cmd.RuleName = ruleID;
            cmd.MsgId = guid;
            cmd.SendNum = null;
            cmd.DistanceValue = null;
            cmd.TimeValue = null;
            cmd.SendTime = DateTime.Now;
            return cmd;
        }

        private static SaveSendUpModel GetSaveGpsSendUpModel(SettingGpsSendUpCMD Setting, string vehicleid, CommandSendStatus status)
        {
            SaveSendUpModel saveGpsSendUpModel = new SaveSendUpModel();
            saveGpsSendUpModel.ID = Guid.NewGuid().ToString();
            saveGpsSendUpModel.Mdvr_core_sn = Setting.DvId;
            saveGpsSendUpModel.Operation_id = Setting.MsgId;
            saveGpsSendUpModel.Vehicle_id = vehicleid;
            saveGpsSendUpModel.Send_Time = Setting.SendTime;
            saveGpsSendUpModel.Create_Time = Setting.SendTime;
            saveGpsSendUpModel.Status = (int)status;
            saveGpsSendUpModel.Cmd_content = System.Text.UTF8Encoding.UTF8.GetBytes(Setting.ToString());
            saveGpsSendUpModel.Cmd_Exchange = Gsafety.MQ.Constdefine.MDVREXCHANGE;
            saveGpsSendUpModel.Cmd_Route = string.Format("{0}{1}", MonitorRoute.HandleGPSMonitorKey, Setting.DvId);
            saveGpsSendUpModel.Cmd_Type = CommandType.C30;
            saveGpsSendUpModel.RuleID = Setting.RuleName;
            saveGpsSendUpModel.UserName = Setting.UserName;
            return saveGpsSendUpModel;
        }

        public static void WaitingTimeout(WaitSendCommand waiteSendCommand)
        {
            UpdateStatusTimeout(waiteSendCommand.RecordID);
        }

        public static void ReplyTimeout(SendingCommand sendingCommand)
        {
            UpdateStatusTimeout(sendingCommand.RecordID);
        }
    }
}
