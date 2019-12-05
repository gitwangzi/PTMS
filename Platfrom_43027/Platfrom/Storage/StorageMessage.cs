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
using System.Collections;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.PTMS.Analysis.Storage;
using Gsafety.Common.Util;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;

namespace Gsafety.PTMS.Analysis.Storage
{
    public static class StorageMessage
    {
        private static IModel _businessCh;
        private static IModel _userMessageCh;

        private static IConnection _userMessageConn;
        private static IConnection _businessConn;

        private static QueueingBasicConsumer _businessConsumer;
        private static QueueingBasicConsumer _userConsumer;

        private static bool _businessStop = false;
        private static bool _userMessageStop = false;

        private static bool _businessConnected = false;
        private static bool _userMessageConnected = false;

        private static int _gpsCount = 0;

        private static DateTime _gpsTime = DateTime.Now;

        public static BaseService baseservice = new BaseService();
        private static string _businessQueue = "Storage.Share";
        private static string _userMessageQueue = "Storage.Private.";

        /// <summary>
        /// Start
        /// </summary>
        public static void Start()
        {
            try
            {
                //working suite cache
                _userMessageQueue += System.Net.Dns.GetHostName();
                OracleHelper.ExecuteSqlWithCache();
                Task.Factory.StartNew(() => { DequeueBusinessMessage(); });
                Task.Factory.StartNew(() => { DequeueUserMessage(); });
                LoggerManager.Logger.Info("Storage service starts successfully!");
            }
            catch (Exception ex)
            {
                baseservice.Error(ex, "StorageService");
                LoggerManager.Logger.Error("An exception occurred when the service starts!" + ex);
            }
        }

        /// <summary>
        /// Stop
        /// </summary>
        public static void Stop()
        {
            try
            {
                _businessStop = true;
                ClearBusinessConn();
                AlarmGpsStorage.Save();

                _userMessageStop = true;
                ClearUserMessageConn();
                LoggerManager.Logger.Info("service Stop successfully!");
            }
            catch (Exception ex)
            {
                baseservice.Error(ex, "StorageService");
                LoggerManager.Logger.Error("An exception occurred when the stopping service!" + ex);
            }
        }

