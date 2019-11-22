/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6afbc4ce-4dcd-4e9e-8ed3-93a98e4b82a7      
/////             clrversion: 4.0.30319.34011
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CommandManagement
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

namespace Gsafety.PTMS.CommandManagement
{
    public class ConfigInfo
    {
        #region Count

        static int DefaultTrafficWaitTimeout = 86400;//1 day
        static int DefaultTrafficReplyTimeout = 1800;//30 min


        #endregion

        #region Fields

        static int _TrafficWaitTimeout = -1;
        static int _TrafficReplyTimeout = -1;
        static bool _IsInitFenceToDevice = false;
        static bool _FenceToDevice = true;
        static bool _IsInitPointToDevice = false;
        static bool _PointToDevice = false;
        static bool _OpenMDVRGPSMoniter = false;
        static bool _IsInitOpenMDVRGPSMonitor = false;
        static int _CommandTimeout = 20;
        static bool _IsCommandTimeout = false;
        static int _TimeInterval = 20;
        static bool _IsInitTimeInterval = false;
        static int _TimeLength = 1800;
        static bool _IsInitTimeLength = false;




        #endregion

        /// <summary>
        /// Electrionic fence information is sent to the device,if sent to the device does not send to platform monitoring program
        /// </summary>
        public static bool FenceToDevice
        {
            get
            {
                try
                {
                    if (!_IsInitFenceToDevice)
                    {

                        string fenceToDevice = ConfigurationManager.AppSettings["FenceToDevice"];
                        if (string.IsNullOrEmpty(fenceToDevice) || fenceToDevice.Equals("0"))
                            _FenceToDevice = false;
                        else
                            _FenceToDevice = true;
                        _IsInitFenceToDevice = true;
                    }
                    return _FenceToDevice;
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                    return _FenceToDevice;
                }
            }
        }

        /// <summary>
        /// Monitoring points are sent to the device, if sent to the device is not sent to the platform monitoring program
        /// </summary>
        public static bool PointToDevice
        {
            get
            {
                try
                {
                    if (!_IsInitPointToDevice)
                    {
                        string pointToDevice = ConfigurationManager.AppSettings["PointToDevice"];
                        if (string.IsNullOrEmpty(pointToDevice) || pointToDevice.Equals("0"))
                            _PointToDevice = false;
                        else
                            _PointToDevice = true;
                        _IsInitPointToDevice = true;
                    }
                    return _PointToDevice;
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                    return _PointToDevice;
                }
            }
        }

        /// <summary>
        /// traffic management command-line because the equipment is not the reason cat not be sent，waiting for the next
        /// command on the command line hair length of time,the default is one day（86400s）,the time in seconds.
        /// </summary>
        public static int TrafficWaitTimeout
        {
            get
            {
                try
                {
                    if (_TrafficWaitTimeout == -1)
                    {
                        int timeout = DefaultTrafficWaitTimeout;
                        string waitTimeout = ConfigurationManager.AppSettings["TrafficWaitTimeout"];
                        if (!string.IsNullOrEmpty(waitTimeout))
                            int.TryParse(waitTimeout, out timeout);
                        _TrafficWaitTimeout = timeout;
                    }
                    return DefaultTrafficWaitTimeout;
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                    return DefaultTrafficWaitTimeout;
                }
            }
        }


        /// <summary>
        /// traffic management command issued after the success of the long wait for a reply time, default is half an hour(1800s).
        /// </summary>
        public static int TrafficReplyTimeout
        {
            get
            {
                try
                {
                    if (_TrafficReplyTimeout == -1)
                    {
                        int timeout = DefaultTrafficReplyTimeout;
                        string replyTimeout = ConfigurationManager.AppSettings["TrafficReplyTimeout"];

                        if (!string.IsNullOrEmpty(replyTimeout))
                        {
                            int.TryParse(replyTimeout, out timeout);
                        }

                        _TrafficReplyTimeout = timeout;
                    }
                    return DefaultTrafficReplyTimeout;
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                    return DefaultTrafficReplyTimeout;
                }
            }
        }



        public static bool OpenMDVRGPSMoniter
        {
            get
            {
                if (!_IsInitOpenMDVRGPSMonitor)
                {
                    _IsInitOpenMDVRGPSMonitor = true;
                    try
                    {
                        string openMDVRGPSMoniterValue = ConfigurationManager.AppSettings["OpenMDVRGPSMoniter"];
                        if (!string.IsNullOrEmpty(openMDVRGPSMoniterValue) && openMDVRGPSMoniterValue.Equals("1"))
                            _OpenMDVRGPSMoniter = true;
                    }
                    catch
                    {
                    }
                }
                return _OpenMDVRGPSMoniter;
            }
        }

        public static int CommandTimeout
        {
            get
            {
                if (!_IsCommandTimeout)
                {
                    _IsCommandTimeout = true;
                    try
                    {
                        int timeValue = DefaultTrafficWaitTimeout;
                        string commandTimeoutValue = ConfigurationManager.AppSettings["CommandTimeout"];
                        if (!string.IsNullOrEmpty(commandTimeoutValue) && int.TryParse(commandTimeoutValue, out timeValue))
                            _CommandTimeout = timeValue;
                    }
                    catch
                    {
                    }

                }
                return DefaultTrafficReplyTimeout;
            }
        }


        public static int TimeInterval
        {
            get
            {
                if (!_IsInitTimeInterval)
                {
                    _IsInitTimeInterval = true;
                    try
                    {
                        int timeValue = DefaultTrafficWaitTimeout;
                        string timeIntervalValue = ConfigurationManager.AppSettings["TimeInterval"];
                        if (!string.IsNullOrEmpty(timeIntervalValue) && int.TryParse(timeIntervalValue, out timeValue))
                            _TimeInterval = timeValue;
                    }
                    catch
                    {
                    }

                }
                return _TimeInterval;
            }
        }

        public static int TimeLength
        {
            get
            {
                if (!_IsInitTimeLength)
                {
                    _IsInitTimeLength = true;
                    try
                    {
                        int timeLength = DefaultTrafficWaitTimeout;
                        string timeLengthValue = ConfigurationManager.AppSettings["TimeLength"];
                        if (!string.IsNullOrEmpty(timeLengthValue) && int.TryParse(timeLengthValue, out timeLength))
                            _TimeLength = timeLength;
                    }
                    catch
                    {
                    }

                }
                return _TimeLength;
            }
        }
    }
}
