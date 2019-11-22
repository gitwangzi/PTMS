/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: c4d4639c-4a32-471b-838d-eafc18588f81      
/////             clrversion: 4.0.30319.34011
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CommandManagement.Management
/////    Project Description:    
/////             Class Name: OverSpeedCommandManager
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/3/13 04:23:07
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/3/13 04:23:07
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Command.Repository;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.MQ;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.CommandManagement
{
    /// <summary>
    /// speeding command management
    /// </summary>
    public class OverSpeedCommandManager
    {
        static OverSpeedCommandRepository _overspeedCommandRepository;

        static OverSpeedCommandManager()
        {
            _overspeedCommandRepository = new OverSpeedCommandRepository();
        }
        /// <summary>
        /// speeding setitings
        /// if the device is not online,command is not issued
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        public static void CommandSend(PTMSEntities context,byte[] bytes, string key)
        {
            var overSpeedCMD = ConvertHelper.BytesToObject(bytes) as SettingOverSpeedCMD;
            overSpeedCMD.CmType = CommandType.OverSpeed;
            overSpeedCMD.SendTime = DateTime.Now;
            SuiteStatusInfo suiteStatusInfo = SuiteStatusInfoManage.GetSuiteStatusInfo(overSpeedCMD.DvId);
            string routeKey = key.Replace(MonitorRoute.OriginalSettingOverSpeedCMDKey, MonitorRoute.HandleSettingOverSpeedCMDKey);
            var commandContent = System.Text.UTF8Encoding.UTF8.GetBytes(overSpeedCMD.ToString()); 
            if (commandContent == null || commandContent.Length == 0)
            {
                LoggerManager.Logger.Warn(string.Format("The device information does not exist,MDVR_Core_ID:{0}", overSpeedCMD.DvId));
                return;
            }
            if (suiteStatusInfo == null)
            {
                LoggerManager.Logger.Warn(string.Format("The device information does not exist,MDVR_Core_ID:{0}", overSpeedCMD.DvId));
                return;
            }
            else
            {
                string recordID = _overspeedCommandRepository.AddSendCommand(context,overSpeedCMD, suiteStatusInfo.VehicleId, Constdefine.MDVREXCHANGE, routeKey, suiteStatusInfo.OnlineFlag ? CommandSendStatus.Sending : CommandSendStatus.Wait);
                ///////record is empty,need to add processing;   
                if (suiteStatusInfo.OnlineFlag)
                {
                    SendingCommand sendingCommand = ToSendingEntity(overSpeedCMD);
                    sendingCommand.RecordID = recordID;
                    SendingManager.AddSendCommmand(sendingCommand);
                    CommandManager.PublishMessage(Constdefine.MDVREXCHANGE, routeKey, commandContent);
                    LoggerManager.Logger.Info(string.Format("Overspeed sending issued:{0},waiting for reply", overSpeedCMD.ToString()));
                }
                else
                {
                    WaitSendCommand waiteSendCommand = ToWaitSendEntity(overSpeedCMD);
                    waiteSendCommand.RouteKey = routeKey;
                    waiteSendCommand.Exchange = Constdefine.MDVREXCHANGE;
                    waiteSendCommand.RecordID = recordID;
                    WaitSendManager.AddWaitSendCommand(waiteSendCommand);
                    LoggerManager.Logger.Info(string.Format("Overspeed add to waiting queue {0}", overSpeedCMD.ToString()));
                }
            }
        }

        /// <summary>
        /// reply speeding set
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        public static void CommandReply(PTMSEntities context,byte[] bytes, string key)
        {
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            SettingOverSpeedReply overSpeedReplay = new SettingOverSpeedReply(str);
            SendingCommand sendingCommand = SendingManager.GetSendCommand(overSpeedReplay.MdvrCoreId, overSpeedReplay.OriginalTime, CommandType.OverSpeed);
            overSpeedReplay.AssociationSetID = sendingCommand.OperationID;
            if (sendingCommand != null)
            {
                _overspeedCommandRepository.UpdateSendResult(context,overSpeedReplay, sendingCommand.RecordID);
                SendingManager.RemoveSendCommmand(sendingCommand);
            }
            else
            {
                _overspeedCommandRepository.UpdateSendResult(context,overSpeedReplay, string.Empty);
               
            }

            CommandManager.PublishMessage(Constdefine.MDVREXCHANGE, string.Format("{0}.*.*.*", key.Replace(ReplyRoute.OriginalSettingOverSpeedReplyKey, MonitorRoute.HandleSettingOverSpeedReplyKey)), ConvertHelper.ObjectToBytes(overSpeedReplay));
            LoggerManager.Logger.Info(string.Format("Set Speeding Reply,CMD context:{0}", str));
        }

        /// <summary>
        /// timeout waiting to send commands
        /// </summary>
        /// <param name="waiteSendCommand"></param>
        public static void WaitingTimeout(PTMSEntities context,WaitSendCommand waiteSendCommand)
        {
            _overspeedCommandRepository.UpdateStatusTimeout(context,waiteSendCommand.RecordID, waiteSendCommand.DeviceID, waiteSendCommand.OperateType);
            SettingOverSpeedReply overSpeedReply = CreateOverSpeedReply(waiteSendCommand.OperationID, waiteSendCommand.DeviceID,false);
            CommandManager.PublishMessage(Constdefine.MDVREXCHANGE, string.Format("{0}.*.*.*", MonitorRoute.HandleSettingOverSpeedReplyKey), ConvertHelper.ObjectToBytes(overSpeedReply));
        }


        /// <summary>
        /// timeout waiting for a reply after sending the command
        /// </summary>
        /// <param name="sendingCommand"></param>
        public static void ReplyTimeout(PTMSEntities context,SendingCommand sendingCommand)
        {
            _overspeedCommandRepository.UpdateStatusTimeout(context,sendingCommand.RecordID, sendingCommand.DeviceID, sendingCommand.OperateType);
            SettingOverSpeedReply overSpeedReply = CreateOverSpeedReply(sendingCommand.OperationID, sendingCommand.DeviceID,false);
            CommandManager.PublishMessage(Constdefine.MDVREXCHANGE, string.Format("{0}.*.*.*", MonitorRoute.HandleSettingOverSpeedReplyKey), ConvertHelper.ObjectToBytes(overSpeedReply));
        }


        private static WaitSendCommand ToWaitSendEntity(SettingOverSpeedCMD overSpeedCMD)
        {
            WaitSendCommand waiteSendCommand = new WaitSendCommand();
            waiteSendCommand.CommandType = overSpeedCMD.CmType;
            waiteSendCommand.OperateType = overSpeedCMD.OperType.ToString();
            waiteSendCommand.DeviceID = overSpeedCMD.DvId;
            waiteSendCommand.OperationID = overSpeedCMD.OverSpeedID;
            waiteSendCommand.CommandContent = System.Text.UTF8Encoding.UTF8.GetBytes(overSpeedCMD.ToString());
            waiteSendCommand.Exchange = Constdefine.MDVREXCHANGE;
            waiteSendCommand.RequestSendTime = overSpeedCMD.SendTime;
            waiteSendCommand.RequestTimeout = waiteSendCommand.RequestSendTime.AddSeconds(ConfigInfo.TrafficWaitTimeout);
            waiteSendCommand.TimeoutAction = WaitingTimeout;
            return waiteSendCommand;
        }

        private static SendingCommand ToSendingEntity(SettingOverSpeedCMD overSpeedCMD)
        {
            SendingCommand sendingCommand = new SendingCommand();
            sendingCommand.CommandType = overSpeedCMD.CmType;
            sendingCommand.OperateType = overSpeedCMD.OperType.ToString();
            sendingCommand.DeviceID = overSpeedCMD.DvId;
            sendingCommand.OperationID = overSpeedCMD.OverSpeedID;
            sendingCommand.RequestTime = overSpeedCMD.SendTime;
            sendingCommand.SendingTime = DateTime.Now;
            sendingCommand.SendingTimeout = sendingCommand.SendingTime.AddSeconds(ConfigInfo.TrafficReplyTimeout);
            sendingCommand.TimeoutAction = ReplyTimeout;
            return sendingCommand;
        }

        private static SettingOverSpeedReply CreateOverSpeedReply(string overSpeedId,string mdvrSN, bool isSuccess)
        {
            SettingOverSpeedReply overspeedReply = new SettingOverSpeedReply();
            overspeedReply.MdvrCoreId = mdvrSN;
            overspeedReply.AssociationSetID = overSpeedId;
            overspeedReply.ReplyResult = isSuccess ? 1 : 0;
            return overspeedReply;
        }
    }
}
