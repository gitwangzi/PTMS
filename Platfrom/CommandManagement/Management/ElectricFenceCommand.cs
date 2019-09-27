/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: b9afbc79-46fd-47f9-8183-61b48ad9a8cf      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CommandManagement.Management
/////    Project Description:    
/////             Class Name: ElectricFenceCommand
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/1 10:46:00
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/1 10:46:00
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
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.Common.Logging;
using Gsafety.PTMS.Command.Repository;
using Gsafety.PTMS.Traffic.Repository;
using Gsafety.MQ;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.CommandManagement
{
    public class  ElectricFenceCommand : BasicInfoManager
    {
        static FenceCommandRepository fenceCommandRepository;
        static Dictionary<string, List<string>> dicExistMdvrID;
        static Dictionary<string, List<string>> dicfenceID;
        static Dictionary<string, List<string>> dicDeleteList;
        private static TrafficRepository _trafficRepository;
        static ElectricFenceCommand()
        {
            try
            {
                _trafficRepository = new TrafficRepository();
                fenceCommandRepository = new FenceCommandRepository();
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("Gsafety.PTMS.CommandManagement.static ElectricFenceCommand()", ex);
            }
        }
        public static void CommandSend(PTMSEntities context, byte[] bytes, string key)
        {
            try
            {
                dicExistMdvrID = fenceCommandRepository.GetDicFenceID(context);
                dicfenceID = fenceCommandRepository.GetFenceList(context);              
                var model = ConvertHelper.BytesToObject(bytes) as ElectircFenceSendSettingModel;
                string routeKey = key.Replace(MonitorRoute.OriginalFenceKey, MonitorRoute.HandleFenceKey);
                string guid = model.Setting.FenceId.ToString();
                model.Setting.MsgId = guid;
                List<string> lstMdvrs = new List<string>();
                switch ((int)model.ElectricFenceOperation)
                {
                    case 1:
                        lstMdvrs = CheckFenceIsExist(context,model.Value, guid);
                        break;
                    case 2:
                        lstMdvrs = GetFenceList(guid, model.Setting.DvId, dicfenceID);
                        break;
                    case 3:
                        if (fenceCommandRepository.DeleteFenceList(context,guid))
                        {
                            dicDeleteList = fenceCommandRepository.GetFenceList(context);
                            lstMdvrs = GetFenceList(guid, model.Setting.DvId, dicDeleteList);
                        }
                        break;
                }              
                if (lstMdvrs != null && lstMdvrs.Count > 0)
                {
                    foreach (string mdvrid in lstMdvrs)
                    {
                        if (model.ElectricFenceOperation == ElectricFenceOperType.Add)
                        {
                            bool isRepeatCommand = JudgeRepeatFenceCommand(mdvrid, model.Setting.FenceId.ToString());
                            if (isRepeatCommand)
                            {
                                LoggerManager.Logger.Info("Repeat Command Message No Send");
                                continue;
                            }
                        }

                        SaveSendUpModel saveGpsSendUpModel;
                        model.Setting.DvId = mdvrid;
                        LoggerManager.Logger.Info("MDVRID IN lstMdvrs" + mdvrid);
                        SuiteStatusInfo suiteStatusInfo = lstSuiteStatusInfo.Find(c => c.MdvrCoreId.Equals(mdvrid));
                        LoggerManager.Logger.Info("SuiteStatusInfo" + suiteStatusInfo);
                        saveGpsSendUpModel = GetSaveGpsSendUpModel(model, suiteStatusInfo.VehicleId, CommandSendStatus.Sending);
                        LoggerManager.Logger.Info("SaveSendUpModel" + saveGpsSendUpModel);
                        fenceCommandRepository.AddFenceInfo(context,ToSendingEntity(saveGpsSendUpModel));
                        LoggerManager.Logger.Info("After Update to Vehicle_Fence Record" + saveGpsSendUpModel);
                        _trafficRepository.AddElectricFenceSetup(context,model.Setting);
                        LoggerManager.Logger.Info("After Add to ElectricFence Record" + saveGpsSendUpModel);
                        string recordID = fenceCommandRepository.AddSendCommand(context,model.Setting, suiteStatusInfo.VehicleId, Constdefine.MDVREXCHANGE, routeKey, suiteStatusInfo.OnlineFlag ? CommandSendStatus.Sending : CommandSendStatus.Wait);
                        LoggerManager.Logger.Info("After Add to Send_Record" + saveGpsSendUpModel);
                        if (suiteStatusInfo.OnlineFlag)
                        {
                            string cmdstringValue = model.Setting.ToString(model.ElectricFenceOperation);
                            LoggerManager.Logger.Info(string.Format("{0}:{1}", "PublishMessage Type SendElectricFence ", cmdstringValue));
                            var commandContent = System.Text.UTF8Encoding.UTF8.GetBytes(cmdstringValue);
                            SendingCommand sendingCommand = ToSendingEntity(saveGpsSendUpModel);
                            sendingCommand.RecordID = recordID;
                            SendingManager.AddSendCommmand(sendingCommand);
                            CommandManager.PublishMessage(Gsafety.MQ.Constdefine.MDVREXCHANGE, string.Format("{0}{1}", MonitorRoute.HandleFenceKey, mdvrid), commandContent);
                        }
                        else
                        {
                            saveGpsSendUpModel.Status = (int)CommandSendStatus.Wait;
                            WaitSendCommand waiteSendCommand = ToWaitSendEntity(saveGpsSendUpModel);
                            waiteSendCommand.RouteKey = routeKey;
                            waiteSendCommand.Exchange = Constdefine.MDVREXCHANGE;
                            waiteSendCommand.RecordID = recordID;
                            WaitSendManager.AddWaitSendCommand(waiteSendCommand);
                        }
                    }
                }
                else
                {
                    LoggerManager.Logger.Info("lstMdvrs Count =0");
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("ElectricFenceCommand CommandSend" + ex);
            }
        }

        public static void CommandReply(PTMSEntities context, byte[] bytes, string key)
        {
            try
            {
                string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
                LoggerManager.Logger.Info(string.Format("{0}:{1}", "Receive Message : Type ElectricFenceReply,Value:", str));
                ElectricFenceReply fenceReplay = new ElectricFenceReply(str);
                SendingCommand sendingCommand = SendingManager.GetSendCommand(fenceReplay.AssociationSetID.Split(':')[0], CommandType.PolygonsRegion, fenceReplay.MdvrCoreId);
                if (sendingCommand != null)
                {
                    //if (_trafficRepository.UpdateElectricFenceResult(context,fenceReplay) >= 0)
                    {
                        if (fenceReplay.ReplyResult == 1)
                        {
                            sendingCommand.commandSendStatus = CommandSendStatus.Success;
                            if (fenceReplay.OperType.Equals("0"))
                            {
                                AddFenceID(sendingCommand.DeviceID, sendingCommand.OperationID);
                            }
                            else
                            {
                                RemoveFenceID(sendingCommand.DeviceID, sendingCommand.OperationID);
                            }
                        }
                        SendingManager.RemoveSendCommmand(sendingCommand);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("ElectricFenceCommand CommandReply" + ex);
            }
        }
        private static List<string> CheckFenceIsExist(PTMSEntities context, List<SelectInfoModel> value, string FenceId)
        {
            List<string> lstMdvrs = new List<string>();
            lstMdvrs = GetLstMdvrs(context,value);
            LoggerManager.Logger.Info("GetLstMdvrs" + lstMdvrs);
            for (int i = lstMdvrs.Count - 1; i >= 0; i--)  //modify by zzg 2015.6.13
            {
                if (dicExistMdvrID.ContainsKey(lstMdvrs[i]))
                {
                    if (dicExistMdvrID[lstMdvrs[i]].Contains(FenceId))
                    {
                        LoggerManager.Logger.Info(string.Format("Fence Is Exist：[OperationType: Add, Mdvr sn :{0},Fence ID :{1} .]not send command", lstMdvrs[i], FenceId));
                        lstMdvrs.Remove(lstMdvrs[i]);
                    }
                }

            }
            return lstMdvrs;
        }

        private static SaveSendUpModel GetSaveGpsSendUpModel(ElectircFenceSendSettingModel model, string vehicleid, CommandSendStatus status)
        {
            SaveSendUpModel saveGpsSendUpModel = new SaveSendUpModel();
            ElectricFenceCMD Setting = new ElectricFenceCMD();
            Setting = model.Setting;
            saveGpsSendUpModel.ID = Guid.NewGuid().ToString();
            saveGpsSendUpModel.Mdvr_core_sn = Setting.DvId;
            saveGpsSendUpModel.Operation_id = Setting.MsgId;
            saveGpsSendUpModel.Vehicle_id = vehicleid;
            saveGpsSendUpModel.Send_Time = Setting.SendTime;
            saveGpsSendUpModel.Create_Time = Setting.SendTime;
            saveGpsSendUpModel.Status = (int)status;
            saveGpsSendUpModel.Cmd_content = System.Text.UTF8Encoding.UTF8.GetBytes(Setting.ToString((ElectricFenceOperType)Setting.OperType));
            saveGpsSendUpModel.Cmd_Exchange = Gsafety.MQ.Constdefine.MDVREXCHANGE;
            saveGpsSendUpModel.Cmd_Route = string.Format("{0}{1}", MonitorRoute.HandleFenceKey, Setting.DvId);
            saveGpsSendUpModel.Cmd_Type = CommandType.PolygonsRegion;
            saveGpsSendUpModel.RuleID = Setting.RuleName;
            saveGpsSendUpModel.UserName = Setting.UserName;
            saveGpsSendUpModel.Cmd_Sub_Type = ((int)model.ElectricFenceOperation).ToString();
            return saveGpsSendUpModel;
        }

        private static void AddFenceID(string mdvrID, string fenceID)
        {
            if (dicExistMdvrID.ContainsKey(mdvrID))
            {
                dicExistMdvrID[mdvrID].Add(fenceID);
            }
            else
            {
                dicExistMdvrID.Add(mdvrID, new List<string>() { fenceID });
            }
        }

        private static void RemoveFenceID(string mdvrID, string fenceID)
        {
            if (dicExistMdvrID.ContainsKey(mdvrID))
            {
                dicExistMdvrID[mdvrID].Remove(fenceID);
            }
        }

        public static void WaitingTimeout(PTMSEntities context,WaitSendCommand waiteSendCommand)
        {
            fenceCommandRepository.UpdateFenceStatusTimeout(context,waiteSendCommand.OperationID, waiteSendCommand.DeviceID);
            ElectricFenceReply fenceReply = CreateFenceReply(waiteSendCommand.OperationID, waiteSendCommand.DeviceID, false);
            LoggerManager.Logger.Info("WaitingTimeout  Before Publishing " + waiteSendCommand.RecordID);
            CommandManager.PublishMessage(Constdefine.MDVREXCHANGE, string.Format("{0}.*.*.*", MonitorRoute.HandleFenceReplyKey), ConvertHelper.ObjectToBytes(fenceReply));
        }

        public static void ReplyTimeout(PTMSEntities context,SendingCommand sendingCommand)
        {
            fenceCommandRepository.UpdateFenceStatusTimeout(context,sendingCommand.OperationID, sendingCommand.DeviceID);
            ElectricFenceReply fenceReply = CreateFenceReply(sendingCommand.OperationID, sendingCommand.DeviceID, false);
            LoggerManager.Logger.Info("ReplyTimeout  Before Publishing " + sendingCommand.RecordID);
            CommandManager.PublishMessage(Constdefine.MDVREXCHANGE, string.Format("{0}.*.*.*", MonitorRoute.HandleFenceReplyKey), ConvertHelper.ObjectToBytes(fenceReply));
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
            sendingCommand.SendingTimeout = sendingCommand.SendingTime.AddSeconds(ConfigInfo.TrafficReplyTimeout);
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

        private static ElectricFenceReply CreateFenceReply(string finceID, string mdvrSN, bool isSuccess)
        {
            ElectricFenceReply feneceReply = new ElectricFenceReply();
            feneceReply.MdvrCoreId = mdvrSN;
            feneceReply.AssociationSetID = finceID;
            feneceReply.ReplyResult = isSuccess ? 1 : 0;
            return feneceReply;
        }

        private static bool JudgeRepeatFenceCommand(string mdvrid, string FenceID)
        {
            List<string> FenceList = new List<string>();
            bool IsHandMdvrid = dicfenceID.TryGetValue(mdvrid, out FenceList);
            if (IsHandMdvrid)
            {
                foreach (var rule in FenceList)
                {
                    if (rule.Equals(FenceID))
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

    }
}
