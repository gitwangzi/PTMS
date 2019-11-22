/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 25b32f3c-78c1-4356-a8ba-bf3df00ad956      
/////             clrversion: 4.0.30319.34011
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CommandManagement
/////    Project Description:    
/////             Class Name: SuiteStatusInfoManage
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/3/11 03:50:29
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/3/11 03:50:29
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.Common.Logging;
using Gsafety.PTMS.Analysis.Helper;
using System.Diagnostics;


namespace Gsafety.PTMS.CommandManagement
{
    public class SuiteStatusInfoManage
    {
        //static Hashtable MDVRSuiteStatusInfos = new Hashtable();
        static RedisManager<SuiteStatusInfo> MDVRSuiteStatusInfos = new RedisManager<SuiteStatusInfo>();
        static object lockobj = new object();
        static SecuritySuiteRepository _SecuritysuiteRepository;
        public static void Init()
        {
            LoggerManager.Logger.Info("Load running device information from the data!");
            _SecuritysuiteRepository = new SecuritySuiteRepository();

            List<SuiteStatusInfo> suiteStatusInfo = _SecuritysuiteRepository.GetWorkingSuiteInfoToAlertManager();
            if (suiteStatusInfo.Count > 0)
            {
                Dictionary<string, SuiteStatusInfo> dic = new Dictionary<string, SuiteStatusInfo>();
                foreach (SuiteStatusInfo item in suiteStatusInfo)
                {
                    if (!dic.ContainsKey(item.MdvrCoreId))
                    {
                        dic.Add(item.MdvrCoreId, item);
                    }
                }
                MDVRSuiteStatusInfos.Hash_SetList(RedisKey.SuiteStatusInfoManage, dic);
                LoggerManager.Logger.Info("Set SuiteStatus In Redis at startup!");
            }
            else
            {
                LoggerManager.Logger.Info("Didn't Set Suite Status in Redis at startup!");
            }
            LoggerManager.Logger.Info(string.Format("The current device data from the database is running:{0}!", suiteStatusInfo.Count));
        }

