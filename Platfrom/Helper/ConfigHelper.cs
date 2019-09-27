/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 99a9fb0d-bcca-4371-8058-af9347af12aa      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis.Helper
/////    Project Description:    
/////             Class Name: ConfigHelper
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/9 9:45:14
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/9 9:45:14
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.Common.Logging;

namespace Gsafety.PTMS.Analysis.Helper
{
    public class ConfigHelper
    {
        public static int HeartbeatTimeSpan
        {
            get
            {
                try
                {
                    return int.Parse(ConfigurationManager.AppSettings["HeartbeatTimeSpan"]);
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                    return 30000;
                }
            }
        }
    }
}
