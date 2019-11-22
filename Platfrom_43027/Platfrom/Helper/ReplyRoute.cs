/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2b83f928-9a6c-451b-9f46-a1d993e76dd2      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis.Helper
/////    Project Description:    
/////             Class Name: ReplyRoute
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/30 11:09:14
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/30 11:09:14
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Analysis.Helper
{
    public class ReplyRoute
    {
        /// <summary>
        /// Upgrade Reply
        /// </summary>
        public const string OriginalUpgradeReplyKey = "MDVR.V0C152.";

        /// <summary>
        /// Device Reply
        /// </summary>
        public const string OriginalFenceyDeviceReplyKey = "MDVR.V0C107.";

        /// <summary>
        /// fence business reply
        /// </summary>
        public const string OriginalFenceyBusinessReplyKey = "MDVR.FenceyBusinessReply.";

        /// <summary>
        /// Get upgrade statue reply
        /// </summary>
        public const string OriginalUpgradeStatusReplyKey = "MDVR.V0C602.";

        /// <summary>
        /// get run statuc reply
        /// </summary>
        public const string OriginalSuiteRunintStatusReplyKey = "MDVR.V0C601.";

        /// <summary>
        /// set over speed reply
        /// </summary>
        public const string OriginalSettingOverSpeedReplyKey = "MDVR.V0C68.";

        /// <summary>
        /// Daily monitoring of the GPS information request MDVR Reply
        /// </summary>
        public const string HandleGPSMonitorReplyKey = "MDVR.V0C30.";

        public const string HandleTemperatureReplyKey = "MDVR.V0C64.";

        public const string HandleOneKeyAlarmReplyKey = "MDVR.V0C80.";

        public const string HandleDelayAlarmReplyKey = "MDVR.V0C78.";

        public const string HandleAbnormalDoorReplyKey = "MDVR.V0C82.";

        public const string HandleVideoListReplyKey = "MDVR.V0C110.";

        public const string HandleDownloadMdvrFile = "MDVR.V0C114.";

        public const string HandleDownloadFileV23 = "MDVR.V23.";

        public const string HandleBusinessVideoListReplyKey = "MDVR.VideoBusinessReply.";

        public const string HandleDownloadFileReplyKey = "MDVR.DownloadFileReply.";
    }
}
