using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Message.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Analysis
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
                else if (commandType.Equals(CommandType.RouteInfo))
                    return RouteInfoCommand.WaitingTimeout;
                else if (commandType.Equals(CommandType.SetTermParam))
                    return ParamCommand.WaitingTimeout;
                //else if (commandType.Equals(CommandType.OverSpeed))
                //    return OverSpeedCommandManager.WaitingTimeout;
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

        public static Action<PTMSEntities, SendingCommand> GetReplyTimeoutAction(string commandType)
        {
            if (string.IsNullOrEmpty(commandType))
            {
                return null;
            }
            else
            {
                if (commandType.Equals(CommandType.PolygonsRegion))
                    return ElectricFenceCommand.ReplyTimeout;
                else if (commandType.Equals(CommandType.RouteInfo))
                    return RouteInfoCommand.ReplyTimeout;
                else if (commandType.Equals(CommandType.SetTermParam))
                    return ParamCommand.ReplyTimeout;
                //else if (commandType.Equals(CommandType.RouteInfo))
                //    return RouteInfoCommand.WaitingTimeout;
                //else if (commandType.Equals(CommandType.OverSpeed))
                //    return OverSpeedCommandManager.ReplyTimeout;
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
