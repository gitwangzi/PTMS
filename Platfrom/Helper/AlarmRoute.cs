/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7fbd4097-3851-4fbe-abfa-609d8b283d6a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis.Helper
/////    Project Description:    
/////             Class Name: AlarmRoute
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/28 19:02:01
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/28 19:02:01
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
    /// <summary>
    /// a key alarm
    /// </summary>
    public class AlarmRoute
    {
        public const string OriginalAlarmInfoKey = "MDVR.AlarmInfo.";

        public const string HandleAlarmInfoKey = "MDVR.AlarmInfoEx.";

        public const string AlarmInfoKey = "App.AlarmInfoEx.";

        public const string CompleteAlarm = "App.CompleteAlarm.";

        public const string CompleteAlarmNotice = "App.CompleteAlarmNotice.";

        /// <summary>
        /// processomg os complete
        /// </summary>
        public const string HandleAlarmKey = "MDVR.Alarm.";

        /// <summary>
        /// Ant processed and police intelligeance platform for true
        /// </summary>
        public const string RealAlarmKey = "APP.RealAlarm.";

        public const string MobileAlarmKey = "MOBILE.AlarmInfo.";

        public const string DirectTransferKey = "App.DirectTransfer.";

        public const string JudgeTransferKey = "App.JudgeTransfer.";
    }
}
