/////Copyright (C)2014Microsoft All Rights Reserved.
/////======================================================================
/////                   Guid: 62a74a90-52ab-4771-958a-59d1b82e4974      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-JIANGJ
/////                 Author: TEST(JiangJ)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis.Helper
/////    Project Description:    
/////             Class Name: RedisKey
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/9/12 11:24:05
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/9/12 11:24:05
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
    public class RedisKey
    {
        public const string RemoveAlarmKey = "Hash.Key.AlarmRemove";

        public const string VehicleGpsKey = "Hash.Key.VehicleGpsKey";

        public const string VehicleOnOffLineKey = "Hash.Key.OnOffLineKey";


        #region CommandManagement

        public const string SuiteStatusInfoManage = "Hash.Key.SuiteStatusInfoManager";

        public const string WaitSendManager = "Hash.Key.WaitSendManager";

        public const string SendingManager = "Hash.Key.SendingManager";

        public const string LocationMonitorManager = "Hash.Key.LocationMonitorManager";
        #endregion
    }
}
