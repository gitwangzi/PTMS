/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e68e31c3-c18d-4100-8905-e978f1f4efc1      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.Common.Util
/////    Project Description:    
/////             Class Name: ConfigurationHelper
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/25 9:41:16
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/25 9:41:16
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.Common.Util
{
    public class ConfigurationHelper
    {
        /// <summary>
        /// FTPServerName
        /// </summary>
        public static string FTPServerName
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["FTPServerName"];
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// FTPWlanServerName
        /// </summary>
        public static string FTPWlanServerName
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["FTPWlanServerName"];
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// FTPUserName
        /// </summary>
        public static string FTPUserName
        {

            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["FTPUserName"];
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// FTPPassword
        /// </summary>
        public static string FTPPassword
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["FTPPassword"];
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// FTPPort
        /// </summary>
        public static int FTPPort
        {
            get
            {
                try
                {
                    return int.Parse(ConfigurationManager.AppSettings["FTPPort"]);
                }
                catch
                {
                    return 21;
                }
            }
        }

        /// <summary>
        /// MailFrom
        /// </summary>
        public static string MailFrom
        {

            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["MailFrom"];
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// MailPwd
        /// </summary>
        public static string MailPwd
        {

            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["MailPwd"];
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// SmtpHost
        /// </summary>
        public static string SmtpHost
        {

            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["SmtpHost"];
                }
                catch
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// SmtpPort
        /// </summary>
        public static int SmtpPort
        {

            get
            {
                try
                {
                    return  Int32.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
                }
                catch
                {
                    return 25;
                }
            }
        }
    }
}