        /// <summary>
        /// dispose connetiongstring
        /// </summary>
        public static void ClearUserMessageConn()
        {
            try
            {
                if (_userMessageConn != null)
                {
                    if (_userMessageConn.IsOpen)
                    {
                        _userMessageConn.Close();
                    }
                    _userMessageConn.Dispose();
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// dispose connectiongstring 
        /// </summary>
        public static void ClearBusinessConn()
        {
            try
            {
                if (_businessConn != null)
                {
                    if (_businessConn.IsOpen)
                    {
                        _businessConn.Close();
                    }
                    _businessConn.Dispose();
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

                if (_userMessageConnected)
                    return _userMessageConnected;

                _userMessageConn = null;

                while (_userMessageConn == null)
                {
                    if (_userMessageStop)
                        return false;
                    _userMessageConn = new MQFactory().CreateConnection();
                }
                ////CreateConnection
                _userMessageConn = new MQFactory().CreateConnection();

                _userMessageCh = _userMessageConn.CreateModel();

                _userMessageCh.ExchangeDeclare(Constdefine.APPEXCHANGE, "topic", true);

                _userMessageCh.QueueDeclare(_userMessageQueue, true, true, true, null);

                ////the device is switched to the state of repair + ".*.*.*"
                _userMessageCh.QueueBind(_userMessageQueue, Constdefine.APPEXCHANGE, UserMessageRoute.SuiteMaintainKey);
                ////device install succeed
                _userMessageCh.QueueBind(_userMessageQueue, Constdefine.APPEXCHANGE, UserMessageRoute.StartSuiteInstallKey);
                _userMessageCh.QueueBind(_userMessageQueue, Constdefine.APPEXCHANGE, UserMessageRoute.DeleteSuiteInstallKey);

                _userMessageCh.QueueBind(_userMessageQueue, Constdefine.APPEXCHANGE, UserMessageRoute.StartGPSInstallKey);
                _userMessageCh.QueueBind(_userMessageQueue, Constdefine.APPEXCHANGE, UserMessageRoute.DeleteGPSInstallKey);
                _userMessageCh.QueueBind(_userMessageQueue, Constdefine.APPEXCHANGE, UserMessageRoute.GPSMaintainKey);

                _userMessageCh.BasicQos(0, 1, false);
                _userConsumer = new QueueingBasicConsumer(_userMessageCh);
                _userMessageCh.BasicConsume(_userMessageQueue, false, _userConsumer);
                _userMessageConnected = true;
            }
            catch (Exception ex)
            {
                baseservice.Error(ex, "StorageService");
                _userMessageConnected = false;
                ClearUserMessageConn();
                LoggerManager.Logger.Error(ex);
            }
            return _userMessageConnected;
        }

        /// <summary>
        /// Init Business Queue
        /// </summary>
        private static bool InitBusinessQueue()
        {
            try
            {

                if (_businessConnected)
                    return _businessConnected;

                _businessConn = null;

                while (_businessConn == null)
                {
                    if (_businessStop)
                        return false;
                    _businessConn = new MQFactory().CreateConnection();
                }

                _businessCh = _businessConn.CreateModel();

                _businessCh.ExchangeDeclare(Constdefine.APPEXCHANGE, "topic", true);

                _businessCh.QueueDeclare(_businessQueue, true, false, false, null);

                _businessCh.QueueBind(_businessQueue, Constdefine.GPSEXCHANGE, GPSRoute.SuiteKey + "*");

                _businessCh.QueueBind(_businessQueue, Constdefine.MOBILEEXCHANGE, GPSRoute.MobileKey + "*");

                _businessCh.QueueBind(_businessQueue, Constdefine.GPSEXCHANGE, GPSRoute.GPSKey + "*");

                _businessCh.BasicQos(0, 2000, false);
                _businessConsumer = new QueueingBasicConsumer(_businessCh);
                _businessCh.BasicConsume(_businessQueue, false, _businessConsumer);
                _businessConnected = true;
            }
            catch (Exception ex)
            {
                baseservice.Error(ex, "StorageService");
                _businessConnected = false;
                ClearBusinessConn();
                LoggerManager.Logger.Error(ex);
                LoggerManager.Logger.Error(ex);
            }
            return _businessConnected;
        }

        /// <summary>
        /// recyling processing business news
        /// </summary>
        private static void DequeueBusinessMessage()
        {
            object sender;
            bool rec;
            //InitBusinessQueue();
            while (true)
            {
                if (_businessStop) break;
                if (!InitBusinessQueue())
                    continue;
                rec = false;
                try
                {
                    rec = (_businessConsumer.Queue.Dequeue(10, out sender));
                    if (rec)
                    {
                        BasicDeliverEventArgs e = sender as BasicDeliverEventArgs;

                        IBasicProperties props = e.BasicProperties;

                        IBasicProperties bp = _businessCh.CreateBasicProperties();

                        bp.Type = "1";
                        bp.Priority = 1;

                        byte[] bytes = (byte[])(e.Body);
                        if (e.RoutingKey.Contains(GPSRoute.SuiteKey))
                        {
                            ////Suite
                            if (AlarmGpsStorage.AddSuite(bytes))
                                _gpsCount++;
                            SaveGPS();
                        }
                        else if (e.RoutingKey.Contains(GPSRoute.MobileKey))
                        {
                            ////Mobile
                            if (AlarmGpsStorage.AddMobile(bytes))
                                _gpsCount++;
                            SaveGPS();
                        }
                        else if (e.RoutingKey.Contains(GPSRoute.GPSKey))
                        {
                            //GPS
                            if (AlarmGpsStorage.AddGPS(bytes))
                                _gpsCount++;
                            SaveGPS();
                        }
                        _businessCh.BasicAck(e.DeliveryTag, false);
                    }
                    else
                    {
                        SaveGPS();
                    }
                }
                catch (Exception ex)
                {
                    _businessConnected = false;
                    ClearBusinessConn();
                    LoggerManager.Logger.Error(ex);
                    LoggerManager.Logger.Error("An error occurs,recconnect in mq.......");
                    baseservice.Error(ex, "StorageService");
                    //InitBusinessQueue();
                    //continue;
                }
            }
        }

        /// <summary>
        /// recyling processing user news
        /// </summary>
        private static void DequeueUserMessage()
        {
            object sender;
            bool rec;
            //InitUserMessageQueue();
            while (true)
            {
                if (_userMessageStop) break;


                if (!InitUserMessageQueue())
                    continue;
                rec = false;
                try
                {
                    rec = (_userConsumer.Queue.Dequeue(10, out sender));
                    if (rec)
                    {
                        BasicDeliverEventArgs e = sender as BasicDeliverEventArgs;

                        IBasicProperties props = e.BasicProperties;

                        byte[] bytes = (byte[])(e.Body);

                        IBasicProperties bp = _userMessageCh.CreateBasicProperties();

                        bp.Type = "1";
                        bp.Priority = 1;

                        string key = ConvertHelper.BytesToObject(bytes) as string;

                        if (e.RoutingKey.Contains(UserMessageRoute.StartSuiteInstallKey))
                        {
                            OracleHelper.ProcessSuiteInstall(key);
                        }
                        else if (e.RoutingKey.Contains(UserMessageRoute.SuiteMaintainKey))
                        {
                            ////the device is switched to the state of repair
                            OracleHelper.ProcessSuiteUnInstall(key);
                        }
                        else if (e.RoutingKey.Contains(UserMessageRoute.DeleteSuiteInstallKey))
                        {
                            OracleHelper.ProcessSuiteUnInstall(key);
                        }
                        else if (e.RoutingKey.Contains(UserMessageRoute.StartGPSInstallKey))
                        {
                            ////device install succeed
                            OracleHelper.ProcessGPSInstall(key);
                        }
                        else if (e.RoutingKey.Contains(UserMessageRoute.GPSMaintainKey))
                        {
                            ////the device is switched to the state of repair
                            OracleHelper.ProcessGPSUnInstall(key);
                        }
                        else if (e.RoutingKey.Contains(UserMessageRoute.DeleteGPSInstallKey))
                        {
                            OracleHelper.ProcessGPSUnInstall(key);
                        }


                        ////reply
                        _userMessageCh.BasicAck(e.DeliveryTag, false);
                    }
                }
                catch (Exception ex)
                {
                    baseservice.Error(ex, "StorageService");
                    _userMessageConnected = false;
                    ClearUserMessageConn();
                    LoggerManager.Logger.Error(ex);
                    LoggerManager.Logger.Error("An error occurs,recconnect in mq.......");
                }
            }
        }

        /// <summary>
        /// Save GPS
        /// </summary>
        private static void SaveGPS()
        {
            if (_gpsCount >= 2000 || (DateTime.Now > _gpsTime.AddSeconds(1)) && (_gpsCount > 0))
            {
                LoggerManager.Logger.Info("AlarmGps start save:" + _gpsCount.ToString());
                AlarmGpsStorage.Save();
                LoggerManager.Logger.Info("AlarmGps saved:" + _gpsCount.ToString());
                _gpsCount = 0;
                _gpsTime = DateTime.Now;
            }
            else
            {
                //LoggerManager.Logger.Info("not save:_gpsCount " + _gpsCount.ToString());
            }
        }
    }
}
