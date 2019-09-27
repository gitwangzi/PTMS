/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2c0557ce-78e6-41f6-9904-58dbca0afba6      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Repository
/////    Project Description:    
/////             Class Name: Util
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-08-31 14:01:36
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-08-31 14:01:36
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Integration.Repository
{
    public class Util
    {
        

        private static string _ip;

        public static string Ip
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ip))
                {
                    _ip = System.Configuration.ConfigurationManager.AppSettings["videoServiceIpAddress"];

                }
                return _ip;
            }
            
        }

        private static int _port;

        public static int Port
        {
            get
            {
                if (_port <= 0)
                {
                    _port =int.Parse(System.Configuration.ConfigurationManager.AppSettings["videoServicePort"]);
                }
                return _port;
            }
            
        }
    }
}
