/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: d2ec524e-4d04-4fba-b7fc-807c24a5d790      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CommandManagement.Management
/////    Project Description:    
/////             Class Name: TemperatureCommand
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/31 17:31:52
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/31 17:31:52
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.Common.Util;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.MQ;
using Gsafety.Common.Logging;
using Gsafety.PTMS.Command.Repository;

namespace Gsafety.PTMS.CommandManagement
{
    public class TemperatureCommand : BasicInfoManager
    {
        
        public static List<SaveSendUpModel> lstSaveGpsSendUpModel;
        static IUpDateRuleStatus c64CommandRepository;
        static Dictionary<string, string> dicRuleResultInfo;
        static TemperatureCommand()
        {
            lstSaveGpsSendUpModel = new List<SaveSendUpModel>();
            c64CommandRepository = new C64CommandRepository();
            dicRuleResultInfo = c64CommandRepository.GetRuleResultInfo();
        }

        public static void CommandSend(byte[] bytes, string key)
        {
            //string guid = Guid.NewGuid().ToString();
            string guid = DateTime.Now.ToString("yyyyMMddHHmmssffff");

            var model = ConvertHelper.BytesToObject(bytes) as TemperatureSendUpModel;
            model.Setting.CmType = CommandType.C64;
            List<string> lstMdvrs = new List<string>();
            if (model.OperationType == RuleOperationType.Add)
            {
                lstMdvrs = GetLstMdvrs(model.Value);
            }
            else if (model.OperationType == RuleOperationType.Default)
            {
                lstMdvrs = GetLstMdvrs(model.Setting.RuleName, model.Value, dicRuleResultInfo);
                model.Setting.RuleName = c64CommandRepository.GetDefaultRuleID();
            }
            if (lstMdvrs.Count > 0)
            {
                foreach (string mdvrid in lstMdvrs)
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
                    SuiteStatusInfo suiteStatusInfo = lstSuiteStatusInfo.Find(c => c.MdvrCoreId.Equals(mdvrid));
                    saveGpsSendUpModel = GetSaveSendUpModel(model.Setting, suiteStatusInfo.VehicleId, CommandSendStatus.Sending);
                    if (suiteStatusInfo.OnlineFlag)
                    {
                        var commandContent = System.Text.UTF8Encoding.UTF8.GetBytes(model.Setting.ToString(model.OperationType));
                        SendingCommand sendingCommand = ToSendingEntity(saveGpsSendUpModel);
                        sendingCommand.TimeoutAction = ReplyTimeout;
                        SendingManager.AddSendCommmand(sendingCommand);
                        CommandManager.PublishMessage(Gsafety.MQ.Constdefine.MDVREXCHANGE, string.Format("{0}{1}", MonitorRoute.HandleTemperatureKey, mdvrid), commandContent);
                        LoggerManager.Logger.Info("Command is :" + model.Setting.ToString(model.OperationType));
                    }
                    else
                    {
                        saveGpsSendUpModel.Status = (int)CommandSendStatus.Wait;
                        WaitSendCommand waiteSendCommand = ToWaitSendEntity(saveGpsSendUpModel);
                        waiteSendCommand.TimeoutAction = WaitingTimeout;
                        WaitSendManager.AddWaitSendCommand(waiteSendCommand);
                    }
                    lstSaveGpsSendUpModel.Add(saveGpsSendUpModel);
                }
            }
            StartUpTask(guid);
        }

        //public static void DeleteCommandSend(byte[] bytes, string key)
        //{
        //    string guid = Guid.NewGuid().ToString();
        //    RuleDeleteModel ruleDeleteModel = ConvertHelper.BytesToObject(bytes) as RuleDeleteModel;
        //    List<string> lstMdvrs = new List<string>();
        //    if (lstMdvrs != null && lstMdvrs.Count > 0)
        //    {
        //        foreach (string mdvrid in lstMdvrs)
        //        {
        //            SettingTemperatureCMD cmd = SetCMD(mdvrid, guid, ruleDeleteModel.RuleId);
        //            SuiteStatusInfo suiteStatusInfo = lstSuiteStatusInfo.Find(c => c.MdvrCoreId.Equals(mdvrid));
        //            SaveSendUpModel saveGpsSendUpModel = GetSaveSendUpModel(cmd, suiteStatusInfo.VehicleId, CommandSendStatus.Sending);
        //            if (suiteStatusInfo.OnlineFlag)
        //            {
        //                LoggerManager.Logger.Info(suiteStatusInfo.VehicleId + " is online");
        //                var commandContent = System.Text.UTF8Encoding.UTF8.GetBytes(cmd.ToString());
        //                SendingCommand sendingCommand = ToSendingEntity(saveGpsSendUpModel);
        //                sendingCommand.TimeoutAction = ReplyTimeout;
        //                SendingManager.AddSendCommmand(sendingCommand);
        //                LoggerManager.Logger.Info("Command is :" + cmd.ToString());
        //                CommandManager.PublishMessage(Gsafety.MQ.Constdefine.MDVREXCHANGE, string.Format("{0}{1}", MonitorRoute.HandleGPSMonitorKey, mdvrid), commandContent);
        //            }
        //            else
        //            {
        //                saveGpsSendUpModel.Status = (int)CommandSendStatus.Wait;
        //                LoggerManager.Logger.Info(suiteStatusInfo.VehicleId + " is offline");
        //                WaitSendCommand waiteSendCommand = ToWaitSendEntity(saveGpsSendUpModel);
        //                waiteSendCommand.TimeoutAction = WaitingTimeout;
        //                WaitSendManager.AddWaitSendCommand(waiteSendCommand);
        //            }
        //            lstSaveGpsSendUpModel.Add(saveGpsSendUpModel);
        //        }
        //    }
        //    StartUpTask(guid);
        //}

