using Gsafety.PTMS.Base.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.AnalysisLib
{
    public class TimeoutHelper
    {
        const int DefaultWaitTimeout = 172800;// 2 days 
        const int DefaultReplyTimeout = 1800;

        public static int GetWaitTimeout(string commandType)
        {
            if (string.IsNullOrEmpty(commandType))
            {
                return DefaultWaitTimeout;
            }
            else
            {
                if (commandType.Equals(CommandType.PolygonsRegion) || commandType.Equals(CommandType.RouteInfo) || commandType.Equals(CommandType.SetTermParam))
                    return ConfigInfo.TrafficWaitTimeout;
                else
                    return DefaultWaitTimeout;
            }
        }

        public static int GetReplyTimeout(string commandType)
        {
            if (string.IsNullOrEmpty(commandType))
            {
                return DefaultReplyTimeout;
            }
            else
            {
                if (commandType.Equals(CommandType.PolygonsRegion) || commandType.Equals(CommandType.RouteInfo) || commandType.Equals(CommandType.SetTermParam))
                    return ConfigInfo.TrafficReplyTimeout;
                else
                    return DefaultReplyTimeout;
            }
        }
    }
}
