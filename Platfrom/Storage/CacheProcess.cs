/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: da972416-a40a-49cc-b708-a70962c75422      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis.Storage
/////    Project Description:    
/////             Class Name: CacheTreatment
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/11 17:23:05
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/11 17:23:05
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.Common.Util;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.Common.Logging;
using Gsafety.PTMS.Analysis.Cache;

namespace Gsafety.PTMS.Analysis.Storage
{
    public static class CacheProcess
    {
        /// <summary>
        /// Cache Working Suite
        /// </summary>
        public static void CacheWorkingSuite()
        {
            //get all suite info from database
            List<SimpleSuiteInfo> simpleSuitInfos = GetSimpleSuiteInfo(null);
            if (simpleSuitInfos != null && simpleSuitInfos.Count > 0)
            {
                MdvrIdkeySimpleSuiteCache.BatchAdd(simpleSuitInfos);
                LoggerManager.Logger.Info(string.Format("add {0} data into the cache", simpleSuitInfos.Count));
            }
            else
            {
                LoggerManager.Logger.Warn("Get information security suite is empty!");
            }
        }


        /// <summary>
        /// device install succeed,cache info
        /// </summary>
        public static void ProcessDeviceInstall(byte[] bytes)
        {
            try
            {
                string mdvrcoresn = ConvertHelper.BytesToObject(bytes) as string;
                var item = GetSimpleSuiteInfo(mdvrcoresn);
                if (item != null)
                {
                    MdvrIdkeySimpleSuiteCache.BatchAdd(item);
                    LoggerManager.Logger.Info(string.Format("add a MdvrCoreId {0} to the cache data when the device install", mdvrcoresn));
                }
                else
                {
                    LoggerManager.Logger.Warn(string.Format("Get a MdvrCoreId {0} security suite information is empty  when the device install!", mdvrcoresn));
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// the device is switched to the state of repair then delete from cache
        /// </summary>
        public static void ProcessDeviceMaintain(byte[] bytes)
        {
            try
            {
                string mdvrcoresn = ConvertHelper.BytesToObject(bytes) as string;
                MdvrIdkeySimpleSuiteCache.Remove(mdvrcoresn);
                LoggerManager.Logger.Info(string.Format("DeviceMaintain,Remove WorkingSuiteCache item MdvrCoreId:{0}", mdvrcoresn));
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// get suite info
        /// </summary>
        /// <param name="mdvrCoreId"></param>
        /// <returns></returns>
        public static List<SimpleSuiteInfo> GetSimpleSuiteInfo(string mdvrCoreId)
        {
            string sql = @"SELECT T1.MDVR_CORE_SN,T1.SUITE_INFO_ID,T2.VEHICLE_ID, NULL DISTRICT_CODE FROM SECURITY_SUITE_INFO    T1,
            SECURITY_SUITE_WORKING T2,VEHICLE   T3 WHERE T1.SUITE_INFO_ID = T2.SUITE_INFO_ID AND T2.VEHICLE_ID = T3.VEHICLE_ID AND T3.VALID = 1";

            if (!string.IsNullOrEmpty(mdvrCoreId))
            {
                sql = sql + " AND T1.MDVR_CORE_SN = '" + mdvrCoreId + "'";
            }

            return OracleHelper.ExecuteSqlWithCache();
        }
    }
}
