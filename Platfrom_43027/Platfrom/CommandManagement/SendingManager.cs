/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: ae4c271f-3dfe-442f-9ddf-5510207c15d3      
/////             clrversion: 4.0.30319.34003
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: CommandManagement
/////    Project Description:    
/////             Class Name: SendingManager
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/2/7 08:42:08
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/2/7 08:42:08
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.PTMS.Command.Repository;
using Gsafety.Common.Logging;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.CommandManagement
{
    public class SendingManager
    {
        static List<SendingCommand> _SendingCommands = new List<SendingCommand>();
        static object lockobj = new object();

        public static void Init(PTMSEntities context)
        {
            LoggerManager.Logger.Info("start loading Waiting for a reply command!");
            CommmandManageRepository commandManageRepository = new CommmandManageRepository();
            _SendingCommands = commandManageRepository.GetAllSendingCommand(context);
            Parallel.ForEach(_SendingCommands, (item) =>
               {
                   item.SendingTimeout = item.SendingTime.AddSeconds(TimeoutHelper.GetReplyTimeout(item.CommandType));
                   item.TimeoutAction = TimeoutActionHelper.GetReplyTimeoutAction(item.CommandType);
               });
            LoggerManager.Logger.Info(string.Format("End load  Waiting for a reply command, command number:{0}", _SendingCommands.Count));
        }

        public static void SendTimeoutControl(PTMSEntities context)
        {
            List<SendingCommand> timeoutCommand;
            while (true)
            {
                lock (lockobj)
                {
                    timeoutCommand = _SendingCommands.Where(item => item.SendingTimeout <= DateTime.Now).ToList();
                }
                if (timeoutCommand != null && timeoutCommand.Count > 0)
                {
                    LoggerManager.Logger.Info(string.Format("Reply wait timeout command issued number:{0}.", timeoutCommand.Count));
                    Parallel.ForEach(timeoutCommand, (item) =>
                    {
                        if (item.TimeoutAction != null)
                        {
                            item.TimeoutAction.Invoke(context, item);
                        }
                        RemoveSendCommmand(item);
                    });
                    timeoutCommand.Clear();
                }
                Thread.Sleep(2000);
            }
        }

        public static SendingCommand GetSendCommand(string mdvrid, DateTime requestTime, string commandType)
        {
            lock (lockobj)
            {
                var command = _SendingCommands.Where(item => item.DeviceID.Equals(mdvrid) && (item.RequestTime - requestTime).Seconds == 0 && item.CommandType.Equals(commandType)).FirstOrDefault();
                return command;
            }
        }

        //public static SendingCommand GetSendCommand(string mdvrid, string commandType)
        //{
        //    lock (lockobj)
        //    {
        //        var command = _SendingCommands.Where(item => item.DeviceID.Equals(mdvrid) && item.CommandType.Equals(commandType)).FirstOrDefault();
        //        return command;
        //    }
        //}

        public static SendingCommand GetSendCommand(string msgId, string commandType, string mdvrID)
        {
            lock (lockobj)
            {
                if (commandType == "C68")
                {
                    return _SendingCommands.Where(item => item.CommandType.Equals(commandType) && item.DeviceID.Equals(mdvrID)).FirstOrDefault();
                }
                else
                {
                    return _SendingCommands.Where(item => item.CommandType.Equals(commandType) && item.DeviceID.Equals(mdvrID) && item.OperationID.Equals(msgId)).FirstOrDefault();
                }
                //if (commandType == "C107")
                //    return _SendingCommands.Where(item => item.CommandType.Equals(commandType) && item.DeviceID.Equals(mdvrID) && item.OperationID.Equals(msgId)).FirstOrDefault();
                //else

                //return _SendingCommands.Where(item => item.CommandType.Equals(commandType) && item.DeviceID.Equals(mdvrID) && item.OperationID.Substring(0, 32).Contains(msgId)).FirstOrDefault();

            }
        }

        /// <summary>
        /// add sent has been sent,waiting for reply command
        /// </summary>
        /// <param name="sendCommand"></param>
        /// <returns></returns>
        public static bool AddSendCommmand(SendingCommand sendCommand)
        {
            if (sendCommand == null || string.IsNullOrEmpty(sendCommand.DeviceID))
                return false;
            lock (lockobj)
            {
                if (_SendingCommands.Count > 0)
                {
                    var isExist = _SendingCommands.Count(item => item.DeviceID.Equals(sendCommand.DeviceID) && item.SendingTime.Equals(sendCommand.SendingTime) && item.CommandType.Equals(sendCommand.CommandType));
                    if (isExist > 0)
                        return false;
                    _SendingCommands.Add(sendCommand);
                }
                else
                    _SendingCommands.Add(sendCommand);
                LoggerManager.Logger.Info("AddSendCommmand:" + "RocordID" + sendCommand.RecordID + "MDVRID" + sendCommand.DeviceID);
            }
            return true;
        }

        /// <summary>
        /// delete command has been set successfully
        /// </summary>
        /// <param name="sendCommand"></param>
        public static bool RemoveSendCommmand(SendingCommand sendCommand)
        {
            lock (lockobj)
            {
                if (_SendingCommands.Contains(sendCommand))
                {
                    _SendingCommands.Remove(sendCommand);
                    return true;
                }
                return false;
            }
        }

    }
}