        public static void CommandReply(byte[] bytes, string key)
        {
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            LoggerManager.Logger.Info("reply command is :" + str);
            C64Relpy c64Replay = new C64Relpy(str);
            SendingCommand sendingCommand = SendingManager.GetSendCommand(c64Replay.AssociationSetID, CommandType.C64, c64Replay.MdvrCoreId);
            if (sendingCommand != null)
            {
                lock (lockobj)
                {
                    if (c64Replay.ReplyResult == 1)
                    {
                        sendingCommand.commandSendStatus = CommandSendStatus.Success;
                        RuleHelper ruleHelper = ToRuleHelperEntity(sendingCommand.DeviceID, sendingCommand.RuleID, c64CommandRepository);
                        lstSuccessRuleID.Add(ruleHelper);
                        if (c64Replay.OperType.Equals("0"))
                        {
                            AddRuleResultInfo(sendingCommand.DeviceID, sendingCommand.RuleID, dicRuleResultInfo);
                        }
                        else
                        {
                            RemoveRuleResultInfo(sendingCommand.DeviceID, dicRuleResultInfo);
                        }
                    }
                    else if (c64Replay.ReplyResult == 0)
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
                taskHelper.TaskMethod = Task.Factory.StartNew(() => { SaveSendUpModelInDB(); });
                lstTaskHelper.Add(taskHelper);
            }
        }

        private static void SaveSendUpModelInDB()
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

        private static SettingTemperatureCMD SetCMD(string mdvrid, string guid, string ruleID)
        {
            SettingTemperatureCMD cmd = new SettingTemperatureCMD();
            cmd.DvId = mdvrid;
            cmd.CmType = CommandType.C30;
            cmd.RuleName = ruleID;
            cmd.MsgId = guid;
            cmd.SendTime = DateTime.Now;
            cmd.SendType = TemperatureMarkType.Forbid;
            cmd.TemperatureType = null;
            cmd.MinValue = null;
            cmd.MaxValue = null;
            return cmd;
        }

        private static SaveSendUpModel GetSaveSendUpModel(SettingTemperatureCMD Setting, string vehicleid, CommandSendStatus status)
        {
            SaveSendUpModel saveGpsSendUpModel = new SaveSendUpModel();
            saveGpsSendUpModel.ID = Guid.NewGuid().ToString();
            saveGpsSendUpModel.Mdvr_core_sn = Setting.DvId;
            saveGpsSendUpModel.Operation_id = Setting.MsgId;
            saveGpsSendUpModel.Vehicle_id = vehicleid;
            saveGpsSendUpModel.Send_Time = DateTime.Now;
            saveGpsSendUpModel.Create_Time = Setting.SendTime;
            saveGpsSendUpModel.Status = (int)status;
            saveGpsSendUpModel.Cmd_content = System.Text.UTF8Encoding.UTF8.GetBytes(Setting.ToString());
            saveGpsSendUpModel.Cmd_Exchange = Gsafety.MQ.Constdefine.MDVREXCHANGE;
            saveGpsSendUpModel.Cmd_Route = string.Format("{0}{1}", MonitorRoute.HandleTemperatureKey, Setting.DvId);
            saveGpsSendUpModel.Cmd_Type = CommandType.C64;
            saveGpsSendUpModel.RuleID = Setting.RuleName;
            saveGpsSendUpModel.UserName = Setting.UserName;
            return saveGpsSendUpModel;
        }

        public static void ReplyTimeout(SendingCommand sendingCommand)
        {
            UpdateStatusTimeout(sendingCommand.RecordID);
        }

        public static void WaitingTimeout(WaitSendCommand waiteSendCommand)
        {
            UpdateStatusTimeout(waiteSendCommand.RecordID);
        }
    }
}
