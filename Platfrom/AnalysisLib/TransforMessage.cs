/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: d6742f65-a254-41b8-b76b-afb4f1c2d2f6      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis
/////    Project Description:    
/////             Class Name: TransforMessage
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/7/31 14:57:37
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/7/31 14:57:37
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Gsafety.MQ;
using RabbitMQ.Client.Events;
using System.Threading;
using Gsafety.Common.Logging;
using System.Reflection;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.BaseInformation.Repository;
using System.Collections;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.AnalysisLib.Command;
using System.Net;

namespace Gsafety.PTMS.AnalysisLib
{
    public static class TransforMessage
    {
        private static IModel _shareCh;
        private static IModel _privateCh;

        private static IConnection _privateConn;
        private static IConnection _shareConn;

        private static QueueingBasicConsumer _shareConsumer;
        private static QueueingBasicConsumer _privateConsumer;

        private static Dictionary<string, MethodInfo> _sharemethodInfo;
        private static ShareBusinessProcess _businessProcess;

        private static Dictionary<string, MethodInfo> _privatemethodInfo;
        private static PrivateBusinessProcess _pirvateProcess;

        private static bool _shareStop = false;
        private static bool _privateStop = false;

        private static bool _ShareConnected = false;
        private static bool _PrivateConnected = false;

        private static string _shareQueue = "Business.Share";
        private static string _privateQueue = "Business.Private.";

        private static object obj = new object();

