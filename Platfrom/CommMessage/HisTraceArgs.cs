/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 386986e4-95f7-4abe-a6c6-537121dd70ba      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LILF
/////                 Author: TEST(lilf)
/////======================================================================
/////           Project Name: Gsafety.Common.CommMessage
/////    Project Description:    
/////             Class Name: HisTraceArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/7 14:27:58
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/7 14:27:58
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

namespace Gsafety.Common.CommMessage
{

    ///
    /// 
    /// operator for the historical track
    /// 
    public enum HisTraceOption : short
    {
        /// <summary>
        /// add the historical track
        /// </summary>
        Add = 1,
        /// <summary>
        /// delete the historical track
        /// </summary>
        Delete = 2,
        /// <summary>
        /// play the historical track
        /// </summary>
        Play = 3,
        /// <summary>
        /// pause the historical track to play
        /// </summary>
        Pause = 4,
        /// <summary>
        /// stop the historical track to play
        /// </summary>
        Stop = 5
    }

    public enum HisGPSDataType : short
    {
        MonitorGPS=1,
        AlarmGPS=2
    }
    
    /// <summary>
    /// define message type of historical track request
    /// </summary>
    public class HisTraceArgs : EventArgs
    {
        public string CarNo
        {
            get;
            set;
        }
        
        public DateTime StartTime
        {
            get;
            set;
        }
        
        public DateTime EndTime
        {
            get;
            set;
        }
        
        public Color LineColor
        {
            get;
            set;
        }

        //public HisGPSDataType GpsDataType
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// add、del、play、pause play、stop
        /// </summary>
        public HisTraceOption Op
        {
            get;
            set;
        }
    }


}
