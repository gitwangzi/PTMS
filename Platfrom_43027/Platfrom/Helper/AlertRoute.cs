/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: bb6dea34-c6f5-4c60-b629-958bd970958a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-GUOH
/////                 Author: TEST(guoh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis.Helper
/////    Project Description:    
/////             Class Name: AlertRoute
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/29 09:02:25
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/29 09:02:25
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
    public class AlertRoute
    {
        public const string OriginalBusinessAlertKey = "MDVR.BusinessAlert.";

        public const string HandleBusinessAlertKey = "MDVR.BusinessAlertEx.";

        public const string OriginalDeviceAlertKey = "MDVR.DeviceAlert.";

        public const string HandleDeviceAlertKey = "MDVR.DeviceAlertEx.";

        public const string HandleRemoveDeviceSuiteAlertKey = "MDVR.RemoveDeviceSuiteAlert.";

        public const string CompleteAlert = "App.CompleteAlert.";

        public const string CompleteAlertNotice = "App.CompleteAlertNotice.";
    }
}
