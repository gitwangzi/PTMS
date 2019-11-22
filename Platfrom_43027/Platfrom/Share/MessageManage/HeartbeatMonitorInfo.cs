/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: dea2f065-0e6b-4aba-b7d7-5346cf1c2ccb      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share
/////    Project Description:    
/////             Class Name: HeartbeatMonitorInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 11/25/2013 6:22:41 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 11/25/2013 6:22:41 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Share
{
    public class HeartbeatMonitorInfo 
    {
        #region Fields

        private readonly long DisConnectionTime = 60000;//1 minues
        private MessageServiceStatus _ServiceStatus = MessageServiceStatus.DisConnected;
        private object lockobj = new object();

        #endregion

        #region Attributes

        public MessageServiceStatus ServiceStatus 
        {
            get
            {
                return _ServiceStatus;
            }
            set
            {
                lock (lockobj)
                {
                    _ServiceStatus = value;
                }
                ApplicationContext.Instance.EventAggregator.Publish<MessageServiceStatus>(_ServiceStatus);
            }
        }

        public DateTime LastHeartBeatTime { get; set; }

        public bool IsDisconnection
        {
            get
            {
                long lastHeartTicks = LastHeartBeatTime.Ticks;
                long currentTicks = DateTime.Now.Ticks;
                TimeSpan elapsedTime = new TimeSpan(currentTicks - lastHeartTicks);
                if (elapsedTime.TotalMilliseconds > DisConnectionTime)
                    return true;
                return false;

            }
        }

        #endregion

        public HeartbeatMonitorInfo()
        {
            
        }
    }
}
