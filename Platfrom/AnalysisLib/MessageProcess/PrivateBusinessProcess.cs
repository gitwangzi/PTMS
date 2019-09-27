using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Analysis.MonitorTreatment;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Message.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.AnalysisLib
{
    public class PrivateBusinessProcess
    {
        [Business(typeName: UserRoute.UserLogin)]
        public void ProcessUserLogin(byte[] bytes, string key)
        {
            LoggerManager.Logger.Info("PrivateProcessUserLogin");
            try
            {
                UserModel usrmodel = ConvertHelper.BytesToObject(bytes) as UserModel;
                if (usrmodel != null)
                {
                    LoggerManager.Logger.Info("usrmodel:" + Environment.NewLine + usrmodel.ToString());
                    //更新缓存数据
                    DBEntity.RUN_USER_ONLINE usr = new DBEntity.RUN_USER_ONLINE();
                    usr.ONLINE_TIME = DateTime.Now;
                    usr.SESSION_ID = usrmodel.UserToken;
                    usr.CLIENT_ID = usrmodel.ClientID;
                    usr.USER_ID = usrmodel.UserID;

                    if (!CacheDataManager.Users.ContainsKey(usr.USER_ID))
                    {
                        lock (CacheDataManager.Users)
                        {
                            CacheDataManager.Users.Add(usr.USER_ID, usr);
                        }
                    }
                    if (usrmodel.AlarmProcessor)
                    {
                        lock (CacheDataManager.OrganizationUser)
                        {
                            foreach (var item in usrmodel.Organization)
                            {
                                if (!CacheDataManager.OrganizationUser.ContainsKey(item))
                                {
                                    CacheDataManager.OrganizationUser.Add(item, new Dictionary<string, RUN_USER_ONLINE>());
                                }

                                Dictionary<string, RUN_USER_ONLINE> organizations = CacheDataManager.OrganizationUser[item];

                                if (organizations.ContainsKey(usr.USER_ID))
                                {
                                    organizations[usr.USER_ID] = usr;
                                }
                                else
                                {
                                    organizations.Add(usr.USER_ID, usr);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        [Business(typeName: UserRoute.UserLogout)]
        public void ProcessLogout(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("PrivateProcessLogout");
                UserModel usrmodel = ConvertHelper.BytesToObject(bytes) as UserModel;
                if (usrmodel != null)
                {
                    LoggerManager.Logger.Info("Session:" + Environment.NewLine + usrmodel.UserToken);
                    //更新缓存数据
                    lock (CacheDataManager.Users)
                    {
                        if (CacheDataManager.Users.ContainsKey(usrmodel.UserID))
                        {
                            DBEntity.RUN_USER_ONLINE usr = CacheDataManager.Users[usrmodel.UserID];

                            if (usr.SESSION_ID == usrmodel.UserToken)
                            {
                                CacheDataManager.Users.Remove(usrmodel.UserID);
                            }
                        }
                    }


                    lock (CacheDataManager.OrganizationUser)
                    {
                        foreach (var item in usrmodel.Organization)
                        {
                            if (CacheDataManager.OrganizationUser.ContainsKey(item))
                            {
                                Dictionary<string, RUN_USER_ONLINE> organizations = CacheDataManager.OrganizationUser[item];
                                if (organizations.ContainsKey(usrmodel.UserID))
                                {
                                    organizations.Remove(usrmodel.UserID);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Fatal("ProcessLogout", ex);
            }
        }

        [Business(typeName: UserRoute.UpdateCache)]
        public void OnUserUpdateCache(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("OnUserUpdateCache");
                UserOnlineHeartBeat heartbeat = ConvertHelper.BytesToObject(bytes) as UserOnlineHeartBeat;
                if (heartbeat != null)
                {
                    using (DBEntity.PTMSEntities context = new PTMSEntities())
                    {
                        List<string> userids = CacheDataManager.Users.Keys.ToList();
                        var onlineList = context.RUN_USER_ONLINE.Select(t => t.USER_ID).ToList();
                        var deletes = userids.Where(u => onlineList.Contains(u) == false);

                        lock (CacheDataManager.Users)
                        {
                            foreach (var item in deletes)
                            {
                                CacheDataManager.Users.Remove(item);
                            }
                        }

                        lock (CacheDataManager.OrganizationUser)
                        {
                            foreach (var item in deletes)
                            {
                                foreach (var orgkey in CacheDataManager.OrganizationUser.Keys)
                                {
                                    Dictionary<string, RUN_USER_ONLINE> organizations = CacheDataManager.OrganizationUser[orgkey];
                                    if (organizations.ContainsKey(item))
                                    {
                                        organizations.Remove(item);
                                    }
                                }
                            }
                        }

                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }


        [Business(typeName: ManagementRoute.ClientModeChange)]
        public void ProcessClientModelChange(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("ProcessClientModelChange");
                OrderClient orderclient = ConvertHelper.BytesToObject(bytes) as OrderClient;
                if (orderclient != null)
                {
                    LoggerManager.Logger.Info("orderclient:" + Environment.NewLine + orderclient.ToString());

                    lock (CacheDataManager.ClientModes)
                    {
                        if (CacheDataManager.ClientModes.ContainsKey(orderclient.ID))
                        {
                            CacheDataManager.ClientModes[orderclient.ID] = (short)orderclient.TansferMode;
                        }
                        else
                        {
                            CacheDataManager.ClientModes.Add(orderclient.ID, (short)orderclient.TansferMode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Fatal("ProcessLogout", ex);
            }
        }

        [Business(typeName: MonitorRoute.OriginalOnOfflineKey)]
        public void ProcessOnOffline(byte[] bytes, string key)
        {
            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
                    LoggerManager.Logger.Info(string.Format("Get OnOffline info:{0}", str));
                    lock (CacheDataManager.Suites)
                    {
                        OnOffLineData tempmodel = JsonHelper.FromJsonString<OnOffLineData>(str);
                        if (tempmodel.IsOnline == 1)
                        {
                            UpdateSuiteCache(tempmodel.UID, context);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }


        /// <summary>
        /// Device installation is successful,cache informationg
        /// </summary>
        [Business(typeName: UserMessageRoute.CompleteSuiteInstallKey)]
        public void ProcessDeviceInstall(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("ProcessDeviceInstall");
                var mdvrcoresn = ConvertHelper.BytesToObject(bytes) as string;
                using (PTMSEntities entites = new PTMSEntities())
                {
                    LoggerManager.Logger.Info(string.Format("Before add a MdvrCoreId {0} to the cache data when the device install", mdvrcoresn));
                    UpdateSuiteCache(mdvrcoresn, entites);

                    LoggerManager.Logger.Info(string.Format("After add a MdvrCoreId {0} to the cache data when the device install", mdvrcoresn));
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        private static void UpdateSuiteCache(string mdvrcoresn, PTMSEntities entites)
        {
            var w = entites.RUN_SUITE_WORKING.FirstOrDefault(x => x.MDVR_CORE_SN == mdvrcoresn);
            if (w != null)
            {
                lock (CacheDataManager.Suites)
                {
                    if (CacheDataManager.Suites.ContainsKey(mdvrcoresn))
                    {
                        CacheDataManager.Suites.Remove(mdvrcoresn);
                    }

                    var v = entites.BSC_VEHICLE.FirstOrDefault(n => n.VEHICLE_ID == w.VEHICLE_ID);
                    var s = entites.BSC_DEV_SUITE.FirstOrDefault(n => n.SUITE_INFO_ID == w.SUITE_INFO_ID);

                    SuiteCache cache = new SuiteCache();
                    cache.CLIENT_ID = w.CLIENT_ID;
                    cache.CONTACT_PHONE = v.CONTACT_PHONE;
                    cache.DISTRICT_CODE = v.DISTRICT_CODE;
                    cache.ORGNIZATION_ID = v.ORGNIZATION_ID;
                    cache.SUITE_ID = s.SUITE_ID;
                    cache.Status = w.STATUS.Value;
                    cache.SuiteStatus = s.STATUS;
                    cache.OWNER = v.OWNER;
                    cache.SUITE_INFO_ID = s.SUITE_INFO_ID;
                    cache.VEHICLE_ID = v.VEHICLE_ID;
                    cache.MDVR_Core_SN = w.MDVR_CORE_SN;

                    CacheDataManager.Suites.Add(cache.MDVR_Core_SN, cache);
                }
            }
        }

        /// <summary>
        /// The device is switched to the repair then delete from cache
        /// </summary>
        [Business(typeName: UserMessageRoute.SuiteMaintainKey)]
        public void ProcessDeviceMaintain(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("ProcessDeviceMaintain");
                string mdvrcoresn = ConvertHelper.BytesToObject(bytes) as string;
                if (mdvrcoresn != null)
                {
                    lock (CacheDataManager.Suites)
                    {
                        if (CacheDataManager.Suites.ContainsKey(mdvrcoresn))
                        {
                            CacheDataManager.Suites.Remove(mdvrcoresn);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// Start Install
        /// </summary>
        [Business(typeName: UserMessageRoute.StartSuiteInstallKey)]
        public void ProcessStartInstall(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("ProcessStartInstall");
                var mdvrcoresn = ConvertHelper.BytesToObject(bytes) as string;
                using (PTMSEntities entites = new PTMSEntities())
                {
                    UpdateSuiteCache(mdvrcoresn, entites);
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// from cache delete suite info where delete install
        /// </summary>
        [Business(typeName: UserMessageRoute.DeleteSuiteInstallKey)]
        public void ProcessDeleteInstall(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("ProcessDeleteInstall");
                string mdvrcoresn = ConvertHelper.BytesToObject(bytes) as string;
                if (mdvrcoresn != null)
                {
                    lock (CacheDataManager.Suites)
                    {
                        if (CacheDataManager.Suites.ContainsKey(mdvrcoresn))
                        {
                            CacheDataManager.Suites.Remove(mdvrcoresn);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// Device installation is successful,cache informationg
        /// </summary>
        [Business(typeName: UserMessageRoute.CompleteGPSInstallKey)]
        public void ProcessGPSInstall(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("ProcessGPSInstall");
                var gpsid = ConvertHelper.BytesToObject(bytes) as string;
                using (PTMSEntities entites = new PTMSEntities())
                {
                    var item = entites.RUN_GPS_WORKING.FirstOrDefault(x => x.GPS_SN == gpsid);
                    if (item != null)
                    {
                        LoggerManager.Logger.Info(string.Format("Before add a GPS {0} to the cache data when the device install", gpsid));
                        lock (CacheDataManager.GPSs)
                        {
                            if (CacheDataManager.GPSs.ContainsKey(gpsid))
                            {
                                CacheDataManager.GPSs.Remove(gpsid);
                            }
                            CacheDataManager.GPSs.Add(item.GPS_SN, item);
                        }
                        LoggerManager.Logger.Info(string.Format("After add a GPS {0} to the cache data when the device install", gpsid));
                    }
                    else
                    {
                        LoggerManager.Logger.Warn(string.Format("Get a GPS {0} security suite information is empty  when the device install!", gpsid));
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// The device is switched to the repair then delete from cache
        /// </summary>
        [Business(typeName: UserMessageRoute.GPSMaintainKey)]
        public void ProcessGPSMaintain(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("ProcessGPSMaintain");
                string gpsid = ConvertHelper.BytesToObject(bytes) as string;
                if (gpsid != null)
                {
                    lock (CacheDataManager.GPSs)
                    {
                        if (CacheDataManager.GPSs.ContainsKey(gpsid))
                        {
                            CacheDataManager.GPSs.Remove(gpsid);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// Start Install
        /// </summary>
        [Business(typeName: UserMessageRoute.StartGPSInstallKey)]
        public void ProcessGPSStartInstall(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("ProcessGPSStartInstall");
                var gpsid = ConvertHelper.BytesToObject(bytes) as string;
                using (PTMSEntities entites = new PTMSEntities())
                {
                    var item = entites.RUN_GPS_WORKING.FirstOrDefault(x => x.GPS_SN == gpsid);
                    if (item != null)
                    {
                        LoggerManager.Logger.Info(string.Format("Before add a GPS {0} to the cache data when the start install", gpsid));
                        lock (CacheDataManager.GPSs)
                        {
                            CacheDataManager.GPSs.Add(item.GPS_SN, item);
                        }
                        LoggerManager.Logger.Info(string.Format("After add a GPS {0} to the cache data when the start install", gpsid));
                    }
                    else
                    {
                        LoggerManager.Logger.Warn(string.Format("Get a GPS {0} security suite information is empty  when the start install!", gpsid));
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// from cache delete suite info where delete install
        /// </summary>
        [Business(typeName: UserMessageRoute.DeleteGPSInstallKey)]
        public void ProcessDeleteGPSInstall(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("ProcessDeleteGPSInstall");
                string gpsid = ConvertHelper.BytesToObject(bytes) as string;
                if (gpsid != null)
                {
                    lock (CacheDataManager.GPSs)
                    {
                        if (CacheDataManager.GPSs.ContainsKey(gpsid))
                        {
                            CacheDataManager.GPSs.Remove(gpsid);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }
    }
}
