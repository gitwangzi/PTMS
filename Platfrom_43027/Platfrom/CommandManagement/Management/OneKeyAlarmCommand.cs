/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: a720610d-98e7-4b48-82fd-4fdfce295ead      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CommandManagement.Management
/////    Project Description:    
/////             Class Name: OneKeyAlarmCommand
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/31 17:32:24
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/31 17:32:24
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
using Gsafety.Common.Logging;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.MQ;
using Gsafety.PTMS.Command.Repository;

namespace Gsafety.PTMS.CommandManagement
{
    public class OneKeyAlarmCommand : BasicInfoManager
    {
        public static List<SaveSendUpModel> lstSaveGpsSendUpModel;
        private static List<string> lstCommand;
        static IUpDateRuleStatus alarmCommandRepository;
        static Dictionary<string, List<SendingCommand>> dicSuccessCommand;
        static Dictionary<string, string> dicRuleResultInfo;

        static OneKeyAlarmCommand()
        {
            lstCommand = new List<string>() { CommandType.C78, CommandType.C80 };
            lstSaveGpsSendUpModel = new List<SaveSendUpModel>();
            alarmCommandRepository = new AlarmCommandRepository();
            dicRuleResultInfo = alarmCommandRepository.GetRuleResultInfo();
            dicSuccessCommand = new Dictionary<string, List<SendingCommand>>();
            Task.Factory.StartNew(() => { GetRuleIDSuccess(); });
        }

