/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 98de134e-fa7a-40f0-8401-fff519318a80      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.Common.Logging
/////    Project Description:    
/////             Class Name: LoggerManager
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/5 11:00:08
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/5 11:00:08
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Gsafety.Common.Logging
{
    public class LoggerManager
    {
        #region Logger

        /// <summary>
        /// Logger
        /// </summary>
        public static ILog Logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion
    }
}
