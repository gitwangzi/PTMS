/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 61da7765-9b51-40c6-8a33-87489c1b27a9      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.MQ
/////    Project Description:    
/////             Class Name: MQConfigHelper
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/7/31 11:16:06
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/7/31 11:16:06
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.MQ
{
    public class MQConfigHelper
    {
        /// <summary>
        /// UserName
        /// </summary>
        public static string UserName
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["UserName"];
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Password
        /// </summary>
        public static string Password
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["Password"];
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// HostName
        /// </summary>
        public static string HostName
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["HostName"];
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