        /// <summary>
        /// According to MDVR_Core_Id
        /// </summary>
        /// <param name="mdvrId"></param>
        /// <returns></returns>
        public static SuiteStatusInfo GetSuiteStatusInfo(string mdvrId)
        {
            try
            {
                if (string.IsNullOrEmpty(mdvrId))
                    return null;
                if (MDVRSuiteStatusInfos.Hash_Exist(RedisKey.SuiteStatusInfoManage, mdvrId))
                {
                    LoggerManager.Logger.Info("Get Suite Info From Redis " + mdvrId);
                    var suiteStatusInfo = MDVRSuiteStatusInfos.Hash_Get(RedisKey.SuiteStatusInfoManage, mdvrId);
                    if (suiteStatusInfo != null)
                        return suiteStatusInfo as SuiteStatusInfo;
                    return null;
                }
                else
                {
                    LoggerManager.Logger.Info("There is no Suite Info In Redis " + mdvrId);
                    return null;
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("Get suite status info failure!" + ex);
                return null;
            }
        }

        /// <summary>
        /// modify security suite online information
        /// </summary>
        /// <param name="mdvrID"></param>
        /// <param name="isOnline"></param>
        public static void ChangeOnlineFlag(string mdvrID, bool isOnline)
        {
            try
            {
                if (string.IsNullOrEmpty(mdvrID))
                    return;
                LoggerManager.Logger.Info("Before Get MDVRS from Redis");
                var suiteInfo = MDVRSuiteStatusInfos.Hash_Get(RedisKey.SuiteStatusInfoManage, mdvrID);
                if (suiteInfo != null)
                {
                    LoggerManager.Logger.Info("The MDVR exists in Redis");
                    lock (lockobj)
                    {
                        (suiteInfo as SuiteStatusInfo).OnlineFlag = isOnline;
                        LoggerManager.Logger.Info("Before Update MDVR to Redis");
                        MDVRSuiteStatusInfos.Hash_Set(RedisKey.SuiteStatusInfoManage, mdvrID, suiteInfo);
                     //   lstSuiteStatusInfo.Find(c => c.MdvrCoreId.Equals(mdvrid));
                        var item =
                            ElectricFenceCommand.lstSuiteStatusInfo.FirstOrDefault(
                                x => x.MdvrCoreId == suiteInfo.MdvrCoreId);
                        if (item !=null)
                        {
                            ElectricFenceCommand.lstSuiteStatusInfo.Remove(item);
                            ElectricFenceCommand.lstSuiteStatusInfo.Add(suiteInfo as SuiteStatusInfo);
                        }
                        else
                        {
                            ElectricFenceCommand.lstSuiteStatusInfo.Add(suiteInfo as SuiteStatusInfo);
                        }
                        LoggerManager.Logger.Info("After Update MDVR to Redis");
                    }
                }
                else
                {
                    LoggerManager.Logger.Info("There is no MDVRS in Redis");
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("Change device online flag failure!" + ex);
            }
        }

        public static bool IsOnline(string mdvrId)
        {
            try
            {
                if (string.IsNullOrEmpty(mdvrId))
                    return false;
                if (MDVRSuiteStatusInfos.Hash_Exist(RedisKey.SuiteStatusInfoManage, mdvrId))
                {
                    var suiteStatusInfo = MDVRSuiteStatusInfos.Hash_Get(RedisKey.SuiteStatusInfoManage, mdvrId);
                    if (suiteStatusInfo.OnlineFlag)
                    {
                        return (suiteStatusInfo as SuiteStatusInfo).OnlineFlag;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("Get suite status info failure!" + ex);
                return false;
            }
        }

        /// <summary>
        /// according to delete the corresponding information security suite package information
        /// </summary>
        /// <param name="mdvrID"></param>
        public static void DeleteSuiteInfo(string mdvrID)
        {
            try
            {
                if (string.IsNullOrEmpty(mdvrID))
                    return;
                if (MDVRSuiteStatusInfos.Hash_Exist(RedisKey.SuiteStatusInfoManage, mdvrID))
                {
                    lock (lockobj)
                    {
                        MDVRSuiteStatusInfos.Hash_Remove(RedisKey.SuiteStatusInfoManage, mdvrID);
                    }
                }
                else
                {
                    LoggerManager.Logger.Info("There is no suite infos in Redis");
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("Remove device failure!" + ex);
            }
        }

        /// <summary>
        /// according to MDVR_Core_SN add a new job in information securirty suite
        /// </summary>
        /// <param name="mdvrId"></param>
        public static void AddSuiteInfo(string mdvrId)
        {
            try
            {
                if (string.IsNullOrEmpty(mdvrId))
                    return;

                if (MDVRSuiteStatusInfos.Hash_Exist(RedisKey.SuiteStatusInfoManage, mdvrId))
                {
                    LoggerManager.Logger.Info("There is no MDVRS in Redis");
                    return;
                }
                else
                {
                    var suiteStatusInfo = _SecuritysuiteRepository.GetSuiteInfoToAlertManager(mdvrId);
                    if (suiteStatusInfo != null)
                    {
                        lock (lockobj)
                        {
                            LoggerManager.Logger.Info("Update security info to Redis");
                            MDVRSuiteStatusInfos.Hash_Set(RedisKey.SuiteStatusInfoManage, mdvrId, suiteStatusInfo);

                            if (ElectricFenceCommand.lstSuiteStatusInfo != null)
                            {
                                ElectricFenceCommand.lstSuiteStatusInfo.Add(GetSuiteStatusInfo(mdvrId));
                            }
                            else
                            {
                                ElectricFenceCommand.lstSuiteStatusInfo = GetAllSuite();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("Add device failure!" + ex);
            }
        }


        public static List<SuiteStatusInfo> GetAllSuite()
        {
            return MDVRSuiteStatusInfos.Hash_GetAll(RedisKey.SuiteStatusInfoManage);
        }
    }
}
