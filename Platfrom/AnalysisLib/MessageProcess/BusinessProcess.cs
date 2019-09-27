/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: b10990cb-1847-4102-9729-7d919934ca41      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis
/////    Project Description:    
/////             Class Name: BusinessProcess
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/2 15:00:34
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/24 15:40:00
/////            Modified by: guoh
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.Common.Util;
using Gsafety.MQ;
using Gsafety.Common.Logging;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Analysis.MonitorTreatment;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.PTMS.Base.Contract.Data;
using System.Reflection;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.AnalysisLib.Command;
using Gsafety.PTMS.Message.Contract;
using Gsafety.PTMS.Monitor.Repository;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Enum;
using Gsafety.PTMS.Analysis.DeviceAccess;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data.Enum;


namespace Gsafety.PTMS.AnalysisLib
{
    public class ShareBusinessProcess
    {
        [Business(typeName: UserRoute.UserLogin)]
        public void ProcessUserLogin(byte[] bytes, string key)
        {
            LoggerManager.Logger.Info("ShareProcessUserLogin");
            try
            {
                UserModel usrmodel = ConvertHelper.BytesToObject(bytes) as UserModel;
                if (usrmodel != null)
                {
                    using (DBEntity.PTMSEntities context = new PTMSEntities())
                    {
                        if (usrmodel.ClientID != null)
                        {
                            RUN_USER_ONLINE usr = context.RUN_USER_ONLINE.FirstOrDefault(n => n.USER_ID == usrmodel.UserID);
                            if (usr == null)
                            {
                                //add to database
                                usr = new RUN_USER_ONLINE();
                                usr.USER_ID = usrmodel.UserID;
                                usr.SESSION_ID = usrmodel.UserToken;
                                usr.ONLINE_TIME = DateTime.UtcNow;
                                usr.UPDATE_TIME = DateTime.UtcNow;
                                usr.CLIENT_ID = usrmodel.ClientID;

                                context.RUN_USER_ONLINE.Add(usr);

                                context.SaveChanges();

                                LoggerManager.Logger.Info("ShareProcessUserLogin:Add New Session:" + usr.SESSION_ID);
                            }
                            else
                            {
                                if (usr.SESSION_ID != usrmodel.UserToken)
                                {
                                    LoggerManager.Logger.Info("ShareProcessUserLogin:Old Session:" + usr.SESSION_ID + " Replace by " + usrmodel.UserToken);
                                    LOG_ACCESS logaccess = context.LOG_ACCESS.FirstOrDefault(n => n.SESSION_ID == usr.SESSION_ID);
                                    if (logaccess != null)
                                    {
                                        logaccess.LOGOUT_TIME = DateTime.UtcNow;
                                    }
                                    usr.USER_ID = usrmodel.UserID;
                                    usr.SESSION_ID = usrmodel.UserToken;
                                    usr.ONLINE_TIME = DateTime.UtcNow;
                                    usr.UPDATE_TIME = DateTime.UtcNow;
                                    usr.CLIENT_ID = usrmodel.ClientID;

                                    context.SaveChanges();
                                    usrmodel.MessageType = (int)MessageTypeEnum.ForceLogout;
                                    byte[] sendbytes = ConvertHelper.ObjectToBytes(usrmodel);

                                    TransforMessage.PublishMessage(Constdefine.APPEXCHANGE, UserRoute.ForceLogout, sendbytes);
                                }
                                else
                                {
                                    LoggerManager.Logger.Info("User login but didn't get processed:" + usr.USER_ID);
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
                LoggerManager.Logger.Info("ProcessLogout");
                UserModel usrmodel = ConvertHelper.BytesToObject(bytes) as UserModel;
                if (usrmodel != null)
                {
                    using (DBEntity.PTMSEntities context = new PTMSEntities())
                    {
                        RUN_USER_ONLINE usr = context.RUN_USER_ONLINE.FirstOrDefault(n => n.USER_ID == usrmodel.UserID && n.SESSION_ID == usrmodel.UserToken);

                        if (usr != null)
                        {
                            LOG_ACCESS log = context.LOG_ACCESS.FirstOrDefault(n => n.SESSION_ID == usr.SESSION_ID);
                            if (log != null)
                            {
                                log.LOGOUT_TIME = DateTime.UtcNow;
                            }
                            context.RUN_USER_ONLINE.Remove(usr);
                            context.SaveChanges();

                            LoggerManager.Logger.Info("ProcessLogout Deal with Session:" + usr.SESSION_ID);
                        }
                        else
                        {
                            LoggerManager.Logger.Info("ProcessLogout Did not Process Session:" + usrmodel.UserToken);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        [Business(typeName: UserRoute.UserOnlineHeartBeat)]
        public void OnUserOnlineHeartbeat(byte[] bytes, string key)
        {
            LoggerManager.Logger.Info("UserOnlineHeartbeat");
            UserOnlineHeartBeat heartbeat = ConvertHelper.BytesToObject(bytes) as UserOnlineHeartBeat;
            if (heartbeat != null)
            {
                using (DBEntity.PTMSEntities context = new PTMSEntities())
                {
                    List<RUN_USER_ONLINE> users = context.RUN_USER_ONLINE.Where(n => heartbeat.SessionIDs.Contains(n.SESSION_ID)).ToList();
                    foreach (var item in users)
                    {
                        item.UPDATE_TIME = DateTime.UtcNow;
                    }
                    context.SaveChanges();

                    DateTime updatetime = DateTime.UtcNow.AddMinutes(-3);
                    List<RUN_USER_ONLINE> userstodelete = context.RUN_USER_ONLINE.Where(n => n.UPDATE_TIME < updatetime || n.UPDATE_TIME == null).ToList();
                    List<string> sessions = userstodelete.Select(n => n.SESSION_ID).ToList();
                    List<LOG_ACCESS> logs = context.LOG_ACCESS.Where(n => sessions.Contains(n.SESSION_ID)).ToList();
                    foreach (var item in userstodelete)
                    {
                        try
                        {
                            LOG_ACCESS log = logs.FirstOrDefault(n => n.SESSION_ID == item.SESSION_ID);
                            if (log != null)
                            {
                                log.LOGOUT_TIME = DateTime.UtcNow;
                            }
                            LoggerManager.Logger.Info("Remove Session:" + item.SESSION_ID);
                            context.RUN_USER_ONLINE.Remove(item);
                        }
                        catch (Exception ex)
                        {
                            LoggerManager.Logger.Error(ex);
                        }
                    }

                    context.SaveChanges();

                    byte[] sendbytes = ConvertHelper.ObjectToBytes("PTMS");
                    TransforMessage.PublishMessage(Constdefine.APPEXCHANGE, UserRoute.UpdateCache, sendbytes);
                }
            }
        }


        //[Business(typeName: ManagementRoute.ClientStatusChange)]
        //public void ProcessClientStatusChange(byte[] bytes, string key)
        //{
        //    try
        //    {
        //        LoggerManager.Logger.Info("ProcessClientStatusChange");
        //        OrderClient client = ConvertHelper.BytesToObject(bytes) as OrderClient;
        //        if (client != null)
        //        {
        //            using (DBEntity.PTMSEntities context = new PTMSEntities())
        //            {
        //                if (client.Status == StatusEnum.Stop || client.EndTime < DateTime.Now)
        //                {
        //                    List<RUN_USER_ONLINE> usrs = context.RUN_USER_ONLINE.Where(n => n.CLIENT_ID == client.ID).ToList();

        //                    if (usrs.Count > 0)
        //                    {
        //                        ClientUserList userlist = new ClientUserList();
        //                        userlist.UserList = new List<string>();
        //                        foreach (var item in usrs)
        //                        {
        //                            userlist.UserList.Add(item.USER_ID);
        //                            context.RUN_USER_ONLINE.Remove(item);
        //                        }

        //                        context.SaveChanges();

        //                        byte[] sendbytes = ConvertHelper.ObjectToBytes(userlist);

        //                        TransforMessage.PublishMessage(Constdefine.APPEXCHANGE, UserRoute.ForceMultiplyLogout, sendbytes);
        //                    }

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.Logger.Error(ex);
        //    }
        //}

        [Business(typeName: RegisterRoute.OriginalRegisterKey)]
        public void ProcessRegister(byte[] bytes, string key)
        {
            LoggerManager.Logger.Info("ProcessRegister");
            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    var result = DeviceTreatment.Register(context, bytes);

                    if (result != null && result.Message != null && result.Message.Length > 0)
                    {
                        LoggerManager.Logger.Info("ProcessRegister Publish Message to MQ");
                        TransforMessage.PublishMessage(Constdefine.MDVREXCHANGE, key.Replace(RegisterRoute.OriginalRegisterKey, RegisterRoute.HandleRegisterResponseKey), result.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// 套件鉴权
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [Business(typeName: RegisterRoute.OriginalAuthenticateKey)]
        public void ProcessAuthenticate(byte[] bytes, string key)
        {
            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    string strMethod = MethodBase.GetCurrentMethod().ToString();
                    LoggerManager.Logger.Info(strMethod + " is start!");

                    var result = OnOfflineTreatment.Authenticate(context, bytes);

                    if (result != null && result.Message != null && result.Message.Length > 0)
                    {
                        LoggerManager.Logger.Info(strMethod + " Publish Message to MQ");
                        TransforMessage.PublishMessage(Constdefine.MDVREXCHANGE, key.Replace(RegisterRoute.OriginalAuthenticateKey, RegisterRoute.HandleAuthenticateResponseKey) + result.RuleKey, result.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// 定位设备鉴权
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [Business(typeName: GPSRoute.GPSAuthenticateKey)]
        public void ProcessGPSAuthenticate(byte[] bytes, string key)
        {
            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    string strMethod = MethodBase.GetCurrentMethod().ToString();
                    LoggerManager.Logger.Info(strMethod + " is start!");

                    var result = OnOfflineTreatment.GPSAuthenticate(context, bytes);

                    if (result != null && result.Message != null && result.Message.Length > 0)
                    {
                        LoggerManager.Logger.Info(strMethod + " Publish Message to MQ");
                        TransforMessage.PublishMessage(Constdefine.GPSEXCHANGE, key.Replace(GPSRoute.GPSAuthenticateKey, GPSRoute.HandleGPSAuthenticateResponseKey) + result.RuleKey, result.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// 手机unbinding
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [Business(typeName: MobileRoute.MoibleUnRegisterKey)]
        public void ProcessMobileUnRegister(byte[] bytes, string key)
        {
            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    string strMethod = MethodBase.GetCurrentMethod().ToString();
                    LoggerManager.Logger.Info(strMethod + " is start!");
                    string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
                    LoggerManager.Logger.Info(string.Format("Get ProcessMobileUnRegister info:{0}", str));
                    //json -> entity
                    UnRegister unregsiter = JsonHelper.FromJsonString<UnRegister>(str);
                    string[] fields = unregsiter.UID.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string vehicleid = fields[1];
                    string num = fields[0];
                    RUN_MOBILE_WORKING working = context.RUN_MOBILE_WORKING.FirstOrDefault(n => n.MOBILE_NUMBER == num && n.VEHICLE_ID == vehicleid);
                    UnRegisterResponse response = new UnRegisterResponse();
                    response.UID = unregsiter.UID;
                    response.SerialNo = unregsiter.SerialNo;
                    response.RegisterNo = unregsiter.UID;
                    response.SIM = unregsiter.SIM;

                    try
                    {
                        if (working != null)
                        {
                            context.RUN_MOBILE_WORKING.Remove(working);

                            context.Save();

                            response.IsPassed = (int)IsPassed.Yes;

                        }
                    }
                    catch (Exception ex)
                    {
                        response.IsPassed = (int)IsPassed.No;
                    }

                    string s = JsonHelper.ToJsonString(response);
                    //json -> byte[]
                    var msg = System.Text.UTF8Encoding.UTF8.GetBytes(s);

                    LoggerManager.Logger.Info(strMethod + " Publish Message to MQ");
                    TransforMessage.PublishMessage(Constdefine.MOBILEEXCHANGE, key.Replace(MobileRoute.MoibleUnRegisterKey, MobileRoute.MoibleUnRegisterKeyResponse), msg);

                    if (response.IsPassed == (int)IsPassed.Yes)
                    {
                        bool online = context.RUN_MOBILE_WORKING.Any(n => n.VEHICLE_ID == vehicleid && n.ONLINE_FLAG == 1);

                        if (!online)
                        {
                            OnOfflineEx model = new OnOfflineEx();
                            model.MdvrCoreId = num;
                            model.IsOnline = 0;
                            model.OnOffLineTime = DateTime.Now;
                            model.UID = model.MdvrCoreId;

                            model.VehicleId = working.VEHICLE_ID;
                            BSC_CHAUFFEUR chauffeur = context.BSC_CHAUFFEUR.FirstOrDefault(n => n.CELLPHONE == model.UID);
                            if (chauffeur != null)
                            {
                                model.SuiteInfoID = chauffeur.ID;
                            }
                            BSC_VEHICLE vehicle = context.BSC_VEHICLE.FirstOrDefault(n => n.VEHICLE_ID == model.VehicleId);
                            if (vehicle != null)
                            {
                                model.OrganizationID = vehicle.ORGNIZATION_ID;
                            }
                            model.ClientId = working.CLIENT_ID;
                            model.SourceMode = 2;

                            byte[] sendbytes = ConvertHelper.ObjectToBytes(model);

                            TransforMessage.PublishMessage(Constdefine.APPEXCHANGE, key.Replace(MobileRoute.MoibleUnRegisterKey, MonitorRoute.HandleOnOfflineKey), sendbytes);

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        [Business(typeName: UserMessageRoute.CompleteSuiteInstallKey)]
        public void ProcessSuiteInstall(byte[] bytes, string key)
        {
            try
            {
                var mdvrcoresn = ConvertHelper.BytesToObject(bytes) as string;
                using (PTMSEntities context = new PTMSEntities())
                {
                    var vehicle = (from veh in context.BSC_VEHICLE.Where(v => v.VALID == (short)ValidEnum.Valid)
                                   join orgation in context.USR_ORGANIZATION on veh.ORGNIZATION_ID equals orgation.ID
                                   join vt in context.BSC_VEHICLE_TYPE.Where(t => t.VALID == (short)ValidEnum.Valid) on veh.VEHICLE_TYPE equals vt.ID
                                   join mdvr in context.RUN_SUITE_WORKING on veh.VEHICLE_ID equals mdvr.VEHICLE_ID
                                   where mdvr != null && (mdvr.STATUS == (short)DeviceSuiteStatus.Running || mdvr.STATUS == (short)DeviceSuiteStatus.Abnormal) && mdvr.MDVR_CORE_SN == mdvrcoresn
                                   select new Vehicle()
                                   {
                                       OrgnizationId = veh.ORGNIZATION_ID,
                                       OrgnizationName = orgation.NAME,
                                       VehicleId = veh.VEHICLE_ID,
                                       MDVROnline = mdvr.ONLINE_FLAG,
                                       ServiceType = veh.SERVICE_TYPE == null ? VehicleSeviceType.Comercial : (VehicleSeviceType)veh.SERVICE_TYPE,
                                       VehicleType = new VehicleType() { ID = veh.VEHICLE_TYPE == null ? string.Empty : veh.VEHICLE_TYPE },
                                       Region = veh.REGION,
                                       OperationLicense = veh.OPERATION_LICENSE,
                                       Owner = veh.OWNER,
                                       Contact = veh.OWNER,
                                       ContactPhone = veh.CONTACT_PHONE,
                                       ContactAddress = veh.CONTACT_ADDRESS,
                                       BrandModel = veh.BRAND_MODEL,
                                       VehicleSn = veh.VEHICLE_SN,
                                       EngineId = veh.ENGINE_ID,
                                       StartYear = veh.START_YEAR,
                                       VehicleStatus = veh.VEHICLE_STATUS == null ? VehicleConditionType.Available : (VehicleConditionType)veh.VEHICLE_STATUS,
                                       Note = veh.NOTE,
                                       VehicleTypeDescribe = vt.NAME,
                                       VehicleTypeImage=vt.ICON,
                                       MDVR_SN = mdvr.MDVR_CORE_SN,
                                       DistrictCode = veh.DISTRICT_CODE,
                                   }).FirstOrDefault();

                    var gpsvehicle = (from gps in context.RUN_GPS_WORKING
                                      where gps.VEHICLE_ID == vehicle.VehicleId
                                      select new Vehicle()
                                      {
                                          GPSOnline = gps.ONLINE_FLAG,
                                          GPS_SN = gps.GPS_SN,
                                      }).FirstOrDefault();

                    if (gpsvehicle != null)
                    {
                        vehicle.GPSOnline = gpsvehicle.GPSOnline;
                        vehicle.GPS_SN = gpsvehicle.GPS_SN;
                    }

                    var mobilevehicle = (from mobile in context.RUN_MOBILE_WORKING
                                         where mobile.VEHICLE_ID == vehicle.VehicleId
                                         select new Vehicle()
                                         {
                                             MobileOnline = mobile.ONLINE_FLAG,
                                             Mobile_SN = mobile.MOBILE_NUMBER,
                                         }).FirstOrDefault();

                    if (mobilevehicle != null)
                    {
                        vehicle.MobileOnline = mobilevehicle.MobileOnline;
                        vehicle.Mobile_SN = mobilevehicle.Mobile_SN;
                    }

                    byte[] sendbytes = ConvertHelper.ObjectToBytes(vehicle);

                    TransforMessage.PublishMessage(Constdefine.APPEXCHANGE, key.Replace(UserMessageRoute.CompleteSuiteInstallKey, UserMessageRoute.InstallCompleteNotification), sendbytes);
                }

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        [Business(typeName: UserMessageRoute.CompleteGPSInstallKey)]
        public void ProcessGPSInstall(byte[] bytes, string key)
        {
            try
            {
                var gpsid = ConvertHelper.BytesToObject(bytes) as string;
                using (PTMSEntities context = new PTMSEntities())
                {
                    var gpsvehicle = (from veh in context.BSC_VEHICLE.Where(v => v.VALID == (short)ValidEnum.Valid)
                                      join orgation in context.USR_ORGANIZATION on veh.ORGNIZATION_ID equals orgation.ID
                                      join vt in context.BSC_VEHICLE_TYPE.Where(t => t.VALID == (short)ValidEnum.Valid) on veh.VEHICLE_TYPE equals vt.ID
                                      join gps in context.RUN_GPS_WORKING on veh.VEHICLE_ID equals gps.VEHICLE_ID
                                      where gps.STATUS == (short)DeviceSuiteStatus.Running && gps.GPS_ID == gpsid
                                      select new Vehicle()
                                      {
                                          OrgnizationId = veh.ORGNIZATION_ID,
                                          OrgnizationName = orgation.NAME,
                                          VehicleId = veh.VEHICLE_ID,
                                          GPSOnline = gps.ONLINE_FLAG,
                                          ServiceType = veh.SERVICE_TYPE == null ? VehicleSeviceType.Comercial : (VehicleSeviceType)veh.SERVICE_TYPE,
                                          VehicleType = new VehicleType() { ID = veh.VEHICLE_TYPE == null ? string.Empty : veh.VEHICLE_TYPE },
                                          Region = veh.REGION,
                                          OperationLicense = veh.OPERATION_LICENSE,
                                          Owner = veh.OWNER,
                                          Contact = veh.OWNER,
                                          ContactPhone = veh.CONTACT_PHONE,
                                          ContactAddress = veh.CONTACT_ADDRESS,
                                          BrandModel = veh.BRAND_MODEL,
                                          VehicleSn = veh.VEHICLE_SN,
                                          EngineId = veh.ENGINE_ID,
                                          StartYear = veh.START_YEAR,
                                          VehicleStatus = veh.VEHICLE_STATUS == null ? VehicleConditionType.Available : (VehicleConditionType)veh.VEHICLE_STATUS,
                                          Note = veh.NOTE,
                                          VehicleTypeDescribe = vt.NAME,
                                          VehicleTypeImage = vt.ICON,
                                          GPS_SN = gps.GPS_SN,
                                          DistrictCode = veh.DISTRICT_CODE,
                                      }).FirstOrDefault();

                    if (gpsvehicle != null)
                    {
                        var suitevehicle = (from mdvr in context.RUN_SUITE_WORKING
                                            where mdvr != null && (mdvr.STATUS == (short)DeviceSuiteStatus.Running || mdvr.STATUS == (short)DeviceSuiteStatus.Abnormal) && mdvr.VEHICLE_ID == gpsvehicle.VehicleId
                                            select new Vehicle()
                                            {
                                                MDVROnline = mdvr.ONLINE_FLAG,
                                                MDVR_SN = mdvr.MDVR_CORE_SN,
                                            }).FirstOrDefault();

                        if (suitevehicle != null)
                        {
                            gpsvehicle.MDVR_SN = suitevehicle.MDVR_SN;
                            gpsvehicle.MDVROnline = suitevehicle.MDVROnline;
                        }

                        var mobilevehicle = (from mobile in context.RUN_MOBILE_WORKING
                                             where mobile.VEHICLE_ID == gpsvehicle.VehicleId
                                             select new Vehicle()
                                             {
                                                 MobileOnline = mobile.ONLINE_FLAG,
                                                 Mobile_SN = mobile.MOBILE_NUMBER,
                                             }).FirstOrDefault();

                        if (mobilevehicle != null)
                        {
                            gpsvehicle.MobileOnline = mobilevehicle.MobileOnline;
                            gpsvehicle.Mobile_SN = mobilevehicle.Mobile_SN;
                        }
                        byte[] sendbytes = ConvertHelper.ObjectToBytes(gpsvehicle);
                        TransforMessage.PublishMessage(Constdefine.APPEXCHANGE, key.Replace(UserMessageRoute.CompleteSuiteInstallKey, UserMessageRoute.InstallCompleteNotification), sendbytes);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }
        /// <summary>
        /// 安全套件上下线
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [Business(typeName: MonitorRoute.OriginalOnOfflineKey)]
        public void ProcessOnOffline(byte[] bytes, string key)
        {
            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    string strMethod = MethodBase.GetCurrentMethod().ToString();
                    LoggerManager.Logger.Info(strMethod + " is start!");
                    ReturnInfo returnInfo = new ReturnInfo();
                    string ruleKey = null;
                    //byte[] -> json
                    string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
                    LoggerManager.Logger.Info(string.Format("Get OnOffline info:{0}", str));
                    //json -> entity
                    OnOffLineData tempmodel = JsonHelper.FromJsonString<OnOffLineData>(str);


                    if (tempmodel != null)
                    {
                        OnOfflineEx model = new OnOfflineEx();
                        model.UID = tempmodel.UID;
                        model.MdvrCoreId = tempmodel.UID;
                        model.IsOnline = Convert.ToInt32(tempmodel.IsOnline);
                        model.OnOffLineTime = DateTime.Parse(tempmodel.OnOffLineTime);
                        model.MdvrCoreId = tempmodel.UID;

                        if (CacheDataManager.Suites.ContainsKey(model.MdvrCoreId))
                        {
                            SuiteCache suite = CacheDataManager.Suites[model.MdvrCoreId];
                            model.VehicleId = suite.VEHICLE_ID;
                            model.SuiteInfoID = suite.SUITE_INFO_ID;
                            model.ClientId = suite.CLIENT_ID;
                            model.SourceMode = 0;
                            model.OrganizationID = suite.ORGNIZATION_ID;

                            MonitorRepository mrepostiory = new MonitorRepository();

                            LoggerManager.Logger.Info(string.Format("Before Add OnOffline to database!UID:{0}", str));
                            //check && storge && response 
                            mrepostiory.AddSuiteOnOffline(context, model);
                            LoggerManager.Logger.Info(string.Format("After Add OnOffline to database!UID:{0}", str));
                            returnInfo.RuleKey = ruleKey;
                            returnInfo.Message = ConvertHelper.ObjectToBytes(model);
                        }
                    }

                    if (returnInfo != null && returnInfo.Message != null && returnInfo.Message.Length > 0)
                    {
                        LoggerManager.Logger.Info(strMethod + " Publish Message to MQ");
                        TransforMessage.PublishMessage(Constdefine.APPEXCHANGE, key.Replace(MonitorRoute.OriginalOnOfflineKey, MonitorRoute.HandleOnOfflineKey), returnInfo.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// 定位设备上下线
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [Business(typeName: GPSRoute.GPSOnOffLine)]
        public void ProcessGPSOnOffline(byte[] bytes, string key)
        {
            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    string strMethod = MethodBase.GetCurrentMethod().ToString();
                    LoggerManager.Logger.Info(strMethod + " is start!");
                    ReturnInfo returnInfo = new ReturnInfo();
                    string ruleKey = null;
                    //byte[] -> json
                    string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
                    LoggerManager.Logger.Info(string.Format("Get OnOffline info:{0}", str));
                    //json -> entity
                    OnOffLineData tempmodel = JsonHelper.FromJsonString<OnOffLineData>(str);


                    if (tempmodel != null)
                    {
                        OnOfflineEx model = new OnOfflineEx();
                        model.UID = tempmodel.UID;
                        model.MdvrCoreId = tempmodel.UID;
                        model.IsOnline = Convert.ToInt32(tempmodel.IsOnline);
                        model.OnOffLineTime = DateTime.Parse(tempmodel.OnOffLineTime);
                        model.MdvrCoreId = tempmodel.UID;

                        if (!CacheDataManager.GPSs.ContainsKey(model.MdvrCoreId))
                        {
                            var item = context.RUN_GPS_WORKING.FirstOrDefault(x => x.GPS_SN == model.MdvrCoreId);
                            if (item != null)
                            {
                                CacheDataManager.GPSs.Add(item.GPS_SN, item);
                            }
                        }

                        if (CacheDataManager.GPSs.ContainsKey(model.MdvrCoreId))
                        {
                            RUN_GPS_WORKING working = CacheDataManager.GPSs[model.MdvrCoreId];
                            model.VehicleId = working.VEHICLE_ID;
                            model.SuiteInfoID = working.GPS_SN;
                            model.ClientId = working.CLIENT_ID;
                            model.SourceMode = 1;
                            BSC_VEHICLE vehicle = context.BSC_VEHICLE.FirstOrDefault(n => n.VEHICLE_ID == model.VehicleId);
                            if (vehicle != null)
                            {
                                model.OrganizationID = vehicle.ORGNIZATION_ID;
                            }
                            MonitorRepository mrepostiory = new MonitorRepository();

                            LoggerManager.Logger.Info(string.Format("Before Add OnOffline to database!UID:{0}", str));
                            //check && storge && response 
                            mrepostiory.AddGPSOnOffline(context, model);
                            LoggerManager.Logger.Info(string.Format("After Add OnOffline to database!UID:{0}", str));
                            returnInfo.RuleKey = ruleKey;
                            returnInfo.Message = ConvertHelper.ObjectToBytes(model);
                        }
                    }

                    if (returnInfo != null && returnInfo.Message != null && returnInfo.Message.Length > 0)
                    {
                        LoggerManager.Logger.Info(strMethod + " Publish Message to MQ");
                        TransforMessage.PublishMessage(Constdefine.APPEXCHANGE, key.Replace(GPSRoute.GPSOnOffLine, MonitorRoute.HandleOnOfflineKey), returnInfo.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// 手机上下线
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [Business(typeName: MobileRoute.MobileOnOffLine)]
        public void ProcessMobileOnOffline(byte[] bytes, string key)
        {
            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    string strMethod = MethodBase.GetCurrentMethod().ToString();
                    LoggerManager.Logger.Info(strMethod + " is start!");
                    ReturnInfo returnInfo = new ReturnInfo();
                    string ruleKey = null;
                    //byte[] -> json
                    string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
                    LoggerManager.Logger.Info(string.Format("Get ProcessMobileOnOffline info:{0}", str));
                    //json -> entity
                    OnOffLineData tempmodel = JsonHelper.FromJsonString<OnOffLineData>(str);

                    if (tempmodel != null)
                    {
                        OnOfflineEx model = new OnOfflineEx();
                        string[] fields = tempmodel.UID.Split(";".ToCharArray());

                        model.MdvrCoreId = fields[0];
                        model.IsOnline = Convert.ToInt32(tempmodel.IsOnline);
                        model.OnOffLineTime = DateTime.Parse(tempmodel.OnOffLineTime);
                        model.UID = model.MdvrCoreId;

                        if (!CacheDataManager.Mobiles.ContainsKey(model.MdvrCoreId))
                        {
                            lock (CacheDataManager.Mobiles)
                            {
                                RUN_MOBILE_WORKING mobile = context.RUN_MOBILE_WORKING.FirstOrDefault(n => n.MOBILE_NUMBER == model.MdvrCoreId);
                                if (mobile != null)
                                {
                                    CacheDataManager.Mobiles.Add(mobile.MOBILE_NUMBER, mobile);
                                }
                            }
                        }

                        if (CacheDataManager.Mobiles.ContainsKey(model.MdvrCoreId))
                        {
                            RUN_MOBILE_WORKING working = CacheDataManager.Mobiles[model.MdvrCoreId];
                            model.VehicleId = working.VEHICLE_ID;
                            BSC_CHAUFFEUR chauffeur = context.BSC_CHAUFFEUR.FirstOrDefault(n => n.CELLPHONE == model.UID);
                            if (chauffeur != null)
                            {
                                model.SuiteInfoID = chauffeur.ID;
                            }
                            BSC_VEHICLE vehicle = context.BSC_VEHICLE.FirstOrDefault(n => n.VEHICLE_ID == model.VehicleId);
                            if (vehicle != null)
                            {
                                model.OrganizationID = vehicle.ORGNIZATION_ID;
                            }
                            model.ClientId = working.CLIENT_ID;
                            model.SourceMode = 2;

                            MonitorRepository mrepostiory = new MonitorRepository();

                            LoggerManager.Logger.Info(string.Format("Before Add OnOffline to database!UID:{0}", str));
                            //check && storge && response 
                            mrepostiory.AddMoibleOnOffline(context, model);
                            LoggerManager.Logger.Info(string.Format("After Add OnOffline to database!UID:{0}", str));

                            if (context.RUN_MOBILE_WORKING.Any(n => n.VEHICLE_ID == model.VehicleId && n.ONLINE_FLAG == 1))
                            {
                                model.IsOnline = 1;
                            }
                            else
                            {
                                model.IsOnline = 0;
                            }

                            returnInfo.RuleKey = ruleKey;
                            returnInfo.Message = ConvertHelper.ObjectToBytes(model);
                        }
                    }

                    if (returnInfo != null && returnInfo.Message != null && returnInfo.Message.Length > 0)
                    {

                        string publishkey = key.Replace(MobileRoute.MobileOnOffLine, MonitorRoute.HandleOnOfflineKey);
                        LoggerManager.Logger.Info(strMethod + " Publish Message to MQ " + publishkey);
                        TransforMessage.PublishMessage(Constdefine.APPEXCHANGE, publishkey, returnInfo.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        [Business(typeName: AlarmRoute.OriginalAlarmInfoKey)]
        public void ProcessAlarmInfo(byte[] bytes, string key)
        {
            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    LoggerManager.Logger.Info(MethodBase.GetCurrentMethod().ToString());
                    var result = Gsafety.PTMS.AnalysisLib.Utility.AlarmTreatment.AlarmInfo(context, bytes);

                    if (result != null && result.Message != null && result.Message.Length > 0)
                    {
                        TransforMessage.PublishMessage(Constdefine.APPEXCHANGE, result.RuleKey, result.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        [Business(typeName: AlarmRoute.MobileAlarmKey)]
        public void ProcessMobileAlarmInfo(byte[] bytes, string key)
        {
            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    LoggerManager.Logger.Info(MethodBase.GetCurrentMethod().ToString());
                    var result = Gsafety.PTMS.AnalysisLib.Utility.AlarmTreatment.MobileAlarmInfo(context, bytes);

                    if (result != null && result.Message != null && result.Message.Length > 0)
                    {
                        TransforMessage.PublishMessage(Constdefine.APPEXCHANGE, result.RuleKey, result.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        [Business(typeName: AlarmRoute.CompleteAlarm)]
        public void ProcessCompleteAlarm(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info(MethodBase.GetCurrentMethod().ToString());
                TransforMessage.PublishMessage(Constdefine.APPEXCHANGE, AlarmRoute.CompleteAlarmNotice, bytes);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }


        [Business(typeName: AlertRoute.CompleteAlert)]
        public void ProcessCompleteAlert(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info(MethodBase.GetCurrentMethod().ToString());
                TransforMessage.PublishMessage(Constdefine.APPEXCHANGE, AlertRoute.CompleteAlertNotice, bytes);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        [Business(typeName: AlertRoute.OriginalBusinessAlertKey)]
        public void ProcessBusinessAlert(byte[] bytes, string key)
        {
            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    LoggerManager.Logger.Info(MethodBase.GetCurrentMethod().ToString());
                    string msgId = string.Empty;
                    var result = Gsafety.PTMS.AnalysisLib.Utility.AlertTreatment.BusinessAlert(context, bytes, out msgId);

                    if (result != null && result.Message != null && result.Message.Length > 0)
                    {
                        LoggerManager.Logger.Info("ProcessOnOffline  Before Publish Message " + AlertRoute.OriginalBusinessAlertKey + "to MQ by ProcessAlarm");

                        TransforMessage.PublishMessage(Constdefine.APPEXCHANGE, result.RuleKey, result.Message);

                        LoggerManager.Logger.Info("ProcessOnOffline  After Publish Message " + AlertRoute.OriginalBusinessAlertKey + " to MQ by ProcessAlarm");
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        [Business(typeName: AlertRoute.OriginalDeviceAlertKey)]
        public void ProcessDeviceAlert(byte[] bytes, string key)
        {
            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    LoggerManager.Logger.Info(MethodBase.GetCurrentMethod().ToString());
                    string msgId = string.Empty;
                    var result = Gsafety.PTMS.AnalysisLib.Utility.AlertTreatment.DeviceAlert(context, bytes, out msgId);

                    if (result != null && result.Message != null && result.Message.Length > 0)
                    {
                        LoggerManager.Logger.Info("ProcessOnOffline  Before Publish Message " + AlertRoute.OriginalBusinessAlertKey + "to MQ by ProcessAlarm");

                        TransforMessage.PublishMessage(Constdefine.APPEXCHANGE, result.RuleKey, result.Message);

                        LoggerManager.Logger.Info("ProcessOnOffline  After Publish Message " + AlertRoute.OriginalBusinessAlertKey + " to MQ by ProcessAlarm");
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }


        //[Business(typeName: MonitorRoute.HandlePolygonsRegionKey)]
        //public void PolygonsRegion(byte[] bytes, string key)
        //{
        //    try
        //    {
        //        using (PTMSEntities context = new PTMSEntities())
        //        {
        //            LoggerManager.Logger.Info(MethodBase.GetCurrentMethod().ToString());

        //            ElectricFenceCommand.CommandSend(context, bytes, key);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.Logger.Error(ex);
        //    }
        //}

        //[Business(typeName: MonitorRoute.OriginalRouteInfoKey)]
        //public void RouteInfo(byte[] bytes, string key)
        //{
        //    try
        //    {
        //        using (PTMSEntities context = new PTMSEntities())
        //        {
        //            LoggerManager.Logger.Info(MethodBase.GetCurrentMethod().ToString());

        //            RouteInfoCommand.CommandSend(context, bytes, key);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.Logger.Error(ex);
        //    }
        //}

        //[Business(typeName: MonitorRoute.OriginalSetTermParamKey)]
        //public void SetTermParam(byte[] bytes, string key)
        //{
        //    try
        //    {
        //        using (PTMSEntities context = new PTMSEntities())
        //        {
        //            LoggerManager.Logger.Info(MethodBase.GetCurrentMethod().ToString());

        //            ParamCommand.CommandSend(context, bytes, key);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.Logger.Error(ex);
        //    }
        //}

        [Business(typeName: MonitorRoute.OriginalQueryPartParamKey)]
        public void QueryPartParam(byte[] bytes, string key)
        {
            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    LoggerManager.Logger.Info(MethodBase.GetCurrentMethod().ToString());

                    var result = CommandHelper.QueryPartParam(context, bytes);

                    if (result != null && result.Message != null && result.Message.Length > 0)
                    {
                        TransforMessage.PublishMessage(Constdefine.MDVREXCHANGE, key.Replace(MonitorRoute.OriginalQueryPartParamKey, MonitorRoute.HandleQueryPartParamKey) + result.RuleKey, result.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        [Business(typeName: MonitorRoute.OriginalQueryParaResponseKey)]
        public void QueryParaResponse(byte[] bytes, string key)
        {
            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    LoggerManager.Logger.Info(MethodBase.GetCurrentMethod().ToString());

                    var result = CommandHelper.QueryParaResponse(context, bytes);

                    if (result != null && result.Message != null && result.Message.Length > 0)
                    {
                        TransforMessage.PublishMessage(Constdefine.MDVREXCHANGE, key.Replace(MonitorRoute.OriginalQueryParaResponseKey, MonitorRoute.HandleQueryPartParamKey), result.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// 通用回复
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [Business(typeName: MonitorRoute.OriginalGenenalResponseKey)]
        public void GenenalResponse(byte[] bytes, string key)
        {
            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    LoggerManager.Logger.Info(MethodBase.GetCurrentMethod().ToString());

                    string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
                    GenenalResponseEx response = JsonHelper.FromJsonString<GenenalResponseEx>(str);
                    int packageseq = response.ResponseSerialNo;
                    switch (response.MessageID)
                    {
                        case "33027":
                            CommandFactory.GetCommand(CommandTypeEnum.CommandParam).OnReply(context, bytes, key);
                            break;
                        case "34308":
                        case "34309":
                            CommandFactory.GetCommand(CommandTypeEnum.ElectricFence).OnReply(context, bytes, key);
                            break;
                        case "34310":
                        case "34311":
                            CommandFactory.GetCommand(CommandTypeEnum.Route).OnReply(context, bytes, key);
                            break;
                        case "33536":
                            CommandFactory.GetCommand(CommandTypeEnum.MDVRMessage).OnReply(context, bytes, key);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }


        /// <summary>
        /// 获取视频文件列表
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [Business(typeName: VideoRoute.QueryMdvrFileListAppKey)]
        public void ProcessQueryMDVRFileList(byte[] bytes, string key)
        {
            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    LoggerManager.Logger.Info("ProcessQueryMDVRFileList");
                    VideoCommand.SendQueryFile(context, bytes, key);
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// 获取视频文件列表回复
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [Business(typeName: VideoRoute.QueryMdvrFileListMDVRResponseKey)]
        public void ProcessQueryMDVRFileListResponse(byte[] bytes, string key)
        {
            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    LoggerManager.Logger.Info("ProcessQueryMDVRFileListResponse");
                    VideoCommand.OnReplyQueryFile(context, bytes, key);
                }

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// 拍照回复
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [Business(typeName: VideoRoute.TakePictureMDVRResponseKey)]
        public void ProcessTakePictureMDVRResponse(byte[] bytes, string key)
        {
            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    LoggerManager.Logger.Info("ProcessTakePictureMDVRResponse");
                    VideoCommand.OnReplyTakePicture(context, bytes, key);
                }

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// 拍照
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [Business(typeName: VideoRoute.TakePictureAppKey)]
        public void ProcessTakePicture(byte[] bytes, string key)
        {
            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    LoggerManager.Logger.Info("ProcessTakePicture");
                    VideoCommand.SendTakePicture(context, bytes, key);
                }

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// 下载MDVR视频文件
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [Business(typeName: VideoRoute.DownloadMdvrFileAppKey)]
        public void ProcessDownloadMdvrFile(byte[] bytes, string key)
        {
            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    LoggerManager.Logger.Info("ProcessQueryMDVRFileListResponse");
                    VideoCommand.SendDownloadMdvrFile(context, bytes, key);
                }

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// 设置报警参数
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [Business(typeName: InstallRoute.SetAlarmParaAppKey)]
        public void ProcessSetAlarmParaCommand(byte[] bytes, string key)
        {
            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    LoggerManager.Logger.Info("ProcessSetAlarmParaCommand");
                    SetAlarmParaCommand.Send(context, bytes, key);
                }

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// 设置报警参数回复
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [Business(typeName: InstallRoute.SetAlarmParaMDVRResponseKey)]
        public void ProcessSetAlarmParaCommandResponse(byte[] bytes, string key)
        {
            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    LoggerManager.Logger.Info("ProcessSetAlarmParaCommandResponse");
                    SetAlarmParaCommand.OnReply(context, bytes, key);
                }

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }
    }
}
