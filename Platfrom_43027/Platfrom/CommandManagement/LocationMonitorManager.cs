/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 71935a9c-0944-4e6e-a04a-2a2d1b2aff16      
/////             clrversion: 4.0.30319.34014
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CommandManagement
/////    Project Description:    
/////             Class Name: LocationMonitorManage
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/4/17 04:54:40
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/4/17 04:54:40
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.Common.Logging;
using System.Threading;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.CommandManagement
{
    public class LocationMonitorManager
    {
        static List<SendingCommand> _MonitorCommands = new List<SendingCommand>();
        static object lockobj = new object();

        public static void MonitoringEndControl(PTMSEntities context)
        {
            List<SendingCommand> monitorEndCommand;
            while (true)
            {
                lock (lockobj)
                {
                    monitorEndCommand = _MonitorCommands.Where(item => item.SendingTimeout <= DateTime.Now).ToList();
                }
                if (monitorEndCommand != null && monitorEndCommand.Count > 0)
                {
                    LoggerManager.Logger.Info(string.Format("Monitoring the number of end:{0}.", monitorEndCommand.Count));
                    Parallel.ForEach(monitorEndCommand, (item) =>
                    {
                        if (item.TimeoutAction != null)
                        {
                            item.TimeoutAction.Invoke(context, item);
                        }
                        RemoveSendCommmand(item);
                    });
                    monitorEndCommand.Clear();
                }
                Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// Add sent has been sent,waiting for reply command
        /// </summary>
        /// <param name="sendCommand"></param>
        /// <returns></returns>
        public static bool AddSendCommmand(SendingCommand sendCommand)
        {
            if (sendCommand == null || string.IsNullOrEmpty(sendCommand.DeviceID))
                return false;
            lock (lockobj)
            {
                var isExist = _MonitorCommands.Count(item => item.DeviceID.Equals(sendCommand.DeviceID) && item.RecordID.Equals(sendCommand.RecordID));
                if (isExist > 0)
                    return false;
                _MonitorCommands.Add(sendCommand);
            }
            return true;
        }


        /// <summary>
        /// Delete command has been sent successfully
        /// </summary>
        /// <param name="sendCommand"></param>
        public static bool RemoveSendCommmand(SendingCommand sendCommand)
        {
            lock (lockobj)
            {
                if (_MonitorCommands.Contains(sendCommand))
                {
                    _MonitorCommands.Remove(sendCommand);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Wound has been waiting for a command to cancel monitoring
        /// </summary>
        /// <param name="sessionId">sessionId</param>
        /// <param name="mdvrId">mdvrId</param>
        /// <returns></returns>
        public static bool RemoveSendCommand(string sessionId, string mdvrId)
        {
            lock (lockobj)
            {
                var command = _MonitorCommands.Where(item => item.DeviceID.Equals(mdvrId) && item.OperationID.Equals(sessionId)).FirstOrDefault();
                if (command != null)
                {
                    _MonitorCommands.Remove(command);
                    return true;
                }
                return false;
            }
        }
    }
}
