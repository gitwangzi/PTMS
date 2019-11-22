/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6afbc4ce-4dcd-4e9e-8ed3-93a98e4b82a7      
/////             clrversion: 4.0.30319.34011
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.Ant.CommandManagement
/////    Project Description:    
/////             Class Name: CommandManagerConfigInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/3/13 06:28:00
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/3/13 06:28:00
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

namespace Gsafety.Ant.CommandManagement
{
    public class CommandManagerConfigInfo
    {
        /// <summary>
        /// 电子围栏信息是否发送到设备，如果发送到设备就不发送到平台监控程序
        /// </summary>
        public static bool FenceToDevice
        {
            get
            {
                try
                {
                    string fenceToDevice = ConfigurationManager.AppSettings["FenceToDevice"];
                    if (string.IsNullOrEmpty(fenceToDevice) || fenceToDevice.Equals("0"))
                        return false;
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                    return true;
                }
            }
        }

        /// <summary>
        /// 监控点是否发送到设备,如果发送到设备就不发送到平台监控程序
        /// </summary>
        public static bool PointToDevice
        {
            get
            {
                try
                {
                    string pointToDevice = ConfigurationManager.AppSettings["PointToDevice"];
                    if (string.IsNullOrEmpty(pointToDevice) || pointToDevice.Equals("0"))
                        return false;
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                    return false;
                }
            }
        }
    }
}
