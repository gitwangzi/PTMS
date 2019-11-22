using Gsafety.Common.Logging;
using Gsafety.PTMS.Command.Repository;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Message.Contract.Data;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: f78bdd21-deba-4f69-b8ac-d870f14a52b5      
/////             clrversion: 4.0.30319.34011
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CommandManagement.Management
/////    Project Description:    
/////             Class Name: BasicCommandManager
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/3/12 11:49:03
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/3/12 11:49:03
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.CommandManagement
{
    public class BasicCommandManager
    {
        static CommmandManageRepository _commandManageRepository;

        static BasicCommandManager()
        {
            _commandManageRepository = new CommmandManageRepository();
        }

        private static SendingCommand ToSendingCommand(WaitSendCommand waitesendCommand)
        {
            SendingCommand sending = new SendingCommand();
            sending.RecordID = waitesendCommand.RecordID;
            sending.OperateType = waitesendCommand.OperateType;
            sending.OperationID = waitesendCommand.OperationID;
            sending.CommandType = waitesendCommand.CommandType;
            sending.DeviceID = waitesendCommand.DeviceID;
            return sending;
        }

        public static void SendWaitCommand(PTMSEntities context, WaitSendCommand waitSendCommand)
        {
            if (waitSendCommand == null)
            {
                return;
            }
            DateTime sendTime = DateTime.Now;
            _commandManageRepository.UpdateCommandSendStatus(context, waitSendCommand.RecordID, sendTime);
            /////////Need to determine whether there is data recorded in the,if there is no,then add.
            SendingCommand sendingComand = ToSendingCommand(waitSendCommand);
            SendingManager.AddSendCommmand(sendingComand);
            WaitSendManager.RemoveCommand(waitSendCommand);
            LoggerManager.Logger.Info("SendWaitCommand Before Publishing " + waitSendCommand.RecordID);
            CommandManager.PublishMessage(waitSendCommand.Exchange, waitSendCommand.RouteKey, waitSendCommand.CommandContent);
        }
    }
}
