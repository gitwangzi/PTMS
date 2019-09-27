/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 301591e2-34b3-4f12-b41f-6a8d19c1e9b4      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis.Helper
/////    Project Description:    
/////             Class Name: GPSRoute
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/28 19:04:08
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/28 19:04:08
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
    public class GPSRoute
    {
        public const string OriginalGPSKey = "MDVR.V30.";

        public const string HandleAlarmGpsKey = "MDVR.AlarmGps.";

        public const string HandleMonitorGpsKey = "MDVR.MonitorGps.";

        public const string SuiteKey = "MDVR.GpsInfo.";

        public const string GPSKey = "GPS.GpsInfo.";

        public const string MobileKey = "MOBILE.GpsInfo.";

        public const string GPSOnOffLine = "GPS.OnOffline.";

        public const string GPSAuthenticateKey = "GPS.Authenticate.";

        public const string HandleGPSAuthenticateResponseKey = "GPS.AuthenticateResponse.";
    }
}
