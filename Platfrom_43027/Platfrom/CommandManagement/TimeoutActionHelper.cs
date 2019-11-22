using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Message.Contract.Data;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: d55bba2e-24da-4e57-9431-8b8406406dd0      
/////             clrversion: 4.0.30319.34011
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CommandManagement
/////    Project Description:    
/////             Class Name: TimeoutActionHelper
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/3/18 03:55:43
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/3/18 03:55:43
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
    public class TimeoutActionHelper
    {
        public static Action<PTMSEntities, WaitSendCommand> GetWaitTimeoutAction(string commandType)
        {
            if (string.IsNullOrEmpty(commandType))
            {
                return null;
            }
            else
            {
                if (commandType.Equals(CommandType.PolygonsRegion))
                    return ElectricFenceCommand.WaitingTimeout;
                else if (commandType.Equals(CommandType.OverSpeed))
                    return OverSpeedCommandManager.WaitingTimeout;
                //else if (commandType.Equals(CommandType.C30))
                //    return GpsSendUpCommand.WaitingTimeout;
                //else if (commandType.Equals(CommandType.C64))
                //    return TemperatureCommand.WaitingTimeout;
                //else if (commandType.Equals(CommandType.C80))
                //    return OneKeyAlarmCommand.WaitingTimeout;
                //else if (commandType.Equals(CommandType.C82))
                //    return AbnormalDoorCommand.WaitingTimeout;
                else
                    return null;
            }
        }

        public static Action<PTMSEntities,SendingCommand> GetReplyTimeoutAction(string commandType)
        {
            if (string.IsNullOrEmpty(commandType))
            {
                return null;
            }
            else
            {
                if (commandType.Equals(CommandType.PolygonsRegion))
                    return ElectricFenceCommand.ReplyTimeout;
                else if (commandType.Equals(CommandType.OverSpeed))
                    return OverSpeedCommandManager.ReplyTimeout;
                //else if (commandType.Equals(CommandType.C30))
                //    return GpsSendUpCommand.ReplyTimeout;
                //else if (commandType.Equals(CommandType.C64))
                //    return TemperatureCommand.ReplyTimeout;
                //else if (commandType.Equals(CommandType.C80))
                //    return OneKeyAlarmCommand.ReplyTimeout;
                //else if (commandType.Equals(CommandType.C82))
                //    return AbnormalDoorCommand.ReplyTimeout;
                else
                    return null;
            }
        }
    }
}
