/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 680f90f9-4bf4-42f5-9fd6-37f4090186c1      
/////             clrversion: 4.0.30319.34003
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: CommandManagement
/////    Project Description:    
/////             Class Name: WaitSendManager
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/2/7 04:51:22
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/2/7 04:51:22
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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
    public class WaitSendManager
    {
        static List<WaitSendCommand> _WaitSendCommands = new List<WaitSendCommand>();
        private static object lockobj = new object();

        public WaitSendManager(PTMSEntities context)
        {

        }

        /// <summary>
        /// Get waiting for the next command issue from the database
        /// </summary>
        public static void Init(PTMSEntities context)
        {
            LoggerManager.Logger.Info("Start loading wait for the next command issued!");
            CommmandManageRepository commandManageRepository = new CommmandManageRepository();
            _WaitSendCommands = commandManageRepository.GetAllWaitSendCommand(context);
            Parallel.ForEach(_WaitSendCommands, (item) =>
                   {
                       item.RequestTimeout = item.RequestSendTime.AddSeconds(TimeoutHelper.GetWaitTimeout(item.CommandType));
                       item.TimeoutAction = TimeoutActionHelper.GetWaitTimeoutAction(item.CommandType);
                   });
            LoggerManager.Logger.Info(string.Format("End load command information, command number:{0}", _WaitSendCommands.Count));


        }

        /// <summary>
        /// Monitoring timeout command,if the command is removed form the queue to timeout。
        /// whether monitoring equipment waiting to be sent on the corresponding command line。if online，begins to transmit the command。
        /// </summary>
        public static void WaitTimeoutControl(PTMSEntities context)
        {
            List<WaitSendCommand> timeoutCommand;
            while (true)
            {
                try
                {
                    lock (lockobj)
                    {
                        timeoutCommand = _WaitSendCommands.Where(item => item.RequestTimeout <= DateTime.Now).ToList();
                    }
                    if (timeoutCommand != null && timeoutCommand.Count > 0)
                    {
                        LoggerManager.Logger.Info(string.Format("Wait for the timeout command issued:{0}.", timeoutCommand.Count));
                        Parallel.ForEach(timeoutCommand, (item) =>
                            {
                                if (item.TimeoutAction != null)
                                {
                                    item.TimeoutAction.Invoke(context, item);
                                }
                                RemoveCommand(item);
                            });
                        timeoutCommand.Clear();
                    }
                    Thread.Sleep(1000);
                    lock (lockobj)
                    {
                        timeoutCommand = _WaitSendCommands.Where(item => SuiteStatusInfoManage.IsOnline(item.DeviceID)).ToList();
                    }
                    if (timeoutCommand != null && timeoutCommand.Count > 0)
                    {
                        LoggerManager.Logger.Info(string.Format("Commands waiting to be sent there {0} commands can be issued", timeoutCommand.Count));
                        Parallel.ForEach(timeoutCommand, (item) =>
                        {
                            BasicCommandManager.SendWaitCommand(context, item);
                            RemoveCommand(item);
                        });
                        timeoutCommand.Clear();
                    }
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                }
            }
        }



        /// <summary>
        /// Adding a need to wait to send the command
        /// </summary>
        /// <param name="waitSendCommand"></param>
        /// <returns></returns>
        public static bool AddWaitSendCommand(WaitSendCommand waitSendCommand)
        {

            if (waitSendCommand == null || string.IsNullOrEmpty(waitSendCommand.RecordID) || waitSendCommand.CommandContent == null)
                return false;
            lock (lockobj)
            {
                var isExist = _WaitSendCommands.Count(item => item.RecordID.Equals(waitSendCommand.RecordID));
                if (isExist > 0)
                    return false;
                _WaitSendCommands.Add(waitSendCommand);
                LoggerManager.Logger.Info("WaitSendingCommand:" + "RocordID" + waitSendCommand.RecordID + "MDVRID" + waitSendCommand.DeviceID);
            }
            return true;
        }

        /// <summary>
        /// Get commands waiting to be sent in accordance with established alias
        /// </summary>
        /// <param name="VehicleID"></param>
        /// <returns></returns>
        public static List<WaitSendCommand> GetWaitSendCommand(string deviceID)
        {
            try
            {
                lock (lockobj)
                {
                    var waitSendCommmand = _WaitSendCommands.Where(item => item.DeviceID.Equals(deviceID)).ToList();
                    return waitSendCommmand;
                }

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// delete command has been sent
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="requestTime"></param>
        /// <returns></returns>
        public static bool RemoveCommand(string deviceID, DateTime requestTime)
        {
            lock (lockobj)
            {
                var command = _WaitSendCommands.Where(item => item.DeviceID.Equals(deviceID) && item.RequestSendTime.Equals(requestTime)).FirstOrDefault();
                if (command != null)
                {
                    _WaitSendCommands.Remove(command);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// delete comand wating for a reply
        /// </summary>
        /// <param name="waitSendCommand"></param>
        /// <returns></returns>
        public static bool RemoveCommand(WaitSendCommand waitSendCommand)
        {
            lock (lockobj)
            {
                if (_WaitSendCommands.Contains(waitSendCommand))
                {
                    _WaitSendCommands.Remove(waitSendCommand);
                    return true;
                }
                return false;
            }
        }
    }
}
