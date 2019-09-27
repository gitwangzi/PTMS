/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 9f739e50-1289-4bb5-a92a-918d75deabda      
/////             clrversion: 4.0.30319.34014
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CommandManagement.Management
/////    Project Description:    
/////             Class Name: C30CommandManager
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/4/13 00:09:21
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/4/13 00:09:21
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
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.Common.Logging;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Base.Contract.Data;

namespace Gsafety.PTMS.CommandManagement
{
    public class C30CommandManager
    {
        /// <summary>
        /// send command（daily monitoring）
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        public static void CommandSend(byte[] bytes, string key)
        {
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            var model = ConvertHelper.BytesToObject(bytes) as LocationMonitorCMD;
            SuiteStatusInfo suiteStatusInfo = SuiteStatusInfoManage.GetSuiteStatusInfo(model.MDVRCoreSN);
            if (suiteStatusInfo.OnlineFlag)
            {
                if (model == null || string.IsNullOrEmpty(model.MDVRCoreSN))
                {
                    LoggerManager.Logger.Warn("Subscribe to daily monitoring data failed MDVR");
                    return;
                }
                C30CMD c30Cmd = CreateC30Cmd(model.MDVRCoreSN);
                var commandContent = System.Text.UTF8Encoding.UTF8.GetBytes(c30Cmd.ToString());
                SendingCommand sendingCommand = ToSendingEntity(model, c30Cmd.SendTime);
                SendingManager.AddSendCommmand(sendingCommand);
                CommandManager.PublishMessage(Gsafety.MQ.Constdefine.MDVREXCHANGE, string.Format("{0}{1}", MonitorRoute.HandleGPSMonitorKey, model.MDVRCoreSN), commandContent);
                LoggerManager.Logger.Info(string.Format("Subscribe to  monitoring GPS data MDVR:MDVR_Core_SN={0},Vechile_ID={1}", model.MDVRCoreSN, model.VechileID));
               
            }
            else
            {
                EndLocationMonitor endLocationMonitor = new EndLocationMonitor();
                endLocationMonitor.VechileID = model.VechileID;
                endLocationMonitor.EndType = LocationMonitorEndType.RequestFails;
                CommandManager.PublishMessage(Gsafety.MQ.Constdefine.APPEXCHANGE, string.Format("{0}{1}", UserMessageRoute.LocationMonitorEndKey, model.SessionID), ConvertHelper.ObjectToBytes(endLocationMonitor));
            }
        }

        public static void CanccenMonitor(byte[] bytes, string key)
        {
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            var model = ConvertHelper.BytesToObject(bytes) as CancelLocationMonitorCMD;
            if (model != null)
            {
                LocationMonitorManager.RemoveSendCommand(model.SessionID, model.MDVRCoreSN);
            }
        }

        public static void CommandReply(byte[] bytes, string key)
        {
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            LoggerManager.Logger.Info(string.Format("Subscribe to  monitoring GPS data Reply:{0}",str));
            C30Reply overSpeedReplay = new C30Reply(str);
            SendingCommand sendingCommand = SendingManager.GetSendCommand(overSpeedReplay.MdvrCoreId, overSpeedReplay.OriginalTime, CommandType.C30);
            if (sendingCommand != null)
            {
                if (overSpeedReplay.ReplyResult == 0)
                {
                    EndLocationMonitor endLocationMonitor = new EndLocationMonitor();
                    endLocationMonitor.VechileID = sendingCommand.RecordID;
                    endLocationMonitor.EndType = LocationMonitorEndType.RequestFails;
                    CommandManager.PublishMessage(Gsafety.MQ.Constdefine.APPEXCHANGE, string.Format("{0}{1}", UserMessageRoute.LocationMonitorEndKey, sendingCommand.OperationID), ConvertHelper.ObjectToBytes(endLocationMonitor));
       
                }
                else if (overSpeedReplay.ReplyResult == 1)
                {
                    sendingCommand.SendingTimeout = sendingCommand.SendingTime.AddSeconds(ConfigInfo.TimeLength);
                    sendingCommand.TimeoutAction = MonitorEnd;
                    LocationMonitorManager.AddSendCommmand(sendingCommand);
                }
                SendingManager.RemoveSendCommmand(sendingCommand);
            }
            
        }

        private static C30CMD CreateC30Cmd(string mdvrId)
        {
            C30CMD c30Cmd = new C30CMD();
            c30Cmd.OpenOrStop = 1;
            c30Cmd.DvId = mdvrId;
            c30Cmd.DistanceInterval = 0;
            c30Cmd.TimeInterval = ConfigInfo.TimeInterval;
            c30Cmd.ReportNumber = ConfigInfo.TimeLength / ConfigInfo.TimeInterval + 1;
            c30Cmd.SendTime = DateTime.Now;
            Random random = new Random(1);
            c30Cmd.MsgId = random.Next(1,65535).ToString();
            return c30Cmd;
        }

        private static void ReplyTimeout(SendingCommand sendingCommand)
        {
            EndLocationMonitor endLocationMonitor = new EndLocationMonitor();
            endLocationMonitor.VechileID = sendingCommand.RecordID;
            endLocationMonitor.EndType = LocationMonitorEndType.RequestTimeout;
            CommandManager.PublishMessage(Gsafety.MQ.Constdefine.APPEXCHANGE, string.Format("{0}{1}", UserMessageRoute.LocationMonitorEndKey, sendingCommand.OperationID), ConvertHelper.ObjectToBytes(endLocationMonitor));
        }

        private static void MonitorEnd(SendingCommand sendingCommand)
        {
            EndLocationMonitor endLocationMonitor = new EndLocationMonitor();
            endLocationMonitor.VechileID = sendingCommand.RecordID;
            endLocationMonitor.EndType = LocationMonitorEndType.MonitorEnd;
            CommandManager.PublishMessage(Gsafety.MQ.Constdefine.APPEXCHANGE, string.Format("{0}{1}", UserMessageRoute.LocationMonitorEndKey, sendingCommand.OperationID), ConvertHelper.ObjectToBytes(endLocationMonitor));
        }

        private static SendingCommand ToSendingEntity(LocationMonitorCMD locationMonitorCMD,DateTime sendtime)
        {
            SendingCommand sendingCommand = new SendingCommand();
            sendingCommand.CommandType = CommandType.C30;
            sendingCommand.OperationID = locationMonitorCMD.SessionID;
            sendingCommand.DeviceID = locationMonitorCMD.MDVRCoreSN;
            sendingCommand.RecordID = locationMonitorCMD.VechileID;
            sendingCommand.RequestTime = sendtime;
            sendingCommand.SendingTime = sendtime;
            sendingCommand.SendingTimeout = sendingCommand.SendingTime.AddSeconds(ConfigInfo.CommandTimeout);
            sendingCommand.TimeoutAction = ReplyTimeout;
            return sendingCommand;
        }
    }
}