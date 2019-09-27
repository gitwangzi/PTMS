using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.Common.Logging;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: c4f39947-7807-4e8c-856a-1b77a81d0c51      
/////             clrversion: 4.0.30319.34011
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CommandManagement
/////    Project Description:    
/////             Class Name: TimeoutHelper
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/3/14 03:30:12
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/3/14 03:30:12
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
                if (commandType.Equals(CommandType.PolygonsRegion) || commandType.Equals(CommandType.OverSpeed))
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
                if (commandType.Equals(CommandType.PolygonsRegion) || commandType.Equals(CommandType.OverSpeed))
                    return ConfigInfo.TrafficReplyTimeout;
                else
                    return DefaultReplyTimeout;
            }
        }
    }
}
