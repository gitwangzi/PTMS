
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2b11e103-ff5b-4bd4-a45e-9903a83ac7fb      
/////             clrversion: 4.0.30319.34011
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CommandManagement
/////    Project Description:    
/////             Class Name: FenceCommandManager
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/3/12 03:37:54
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/3/12 03:37:54
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Command.Repository;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.MQ;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Traffic.Repository;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.CommandManagement
{
    public class FenceCommandManager
    {
        private static FenceCommandRepository _fenceCommandRepository;
        private static TrafficRepository _trafficRepository;

        static FenceCommandManager()
        {
            _fenceCommandRepository = new FenceCommandRepository();
            _trafficRepository = new TrafficRepository();
        }


        /// <summary>
        /// send electronic fence message
        /// if the device is not online,command is not issued
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        public static void CommandSend(PTMSEntities context, byte[] bytes, string key)
        {
            var fenceCMD = ConvertHelper.BytesToObject(bytes) as ElectricFenceCMD;
            fenceCMD.CmType = CommandType.PolygonsRegion;
            fenceCMD.SendTime = DateTime.Now;
            fenceCMD.MsgId = fenceCMD.FenceId.ToString();
            SuiteStatusInfo suiteStatusInfo = SuiteStatusInfoManage.GetSuiteStatusInfo(fenceCMD.DvId);
            string routeKey = key.Replace(MonitorRoute.OriginalFenceKey, MonitorRoute.HandleFenceKey);
            var commandContent = System.Text.UTF8Encoding.UTF8.GetBytes(fenceCMD.ToString(ElectricFenceOperType.Modify));
            var model = ConvertHelper.BytesToObject(bytes) as ElectricFenceCMD;
            if (commandContent == null || commandContent.Length == 0)
            {
                LoggerManager.Logger.Warn(string.Format("The device information does not exist,MDVR_Core_ID:{0}", fenceCMD.DvId));
                return;
            }
            if (suiteStatusInfo == null)
            {
                LoggerManager.Logger.Warn(string.Format("The device information does not exist,MDVR_Core_ID:{0}", fenceCMD.DvId));
                return;
            }
            else
            {
                _trafficRepository.AddElectricFenceSetup(context, fenceCMD);
                string recordID = _fenceCommandRepository.AddSendCommand(context, fenceCMD, suiteStatusInfo.VehicleId, Constdefine.MDVREXCHANGE, routeKey, suiteStatusInfo.OnlineFlag ? CommandSendStatus.Sending : CommandSendStatus.Wait);
                if ((fenceCMD.AreaType == AreaType.ElectronicFence && ConfigInfo.FenceToDevice && !suiteStatusInfo.OnlineFlag)
                    || fenceCMD.AreaType == AreaType.MonitoringPoint && ConfigInfo.PointToDevice && !suiteStatusInfo.OnlineFlag)
                {
                    WaitSendCommand waiteSendCommand = ToWaitSendEntity(fenceCMD);
                    waiteSendCommand.RouteKey = routeKey;
                    waiteSendCommand.Exchange = Constdefine.MDVREXCHANGE;
                    waiteSendCommand.RecordID = recordID;
                    WaitSendManager.AddWaitSendCommand(waiteSendCommand);
                    LoggerManager.Logger.Info(string.Format("Electronic fence waiting issued:{0}", fenceCMD.ToString((ElectricFenceOperType)fenceCMD.OperType)));
                }
                else
                {
                    SendingCommand sendingCommand = ToSendingEntity(fenceCMD);
                    sendingCommand.RecordID = recordID;
                    SendingManager.AddSendCommmand(sendingCommand);
                    if (fenceCMD.AreaType == AreaType.ElectronicFence)
                    {
                        CommandManager.PublishMessage(Constdefine.MDVREXCHANGE, routeKey, commandContent);
                        CommandManager.PublishMessage(Constdefine.APPEXCHANGE, UserMessageRoute.OriginalElectronicFenceKey, ConvertHelper.ObjectToBytes(fenceCMD));
                    }
                    else
                    {
                        if (ConfigInfo.PointToDevice)
                        {
                            CommandManager.PublishMessage(Constdefine.MDVREXCHANGE, routeKey, commandContent);
                        }
                        else
                        {
                            CommandManager.PublishMessage(Constdefine.APPEXCHANGE, UserMessageRoute.OriginalMonitorPointKey, ConvertHelper.ObjectToBytes(fenceCMD));
                        }
                    }
                    LoggerManager.Logger.Info(string.Format("Electronic fence has been issued, waiting for reply:{0}", fenceCMD.ToString((ElectricFenceOperType)fenceCMD.OperType)));
                }
            }
        }


        /// <summary>
        /// electrion fence reply
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        public static void CommandReply(PTMSEntities context, byte[] bytes, string key, TrafficReplySource replySource)
        {
            ElectricFenceReply fenceReplay = null;
            if (replySource == TrafficReplySource.Device)
            {
                string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
                LoggerManager.Logger.Info(string.Format("Electronic Fence Set Reply(Device),CMD context:{0}", str));
                fenceReplay = new ElectricFenceReply(str);
            }
            else
            {
                fenceReplay = ConvertHelper.BytesToObject(bytes) as ElectricFenceReply;
                LoggerManager.Logger.Info(string.Format("Electronic Fence Set Reply(Monitor),CMD context:MDVR_CORE_SN={0},Setting Time={1}", fenceReplay.MdvrCoreId, fenceReplay.OriginalTime));
            }
            SendingCommand sendingCommand = SendingManager.GetSendCommand(fenceReplay.MdvrCoreId, fenceReplay.OriginalTime, CommandType.PolygonsRegion);

            if (sendingCommand != null)
            {
                fenceReplay.AssociationSetID = sendingCommand.OperationID;
                if (sendingCommand != null && !string.IsNullOrEmpty(sendingCommand.OperationID))
                {
                    fenceReplay.AreaType = _trafficRepository.GetFenceType(decimal.Parse(sendingCommand.OperationID));
                }
            }

            if (fenceReplay.AreaType == AreaType.MonitoringPoint)
            {
                if (ConfigInfo.PointToDevice && replySource == TrafficReplySource.MoitorAlert ||
                  !ConfigInfo.PointToDevice && replySource == TrafficReplySource.Device)
                    return;
            }
            else
            {
                if (ConfigInfo.FenceToDevice && replySource == TrafficReplySource.MoitorAlert ||
                  !ConfigInfo.FenceToDevice && replySource == TrafficReplySource.Device)
                    return;
            }

            if (sendingCommand != null)
            {
                _fenceCommandRepository.UpdateSendResult(context, fenceReplay, sendingCommand.RecordID, sendingCommand.OperationID);
                //SendingManager.RemoveSendCommmand(sendingCommand);
            }
            if (replySource == TrafficReplySource.Device)
            {
                CommandManager.PublishMessage(Constdefine.MDVREXCHANGE, string.Format("{0}.*.*.*", key.Replace(ReplyRoute.OriginalFenceyDeviceReplyKey, MonitorRoute.HandleFenceReplyKey)), ConvertHelper.ObjectToBytes(fenceReplay));
            }
            else
            {
                CommandManager.PublishMessage(Constdefine.MDVREXCHANGE, string.Format("{0}.*.*.*", key.Replace(ReplyRoute.OriginalFenceyBusinessReplyKey, MonitorRoute.HandleFenceReplyKey)), ConvertHelper.ObjectToBytes(fenceReplay));
            }

        }

        /// <summary>
        /// timeout waiting to send commands
        /// </summary>
        /// <param name="waiteSendCommand"></param>
        public static void WaitingTimeout(PTMSEntities context, WaitSendCommand waiteSendCommand)
        {
            try
            {
                _fenceCommandRepository.UpdateStatusTimeout(context, waiteSendCommand.RecordID, waiteSendCommand.DeviceID, waiteSendCommand.OperationID, waiteSendCommand.OperateType);
                ElectricFenceReply fenceReply = CreateFenceReply(waiteSendCommand.OperationID, waiteSendCommand.DeviceID, false);
                CommandManager.PublishMessage(Constdefine.MDVREXCHANGE, string.Format("{0}.*.*.*", MonitorRoute.HandleFenceReplyKey), ConvertHelper.ObjectToBytes(fenceReply));
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }


        /// <summary>
        /// waiting for a reply after sending the command
        /// </summary>
        /// <param name="sendingCommand"></param>
        public static void ReplyTimeout(PTMSEntities context,SendingCommand sendingCommand)
        {
            try
            {
                _fenceCommandRepository.UpdateStatusTimeout(context,sendingCommand.RecordID, sendingCommand.DeviceID, sendingCommand.OperationID, sendingCommand.OperateType);
                ElectricFenceReply fenceReply = CreateFenceReply(sendingCommand.OperationID, sendingCommand.DeviceID, false);
                CommandManager.PublishMessage(Constdefine.MDVREXCHANGE, string.Format("{0}.*.*.*", MonitorRoute.HandleFenceReplyKey), ConvertHelper.ObjectToBytes(fenceReply));
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }

        }


        private static WaitSendCommand ToWaitSendEntity(ElectricFenceCMD fenceCMD)
        {
            WaitSendCommand waiteSendCommand = new WaitSendCommand();
            waiteSendCommand.CommandType = CommandType.PolygonsRegion;
            waiteSendCommand.OperateType = fenceCMD.OperType.ToString();
            waiteSendCommand.OperationID = fenceCMD.FenceId.ToString();
            waiteSendCommand.DeviceID = fenceCMD.DvId;
            waiteSendCommand.CommandContent = System.Text.UTF8Encoding.UTF8.GetBytes(fenceCMD.ToString());
            waiteSendCommand.Exchange = Constdefine.MDVREXCHANGE;
            waiteSendCommand.RequestSendTime = fenceCMD.SendTime;
            waiteSendCommand.RequestTimeout = waiteSendCommand.RequestSendTime.AddSeconds(ConfigInfo.TrafficWaitTimeout);
            waiteSendCommand.TimeoutAction = WaitingTimeout;
            return waiteSendCommand;
        }

        private static SendingCommand ToSendingEntity(ElectricFenceCMD fenceCMD)
        {
            SendingCommand sendingCommand = new SendingCommand();
            sendingCommand.CommandType = CommandType.PolygonsRegion;
            sendingCommand.OperateType = fenceCMD.OperType.ToString();
            sendingCommand.OperationID = fenceCMD.FenceId.ToString();
            sendingCommand.DeviceID = fenceCMD.DvId;
            sendingCommand.RequestTime = fenceCMD.SendTime;
            sendingCommand.SendingTime = DateTime.Now;
            sendingCommand.SendingTimeout = sendingCommand.SendingTime.AddSeconds(ConfigInfo.TrafficReplyTimeout);
            sendingCommand.TimeoutAction = ReplyTimeout;
            return sendingCommand;
        }

        private static ElectricFenceReply CreateFenceReply(string finceID, string mdvrSN, bool isSuccess)
        {
            ElectricFenceReply feneceReply = new ElectricFenceReply();
            feneceReply.MdvrCoreId = mdvrSN;
            feneceReply.AssociationSetID = finceID;
            feneceReply.ReplyResult = isSuccess ? 1 : 0;
            return feneceReply;
        }
    }
}