        public static void Start()
        {
            try
            {
                _privateQueue += Dns.GetHostName();
                ////Get all the business methods to be executed
                _sharemethodInfo = MethodHelper.GetShareMethodInfo();
                _businessProcess = new ShareBusinessProcess();
                _privatemethodInfo = MethodHelper.GetPrivateMethodInfo();
                _pirvateProcess = new PrivateBusinessProcess();
                ////cache data
                if (CacheData())
                {
                    Task.Factory.StartNew(() => { DequeueShareMessage(); });
                    Task.Factory.StartNew(() => { DequeuePrivateMessage(); });
                    Task.Factory.StartNew(() => { CommandBase.RunJob(); });
                    LoggerManager.Logger.Info("TransforMessage service starts successfully!");
                }
                else
                {
                    LoggerManager.Logger.Error("TransforMessage service failed to Start!");
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("An exception occurred when the TransforMessage service starts!" + ex);
            }
        }

        /// <summary>
        /// Shop
        /// </summary>
        public static void Stop()
        {
            try
            {
                _shareStop = true;
                ClearBusinessConn();
                _privateStop = true;
                ClearUserMessageConn();

                LoggerManager.Logger.Info("TransforMessage service Stop successfully!");
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("An exception occurred when the stopping TransforMessage service!" + ex);
            }
        }

        /// <summary>
        /// dispose connetiong
        /// </summary>
        public static void ClearUserMessageConn()
        {
            try
            {
                _PrivateConnected = false;
                if (_privateConn != null)
                {
                    if (_privateConn.IsOpen)
                    {
                        _privateConn.Close();
                    }
                    _privateConn.Dispose();
                    _privateConn = null;
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// dispose connetiong
        /// </summary>
        public static void ClearBusinessConn()
        {
            try
            {
                _ShareConnected = false;
                if (_shareConn != null)
                {
                    if (_shareConn.IsOpen)
                    {
                        _shareConn.Close();
                    }
                    _shareConn.Dispose();
                    _shareConn = null;
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// Init User Message Queue
        /// </summary>
        private static bool InitUserMessageQueue()
        {
            try
            {
              
                if (_PrivateConnected)
                    return _PrivateConnected;
                ////Create Connection

                _privateConn = null;
                while (_privateConn == null)
                {
                    if (_privateStop)
                        return false;
                    _privateConn = new MQFactory().CreateConnection();
                }


                _privateCh = _privateConn.CreateModel();

                _privateCh.ExchangeDeclare(Constdefine.MDVREXCHANGE, "topic", true);
                _privateCh.ExchangeDeclare(Constdefine.APPEXCHANGE, "topic", true);

                ////Business queue
                _privateCh.QueueDeclare(_privateQueue, true, true, true, null);

                _privateCh.QueueBind(_privateQueue, Constdefine.APPEXCHANGE, UserRoute.UserLogin);
                _privateCh.QueueBind(_privateQueue, Constdefine.APPEXCHANGE, UserRoute.UserLogout);
                _privateCh.QueueBind(_privateQueue, Constdefine.APPEXCHANGE, UserRoute.UpdateCache);
                _privateCh.QueueBind(_privateQueue, Constdefine.APPEXCHANGE, ManagementRoute.ClientModeChange);



                ////Device Maintain + ".*.*.*"
                _privateCh.QueueBind(_privateQueue, Constdefine.APPEXCHANGE, UserMessageRoute.SuiteMaintainKey);
                ////devic install seccessful
                _privateCh.QueueBind(_privateQueue, Constdefine.APPEXCHANGE, UserMessageRoute.CompleteSuiteInstallKey);
                ////start install
                _privateCh.QueueBind(_privateQueue, Constdefine.APPEXCHANGE, UserMessageRoute.StartSuiteInstallKey);
                ////delete install
                _privateCh.QueueBind(_privateQueue, Constdefine.APPEXCHANGE, UserMessageRoute.DeleteSuiteInstallKey);

                ////Device Maintain + ".*.*.*"
                _privateCh.QueueBind(_privateQueue, Constdefine.APPEXCHANGE, UserMessageRoute.StartGPSInstallKey);
                ////devic install seccessful
                _privateCh.QueueBind(_privateQueue, Constdefine.APPEXCHANGE, UserMessageRoute.GPSMaintainKey);
                ////start install
                _privateCh.QueueBind(_privateQueue, Constdefine.APPEXCHANGE, UserMessageRoute.CompleteGPSInstallKey);
                ////delete install
                _privateCh.QueueBind(_privateQueue, Constdefine.APPEXCHANGE, UserMessageRoute.DeleteGPSInstallKey);


                _privateCh.QueueBind(_privateQueue, Constdefine.MDVREXCHANGE, MonitorRoute.OriginalOnOfflineKey + "*");

                _privateCh.BasicQos(0, 1, false);
                _privateConsumer = new QueueingBasicConsumer(_privateCh);
                _privateCh.BasicConsume(_privateQueue, false, _privateConsumer);
                _PrivateConnected = true;
            }
            catch (Exception ex)
            {
                _PrivateConnected = false;
                ClearUserMessageConn();
                LoggerManager.Logger.Error(ex);
            }
            return _PrivateConnected;
        }

        /// <summary>
        /// Init Business Queue
        /// </summary>
        private static bool InitBusinessQueue()
        {
            try
            {
               
                if (_ShareConnected)
                    return _ShareConnected;

                _shareConn = null;
                while (_shareConn == null)
                {
                    if (_shareStop)
                        return false;
                    _shareConn = new MQFactory().CreateConnection();
                }

                _shareCh = _shareConn.CreateModel();

                _shareCh.ExchangeDeclare(Constdefine.MDVREXCHANGE, "topic", true);
                _shareCh.ExchangeDeclare(Constdefine.APPEXCHANGE, "topic", true);
                _shareCh.ExchangeDeclare(Constdefine.MOBILEEXCHANGE, "topic", true);
                ////business queue
                _shareCh.QueueDeclare(_shareQueue, true, false, false, null);

                _shareCh.QueueBind(_shareQueue, Constdefine.APPEXCHANGE, UserRoute.UserLogin);
                _shareCh.QueueBind(_shareQueue, Constdefine.APPEXCHANGE, UserRoute.UserLogout);
                _shareCh.QueueBind(_shareQueue, Constdefine.APPEXCHANGE, UserRoute.UserOnlineHeartBeat);

                //_shareCh.QueueBind(_shareQueue, Constdefine.APPEXCHANGE, ManagementRoute.ClientStatusChange);

                _shareCh.QueueBind(_shareQueue, Constdefine.MDVREXCHANGE, RegisterRoute.OriginalUnRegisterKey + "*");
                _shareCh.QueueBind(_shareQueue, Constdefine.MDVREXCHANGE, RegisterRoute.OriginalRegisterKey + "*");
                _shareCh.QueueBind(_shareQueue, Constdefine.MDVREXCHANGE, RegisterRoute.OriginalUnRegisterKey + "*");
                _shareCh.QueueBind(_shareQueue, Constdefine.MDVREXCHANGE, RegisterRoute.OriginalAuthenticateKey + "*");
                _shareCh.QueueBind(_shareQueue, Constdefine.GPSEXCHANGE, RegisterRoute.OriginalAuthenticateKey + "*");
                _shareCh.QueueBind(_shareQueue, Constdefine.GPSEXCHANGE, GPSRoute.GPSAuthenticateKey + "*");
                _shareCh.QueueBind(_shareQueue, Constdefine.MOBILEEXCHANGE, MobileRoute.MoibleUnRegisterKey + "*");

                _shareCh.QueueBind(_shareQueue, Constdefine.MDVREXCHANGE, MonitorRoute.OriginalOnOfflineKey + "*");

                _shareCh.QueueBind(_shareQueue, Constdefine.APPEXCHANGE, VideoRoute.QueryMdvrFileListAppKey + "*");
                _shareCh.QueueBind(_shareQueue, Constdefine.MDVREXCHANGE, VideoRoute.QueryMdvrFileListMDVRResponseKey + "*");
                _shareCh.QueueBind(_shareQueue, Constdefine.APPEXCHANGE, VideoRoute.DownloadMdvrFileAppKey + "*");

                _shareCh.QueueBind(_shareQueue, Constdefine.APPEXCHANGE, VideoRoute.TakePictureAppKey + "*");
                _shareCh.QueueBind(_shareQueue, Constdefine.MDVREXCHANGE, VideoRoute.TakePictureMDVRResponseKey + "*");



                _shareCh.QueueBind(_shareQueue, Constdefine.APPEXCHANGE, UserMessageRoute.CompleteGPSInstallKey);
                _shareCh.QueueBind(_shareQueue, Constdefine.APPEXCHANGE, UserMessageRoute.CompleteSuiteInstallKey);

                _shareCh.QueueBind(_shareQueue, Constdefine.MDVREXCHANGE, AlarmRoute.OriginalAlarmInfoKey + "*");
                _shareCh.QueueBind(_shareQueue, Constdefine.APPEXCHANGE, AlarmRoute.CompleteAlarm + "*");
                _shareCh.QueueBind(_shareQueue, Constdefine.APPEXCHANGE, AlertRoute.CompleteAlert + "*");
                _shareCh.QueueBind(_shareQueue, Constdefine.MDVREXCHANGE, AlertRoute.OriginalBusinessAlertKey + "*");
                _shareCh.QueueBind(_shareQueue, Constdefine.MDVREXCHANGE, AlertRoute.OriginalDeviceAlertKey + "*");


                _shareCh.QueueBind(_shareQueue, Constdefine.MDVREXCHANGE, MonitorRoute.OriginalPolygonsRegionKey + "*");
                _shareCh.QueueBind(_shareQueue, Constdefine.MDVREXCHANGE, MonitorRoute.OriginalRouteInfoKey + "*");

                _shareCh.QueueBind(_shareQueue, Constdefine.MDVREXCHANGE, MonitorRoute.OriginalQueryPartParamKey + "*");
                _shareCh.QueueBind(_shareQueue, Constdefine.MDVREXCHANGE, MonitorRoute.OriginalQueryParaResponseKey + "*");
                _shareCh.QueueBind(_shareQueue, Constdefine.MDVREXCHANGE, MonitorRoute.OriginalGenenalResponseKey + "*");

                _shareCh.QueueBind(_shareQueue, Constdefine.APPEXCHANGE, InstallRoute.SetAlarmParaAppKey + "*");
                _shareCh.QueueBind(_shareQueue, Constdefine.MDVREXCHANGE, InstallRoute.SetAlarmParaMDVRResponseKey + "*");

                _shareCh.QueueBind(_shareQueue, Constdefine.MOBILEEXCHANGE, AlarmRoute.MobileAlarmKey + "*");
                _shareCh.QueueBind(_shareQueue, Constdefine.GPSEXCHANGE, GPSRoute.GPSOnOffLine + "*");
                _shareCh.QueueBind(_shareQueue, Constdefine.MOBILEEXCHANGE, MobileRoute.MobileOnOffLine + "*");

                //////binding route
                ////// process alarm
                //_businessCh.QueueBind(_businessQueue, Constdefine.MDVREXCHANGE, AlarmRoute.OriginalAlarmKey + "*");
                //////Original Transparent CMD Key
                //_businessCh.QueueBind(_businessQueue, Constdefine.MDVREXCHANGE, MonitorRoute.OriginalTransparentCMDKey + "*");
                //////online 
                //_businessCh.QueueBind(_businessQueue, Constdefine.MDVREXCHANGE, MonitorRoute.OriginalOnlineKey + "*");
                //////offline A1
                //_businessCh.QueueBind(_businessQueue, Constdefine.MDVREXCHANGE, MonitorRoute.OriginalOfflineA1Key + "*");
                //////offline A2
                //_businessCh.QueueBind(_businessQueue, Constdefine.MDVREXCHANGE, MonitorRoute.OriginalOfflineA2Key + "*");
                //////shutdown send up V20

                ////// Camera No Signal Alert Key
                //_businessCh.QueueBind(_businessQueue, Constdefine.MDVREXCHANGE, AlertRoute.OriginalCameraNoSignalAlertKey + "*");
                //////Original Camera Occlusion Alert Key
                //// Protocol is not supported

                //////Original Temperature Alert Key
                //_businessCh.QueueBind(_businessQueue, Constdefine.MDVREXCHANGE, AlertRoute.OriginalTemperatureAlertKey + "*");
                //////Original Mdvr MemoryCard Error Alert Key
                //_businessCh.QueueBind(_businessQueue, Constdefine.MDVREXCHANGE, AlertRoute.OriginalMdvrMemoryCardErrorAlertKey + "*");
                //////Original Gps Receiver Fault Alert Key
                //_businessCh.QueueBind(_businessQueue, Constdefine.MDVREXCHANGE, AlertRoute.OriginalGpsReceiverFaultAlertKey + "*");
                //// Protocol is not supported
                //////Original Voltage Abnormal AlertKey
                //_businessCh.QueueBind(_businessQueue, Constdefine.MDVREXCHANGE, AlertRoute.OriginalVoltageAbnormalAlertKey + "*");



                //////Original Over Speed AlerKey
                //_businessCh.QueueBind(_businessQueue, Constdefine.MDVREXCHANGE, AlertRoute.OriginalOverSpeedAlerKey + "*");
                //////Original Region AlertKey
                //_businessCh.QueueBind(_businessQueue, Constdefine.MDVREXCHANGE, AlertRoute.OriginalRegionAlertKey + "*");

                //////Original Remove Device Suite AlertKey
                //_businessCh.QueueBind(_businessQueue, Constdefine.MDVREXCHANGE, AlertRoute.OriginalRemoveDeviceSuiteAlertKey + "*");

                //////miles
                //_businessCh.QueueBind(_businessQueue, Constdefine.MDVREXCHANGE, MonitorRoute.OriginalRapterMileageKey + "*");

                _shareCh.BasicQos(0, 200, false);
                _shareConsumer = new QueueingBasicConsumer(_shareCh);
                _shareCh.BasicConsume(_shareQueue, false, _shareConsumer);
                _ShareConnected = true;

            }
            catch (Exception ex)
            {
                _ShareConnected = false;
                ClearBusinessConn();
                LoggerManager.Logger.Error(ex);
            }
            return _ShareConnected;
        }

        /// <summary>
        /// working suite cache
        /// </summary>
        private static bool CacheData()
        {
            using (PTMSEntities entites = new PTMSEntities())
            {
                try
                {
                    lock (CacheDataManager.GPSs)
                    {
                        foreach (var item in entites.RUN_GPS_WORKING.ToList())
                        {
                            CacheDataManager.GPSs.Add(item.GPS_SN, item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                    return false;
                }

                try
                {
                    lock (CacheDataManager.Mobiles)
                    {
                        foreach (var item in entites.RUN_MOBILE_WORKING.ToList())
                        {
                            CacheDataManager.Mobiles.Add(item.MOBILE_NUMBER, item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                    return false;
                }

                try
                {
                    lock (CacheDataManager.Suites)
                    {
                        var suites = from w in entites.RUN_SUITE_WORKING
                                     join v in entites.BSC_VEHICLE on w.VEHICLE_ID equals v.VEHICLE_ID
                                     join s in entites.BSC_DEV_SUITE on w.SUITE_INFO_ID equals s.SUITE_INFO_ID
                                     join t in entites.BSC_VEHICLE_TYPE on v.VEHICLE_TYPE equals t.ID
                                     select new SuiteCache
                                     {
                                         CLIENT_ID = w.CLIENT_ID,
                                         CONTACT_PHONE = v.CONTACT_PHONE,
                                         DISTRICT_CODE = v.DISTRICT_CODE,
                                         ORGNIZATION_ID = v.ORGNIZATION_ID,
                                         SUITE_ID = s.SUITE_ID,
                                         Status = w.STATUS.Value,
                                         SuiteStatus = s.STATUS,
                                         OWNER = v.OWNER,
                                         SUITE_INFO_ID = s.SUITE_INFO_ID,
                                         VEHICLE_ID = v.VEHICLE_ID,
                                         MDVR_Core_SN = w.MDVR_CORE_SN,
                                         VehicleSn=v.VEHICLE_SN,
                                         VehicleType=t.NAME
                                     };
                        var list = suites.ToList();
                        foreach (var item in list)
                        {
                            CacheDataManager.Suites.Add(item.MDVR_Core_SN, item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                    return false;
                }

                try
                {
                    lock (CacheDataManager.Users)
                    {
                        foreach (var item in entites.RUN_USER_ONLINE.ToList())
                        {
                            CacheDataManager.Users.Add(item.USER_ID, item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                    return false;
                }

                try
                {
                    lock (CacheDataManager.Districts)
                    {
                        foreach (var item in entites.BSC_DISTRICT)
                        {
                            CacheDataManager.Districts.Add(item.CODE, item.NAME);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                    return false;
                }

                try
                {
                    lock (CacheDataManager.ClientModes)
                    {
                        foreach (var item in entites.BSC_ORDER_CLIENT)
                        {
                            CacheDataManager.ClientModes.Add(item.ID, item.TANSFER_MODE);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Dequeue Business Message
        /// </summary>
        private static void DequeueShareMessage()
        {
            object sender;
            bool rec;
            while (true)
            {
                if (_shareStop)
                    return;
                if (!InitBusinessQueue())
                    continue;

                rec = false;
                try
                {
                    rec = _shareConsumer.Queue.Dequeue(10, out sender);

                    if (rec)
                    {
                        BasicDeliverEventArgs e = sender as BasicDeliverEventArgs;

                        IBasicProperties props = e.BasicProperties;

                        byte[] bytes = (byte[])(e.Body);

                        IBasicProperties bp = _shareCh.CreateBasicProperties();

                        bp.Type = "1";
                        bp.Priority = 1;

                        Task.Factory.StartNew(() => { InvokeShareMethod(e.RoutingKey, bytes); });

                        ////reply
                        _shareCh.BasicAck(e.DeliveryTag, false);
                    }
                }
                catch (Exception ex)
                {
                    if (!_shareStop)
                    {
                        LoggerManager.Logger.Error(ex);
                        ClearBusinessConn();
                    }
                }
            }
        }

        /// <summary>
        /// Circulation processing User Message
        /// </summary>
        private static void DequeuePrivateMessage()
        {
            object sender;
            bool rec = false;
            while (true)
            {
                if (_privateStop)
                    return;
                if (!InitUserMessageQueue())
                    continue;
                try
                {
                    rec = (_privateConsumer.Queue.Dequeue(10, out sender));
                    if (rec)
                    {
                        BasicDeliverEventArgs e = sender as BasicDeliverEventArgs;
                        IBasicProperties props = e.BasicProperties;
                        byte[] bytes = (byte[])(e.Body);
                        IBasicProperties bp = _privateCh.CreateBasicProperties();
                        bp.Type = "1";
                        bp.Priority = 1;
                        Task task = Task.Factory.StartNew(() =>
                        {
                            InvokePrivateMethod(e.RoutingKey, bytes);
                        });
                        ////reply
                        _privateCh.BasicAck(e.DeliveryTag, false);
                    }
                }
                catch (Exception ex)
                {
                    if (!_privateStop)
                    {
                        ClearUserMessageConn();
                        LoggerManager.Logger.Error(ex);
                    }
                }
            }
        }

        /// <summary>
        /// execute method by key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="msg">message</param>
        private static void InvokeShareMethod(string key, object msg)
        {
            try
            {
                if (_sharemethodInfo != null)
                {
                    var method = _sharemethodInfo.Where(x => key.ToUpper().Contains(x.Key.ToUpper())).FirstOrDefault().Value;
                    if (method != null)
                    {
                        method.Invoke(_businessProcess, new object[] { msg, key });
                    }
                    else
                    {
                        LoggerManager.Logger.Warn(string.Format("According to the route {0} not find method", key));
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// execute method by key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="msg">message</param>
        private static void InvokePrivateMethod(string key, object msg)
        {
            try
            {
                if (_privatemethodInfo != null)
                {
                    var method = _privatemethodInfo.Where(x => key.ToUpper().Contains(x.Key.ToUpper())).FirstOrDefault().Value;
                    if (method != null)
                    {
                        method.Invoke(_pirvateProcess, new object[] { msg, key });
                    }
                    else
                    {
                        LoggerManager.Logger.Warn(string.Format("According to the route {0} not find method", key));
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// publish message
        /// </summary>
        /// <param name="exchange">exchange name</param>
        /// <param name="route">route key</param>
        /// <param name="msg">message</param>
        public static void PublishMessage(string exchange, string route, byte[] msg)
        {
            try
            {
                IBasicProperties bp = _shareCh.CreateBasicProperties();
                bp.Type = "1";
                bp.Priority = 1;
                _shareCh.BasicPublish(exchange, route, bp, msg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }
    }
}