        public static void CommandSend(byte[] bytes, string key)
        {
            //string guid = Guid.NewGuid().ToString();
            string guid = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            var model = ConvertHelper.BytesToObject(bytes) as OneKeyAlarmSendUpModel;
            model.Setting.CmType = CommandType.C80;
            model.DelayAlarmSetting.CmType = CommandType.C78;
            List<string> lstMdvrs = new List<string>();
            if (model.OperationType == RuleOperationType.Add) 
            {
                lstMdvrs = GetLstMdvrs(model.Value);
            }
            else if (model.OperationType == RuleOperationType.Default)
            {
                lstMdvrs = GetLstMdvrs(model.Setting.RuleName, model.Value, dicRuleResultInfo);
                model.Setting.RuleName = alarmCommandRepository.GetDefaultRuleID();
                model.DelayAlarmSetting.RuleID = model.Setting.RuleName;

            }
            LoggerManager.Logger.Info(string.Format("begin send OneKeyAlarmCommand,Mdvr count： {0}", lstMdvrs.Count));

            if (lstMdvrs != null && lstMdvrs.Count > 0)
            {
                foreach (string mdvrid in lstMdvrs)
                {
                    bool isRepeatCommand = JudgeRepeatCommand(mdvrid, model.Setting.RuleName, dicRuleResultInfo);
                    if (isRepeatCommand)
                    {
                        LoggerManager.Logger.Info(string.Format("Repeat Command Message No Send. MDVR SN:{0}, Rule Id :{1}", mdvrid, model.Setting.RuleName));
                        continue;
                    }
                    SaveSendUpModel saveSendUpModel;
                    byte[] commandContent;
                    string routeKey = string.Empty;
                    SuiteStatusInfo suiteStatusInfo = lstSuiteStatusInfo.Find(c => c.MdvrCoreId.Equals(mdvrid));
                    model.DelayAlarmSetting.UserName = model.Setting.UserName;
                    foreach (string commandType in lstCommand)
                    {
                        if (commandType == CommandType.C80)
                        {
                            model.Setting.DvId = mdvrid;
                            model.Setting.MsgId = guid;
                            saveSendUpModel = GetSaveOneKeyAlarmModel(model.Setting, suiteStatusInfo.VehicleId, CommandSendStatus.Sending, commandType);
                            commandContent = System.Text.UTF8Encoding.UTF8.GetBytes(model.Setting.ToString(model.OperationType));
                            routeKey = string.Format("{0}{1}", MonitorRoute.HandleOneKeyAlarmKey, mdvrid);
                            //c80Successfull = false;
                        }
                        else
                        {
                            model.DelayAlarmSetting.DvId = mdvrid;
                            model.DelayAlarmSetting.MsgId = guid;
                            saveSendUpModel = GetSaveDelayAlarmModel(model.DelayAlarmSetting, suiteStatusInfo.VehicleId, CommandSendStatus.Sending, commandType);
                            commandContent = System.Text.UTF8Encoding.UTF8.GetBytes(model.DelayAlarmSetting.ToString(model.OperationType));
                            routeKey = string.Format("{0}{1}", MonitorRoute.HandleDelayAlarmKey, mdvrid);
                            //c78Successfull = false;
                        }
                        if (suiteStatusInfo.OnlineFlag)
                        {
                            LoggerManager.Logger.Info(suiteStatusInfo.VehicleId + " is online");
                            SendingCommand sendingCommand = ToSendingEntity(saveSendUpModel);
                            sendingCommand.TimeoutAction = ReplyTimeout;
                            SendingManager.AddSendCommmand(sendingCommand);
                            CommandManager.PublishMessage(Gsafety.MQ.Constdefine.MDVREXCHANGE, routeKey, commandContent);
                            LoggerManager.Logger.Info(string.Format("Send Message :{0}", Encoding.UTF8.GetString(commandContent)));

                        }
                        else
                        {
                            LoggerManager.Logger.Info(suiteStatusInfo.VehicleId + " is not online waitSendCommand");
                            saveSendUpModel.Status = (int)CommandSendStatus.Wait;
                            WaitSendCommand waiteSendCommand = ToWaitSendEntity(saveSendUpModel);
                            waiteSendCommand.TimeoutAction = WaitingTimeout;
                            WaitSendManager.AddWaitSendCommand(waiteSendCommand);
                        }
                        lstSaveGpsSendUpModel.Add(saveSendUpModel);
                    }
                }
            }
            StartUpTask(guid);
        }
        static void UpdateRuleRelation(string mdvrSn, string newruleId)
        {
            if (dicRuleResultInfo.Keys.Contains(mdvrSn))
            {
                dicRuleResultInfo[mdvrSn] = newruleId;
            }
            else
            {
                dicRuleResultInfo.Add(mdvrSn, newruleId);
            }
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
        //            SuiteStatusInfo suiteStatusInfo = lstSuiteStatusInfo.Find(c => c.MdvrCoreId.Equals(mdvrid));
        //            SaveSendUpModel saveSendUpModel;
        //            byte[] commandContent;
        //            string routeKey = string.Empty;
        //            foreach (string commandType in lstCommand)
        //            {
        //                if (commandType == CommandType.C80)
        //                {
        //                    SettingOneKeyAlarmCMD Setc80CMD = SetC80CMD(mdvrid, guid, ruleDeleteModel.RuleId);
        //                    saveSendUpModel = GetSaveOneKeyAlarmModel(Setc80CMD, suiteStatusInfo.VehicleId, CommandSendStatus.Sending, commandType);
        //                    commandContent = System.Text.UTF8Encoding.UTF8.GetBytes(Setc80CMD.ToString());
        //                    routeKey = string.Format("{0}{1}", MonitorRoute.HandleOneKeyAlarmKey, mdvrid);
        //                }
        //                else
        //                {
        //                    SettingDelayAlarmCMD Setc78CMD = SetC78CMD(mdvrid, guid, ruleDeleteModel.RuleId);
        //                    saveSendUpModel = GetSaveDelayAlarmModel(Setc78CMD, suiteStatusInfo.VehicleId, CommandSendStatus.Sending, commandType);
        //                    commandContent = System.Text.UTF8Encoding.UTF8.GetBytes(Setc78CMD.ToString());
        //                    routeKey = string.Format("{0}{1}", MonitorRoute.HandleDelayAlarmKey, mdvrid);
        //                }
        //                if (suiteStatusInfo.OnlineFlag)
        //                {
        //                    LoggerManager.Logger.Info(suiteStatusInfo.VehicleId + " is online");
        //                    SendingCommand sendingCommand = ToSendingEntity(saveSendUpModel);
        //                    sendingCommand.TimeoutAction = ReplyTimeout;
        //                    SendingManager.AddSendCommmand(sendingCommand);
        //                    CommandManager.PublishMessage(Gsafety.MQ.Constdefine.MDVREXCHANGE, routeKey, commandContent);
        //                }
        //                else
        //                {
        //                    saveSendUpModel.Status = (int)CommandSendStatus.Wait;
        //                    WaitSendCommand waiteSendCommand = ToWaitSendEntity(saveSendUpModel);
        //                    waiteSendCommand.TimeoutAction = WaitingTimeout;
        //                    WaitSendManager.AddWaitSendCommand(waiteSendCommand);
        //                }
        //                lstSaveGpsSendUpModel.Add(saveSendUpModel);
        //            }
        //        }
        //    }
        //    StartUpTask(guid);
        //}
        /// <summary>
        /// C80
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        public static void CommandReply(byte[] bytes, string key)
        {
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            LoggerManager.Logger.Info("reply command is :" + str);
            C80Reply c80Replay = new C80Reply(str);
            SendingCommand sendingCommand = SendingManager.GetSendCommand(c80Replay.AssociationSetID, CommandType.C80, c80Replay.MdvrCoreId);
            if (sendingCommand != null)
            {
                lock (lockobj)
                {
                    if (c80Replay.ReplyResult == 1)
                    {
                        sendingCommand.commandSendStatus = CommandSendStatus.Success;
                        //sendingCommand.OperateType = c80Replay.Cmd;
                        sendingCommand.OperateType = c80Replay.OperType;

                        AddSuccessCommand(sendingCommand);
                    }
                    else if (c80Replay.ReplyResult == 0)
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

        /// <summary>
        /// C78
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        public static void CommandC78Reply(byte[] bytes, string key)
        {
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            LoggerManager.Logger.Info("reply command is :" + str);
            C78Reply c78Replay = new C78Reply(str);
            SendingCommand sendingCommand = SendingManager.GetSendCommand(c78Replay.AssociationSetID, CommandType.C78, c78Replay.MdvrCoreId);
            if (sendingCommand != null)
            {
                lock (lockobj)
                {
                    if (c78Replay.ReplyResult == 1)
                    {
                        sendingCommand.commandSendStatus = CommandSendStatus.Success;
                        sendingCommand.OperateType = c78Replay.OperType;
                        AddSuccessCommand(sendingCommand);
                    }
                    else if (c78Replay.ReplyResult == 0)
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

        private static void AddSuccessCommand(SendingCommand sendingCommand)
        {
            if (!dicSuccessCommand.ContainsKey(sendingCommand.OperationID))
                dicSuccessCommand.Add(sendingCommand.OperationID, new List<SendingCommand>() { sendingCommand });
            else
                dicSuccessCommand[sendingCommand.OperationID].Add(sendingCommand);
        }

        private static void GetRuleIDSuccess()
        {
            while (true)
            {
                lock (lockobj)
                {
                    if (dicSuccessCommand.Count > 0)
                    {
                        List<string> lstRemove = new List<string>();
                        foreach (KeyValuePair<string, List<SendingCommand>> item in dicSuccessCommand)
                        {
                            if (item.Value.Count == 2)
                            {
                                RuleHelper ruleHelper = ToRuleHelperEntity(item.Value[0].DeviceID, item.Value[0].RuleID, alarmCommandRepository);
                                lstSuccessRuleID.Add(ruleHelper);
                                lstRemove.Add(item.Key);
                                if (item.Value[0].OperateType.Equals("0"))
                                {
                                    AddRuleResultInfo(item.Value[0].DeviceID, item.Value[0].RuleID, dicRuleResultInfo);
                                }
                                else
                                {
                                    RemoveRuleResultInfo(item.Value[0].DeviceID, dicRuleResultInfo);
                                }
                            }
                        }
                        if (lstRemove.Count > 0)
                        {
                            foreach (string id in lstRemove)
                            {
                                dicSuccessCommand.Remove(id);
                            }
                        }
                    }
                }
            }
        }

        private static SaveSendUpModel GetSaveOneKeyAlarmModel(SettingOneKeyAlarmCMD Setting, string vehicleid, CommandSendStatus status, string commandType)
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
            saveGpsSendUpModel.Cmd_Route = string.Format("{0}{1}", MonitorRoute.HandleOneKeyAlarmKey, Setting.DvId);
            saveGpsSendUpModel.Cmd_Type = commandType;
            saveGpsSendUpModel.RuleID = Setting.RuleName;
            saveGpsSendUpModel.UserName = Setting.UserName;
            return saveGpsSendUpModel;
        }

        private static SaveSendUpModel GetSaveDelayAlarmModel(SettingDelayAlarmCMD Setting, string vehicleid, CommandSendStatus status, string commandType)
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
            saveGpsSendUpModel.Cmd_Route = string.Format("{0}{1}", MonitorRoute.HandleDelayAlarmKey, Setting.DvId);
            saveGpsSendUpModel.Cmd_Type = commandType;
            saveGpsSendUpModel.RuleID = Setting.RuleID;
            saveGpsSendUpModel.UserName = Setting.UserName;
            return saveGpsSendUpModel;
        }

        private static SettingOneKeyAlarmCMD SetC80CMD(string mdvrid, string guid, string ruleID)
        {
            SettingOneKeyAlarmCMD cmd = new SettingOneKeyAlarmCMD();
            cmd.DvId = mdvrid;
            cmd.CmType = CommandType.C80;
            cmd.RuleName = ruleID;
            cmd.MsgId = guid;
            cmd.SendValue = 2;
            cmd.SendTime = DateTime.Now;
            return cmd;
        }

        private static SettingDelayAlarmCMD SetC78CMD(string mdvrid, string guid, string ruleID)
        {
            SettingDelayAlarmCMD cmd = new SettingDelayAlarmCMD();
            cmd.DvId = mdvrid;
            cmd.CmType = CommandType.C78;
            cmd.RuleName = ruleID;
            cmd.MsgId = guid;
            cmd.SendValue = 2;
            cmd.OneKeyDelayTime = 15;
            cmd.OverSpeedTime = 15;
            cmd.SendTime = DateTime.Now;
            return cmd;
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
