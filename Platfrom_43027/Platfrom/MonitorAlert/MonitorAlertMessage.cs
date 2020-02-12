/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7afbfba4-cdcc-4aa0-972b-f03d3be355c2      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.MonitorAlert
/////    Project Description:    
/////             Class Name: MonitorAlertMessage
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/30 13:31:18
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/30 13:31:18
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Gsafety.Common.Logging;
using Gsafety.MQ;
using Gsafety.PTMS.Analysis.Helper;
using RabbitMQ.Client.Events;
using Gsafety.Common.Util;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.PTMS.Traffic.Repository;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Configuration;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.PTMS.Common.Data;

namespace Gsafety.PTMS.MonitorAlert
{
    public class MonitorAlertMessage
    {
        private static IModel _ch;
        private static IConnection _conn;
        private static QueueingBasicConsumer _consumer;

        private static bool _stop = false;
        private static bool _BusinessConnected = false;

        private static MonitorAlertGenerator _monitorAlertGenerator;
        private static TrafficRepository trafficeServer;
        private static string QUEUE = "MonitorAlert.Share";
        private static bool _GenerateFenceAlert;
        private static bool _GenerateRouteAlert;
        private static bool _GenerateSpeedAlert;
        private static bool _GenerateMonitorPointAlert;

        /// <summary>
        /// 开始
        /// </summary>
        public static void Start()
        {
            try
            {
                _GenerateFenceAlert = Convert.ToBoolean(ConfigurationManager.AppSettings["GenerateFenceAlert"]);
                _GenerateRouteAlert = Convert.ToBoolean(ConfigurationManager.AppSettings["GenerateRouteAlert"]);
                _GenerateSpeedAlert = Convert.ToBoolean(ConfigurationManager.AppSettings["GenerateSpeedAlert"]);
                _GenerateMonitorPointAlert = Convert.ToBoolean(ConfigurationManager.AppSettings["GenerateMonitorPointAlert"]);

                _monitorAlertGenerator = new MonitorAlertGenerator();
                trafficeServer = new TrafficRepository();
                LoggerManager.Logger.Info("Service starts successfully!");
            }
            catch (Exception ex)
            {
                //写日志
                LoggerManager.Logger.Error("An exception occurred when the service starts!" + ex);
            }
        }

        /// <summary>
        /// 结束
        /// </summary>
        public static void Shop()
        {
            try
            {
                _stop = true;
                ClearConn();
                
                LoggerManager.Logger.Info("service Stop successfully!");
            }
            catch (Exception ex)
            {
                //写日志
                LoggerManager.Logger.Error("An exception occurred when the stopping service!" + ex);
            }
        }

        /// <summary>
        /// 释放连接
        /// </summary>
        public static void ClearConn()
        {
            try
            {
                _BusinessConnected = false;
                if (_conn != null)
                {
                    if (_conn.IsOpen)
                    {
                        _conn.Close();
                    }
                    _conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                //写日志
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// 初始化队列
        /// </summary>
        private static bool Init()
        {
            try
            {
                if (_BusinessConnected)
                    return _BusinessConnected;
                ////创建连接
                _conn = new MQFactory().CreateConnection();

                _ch = _conn.CreateModel();

                _ch.ExchangeDeclare(Constdefine.APPEXCHANGE, "topic", true);
                _ch.ExchangeDeclare(Constdefine.GPSEXCHANGE, "topic", true);

                ////声明队列
                _ch.QueueDeclare(QUEUE, true, false, false, null);

                ////绑定路由
                ////GPS
                //_ch.QueueBind(QUEUE, Constdefine.GPSEXCHANGE, GPSRoute.OriginalANTGPSKey + "*");
                //_ch.QueueBind(QUEUE, Constdefine.GPSEXCHANGE, GPSRoute.OriginalGPSKey + "*");

                _ch.QueueBind(QUEUE, Constdefine.GPSEXCHANGE, GPSRoute.SuiteKey + "*");

                //_ch.QueueBind(QUEUE, Constdefine.MOBILEEXCHANGE, GPSRoute.MobileKey + "*");

                _ch.QueueBind(QUEUE, Constdefine.GPSEXCHANGE, GPSRoute.GPSKey + "*");


                //订阅围栏相关的消息
                //_ch.QueueBind(QUEUE, Constdefine.APPEXCHANGE, MonitorRoute.OriginalFenceKey + "*");
                _ch.QueueBind(QUEUE, Constdefine.APPEXCHANGE, UserMessageRoute.OriginalRouteKey);
                _ch.QueueBind(QUEUE, Constdefine.APPEXCHANGE, UserMessageRoute.OriginalTravePlanKey);            ////////行驶计划
                _ch.QueueBind(QUEUE, Constdefine.APPEXCHANGE, UserMessageRoute.OriginalMonitorPointKey);         ////////监控点
                _ch.QueueBind(QUEUE, Constdefine.APPEXCHANGE, UserMessageRoute.OriginalElectronicFenceKey);      ////////电子围栏

                _ch.BasicQos(0, 2000, false);
                _consumer = new QueueingBasicConsumer(_ch);
                _ch.BasicConsume(QUEUE, false , _consumer);
                _BusinessConnected = true;

            }
            catch (Exception ex)
            {
                _BusinessConnected = false;
                ClearConn();
                LoggerManager.Logger.Error(ex);
            }
            return _BusinessConnected;
        }

        /// <summary>
        /// 循环处理消息
        /// </summary>
        public static void DequeueMessage()
        {
            object sender;
            bool rec ;
            while (true)
            {
                if (_stop)
                    return;
                if (!Init())
                    continue;

                rec = false;
                try
                {                    
                    rec = (_consumer.Queue.Dequeue(100, out sender));
                    //if (rec == false)
                    //    _BusinessConnected = false;
                    if (rec)
                    {
                        BasicDeliverEventArgs e = sender as BasicDeliverEventArgs;

                        IBasicProperties props = e.BasicProperties;

                        byte[] bytes = (byte[])(e.Body);

                        IBasicProperties bp = _ch.CreateBasicProperties();

                        bp.Type = "1";
                        bp.Priority = 1;

                        //处理AntGPS消息
                        if (e.RoutingKey.Contains(GPSRoute.GPSKey) || e.RoutingKey.Contains(GPSRoute.SuiteKey))
                        {
                            try
                            {
                                //LoggerManager.Logger.Info("AddSuite.......");
                                string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
                                LoggerManager.Logger.Info(str);
                                //json -> entity
                                GpsInfo gpsinfo = JsonHelper.FromJsonString<Gsafety.PTMS.Common.Data.GpsInfo>(str);

                                if (gpsinfo != null)
                                {
                                    //LoggerManager.Logger.Info("gpsinfo is not empty");
                                    GPS gps = GetGPS(gpsinfo);

                                    _monitorAlertGenerator.HandleGPS(gps);
                                }
                                else
                                {
                                    LoggerManager.Logger.Warn(string.Format("Converted gps to entity is empty,string:{0}", str));
                                }
                            }
                            catch (Exception ex)
                            {
                                LoggerManager.Logger.Error(ex);
                            }
                            
                           
                        }
                        //处理电子围栏消息
                        if (e.RoutingKey.Contains(UserMessageRoute.OriginalElectronicFenceKey))
                        {
                            if (_GenerateFenceAlert)
                            {
                                string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
                                var model = ConvertHelper.BytesToObject(bytes) as ElectricFenceCMD;
                                string mdvrid = model.DvId;
                                string gpsid = "";
                                try
                                {                                    
                                    //if (trafficeServer.GetGPSID(mdvrid) != null)
                                    //{
                                    //    gpsid = trafficeServer.GetGPSID(mdvrid);
                                    //}
                                    //LoggerManager.Logger.Info(string.Format("GPS Time ={0},MDVRCOREID={1}", model.GpsTime, model.DvId)); 
                                    _monitorAlertGenerator.MonitorGrid.UpdateFence(gpsid,model);
                                }
                                catch (Exception ex)
                                {
                                    LoggerManager.Logger.Error(ex.Message);
                                }
                            }
                        }

                        if (e.RoutingKey.Contains(UserMessageRoute.OriginalSpeedLimitKey))
                        {
                            if (_GenerateMonitorPointAlert)
                            {
                                try
                                {
                                    //string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
                                    //var model = ConvertHelper.BytesToObject(bytes) as ElectricFenceCMD;
                                    //LoggerManager.Logger.Info(string.Format("GPS Time ={0},MDVRCOREID={1}", model.GpsTime, model.DvId));    
                                    //_monitorAlertGenerator.MonitorGrid.UpdateSpeed(model);

                                }
                                catch (Exception ex)
                                {
                                    LoggerManager.Logger.Error(ex.Message);
                                }
                            }
                        }
                        //处理路线的消息
                        if (e.RoutingKey.Contains(UserMessageRoute.OriginalRouteKey))
                        {
                            if (_GenerateRouteAlert)
                            {
                                string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
                                var model = ConvertHelper.BytesToObject(bytes) as RouteCMD;
                                string mdvrid = model.MdvrCoreId;
                                try
                                {
                                    string gpsid = "";

                                    //if (trafficeServer.GetGPSID(mdvrid) != null)
                                    //{
                                    //    gpsid = trafficeServer.GetGPSID(mdvrid);                                        
                                    //}
                                    //LoggerManager.Logger.Info(string.Format("GPS Time ={0},MDVRCOREID={1}", model.GpsTime, model.DvId)); 
                                    _monitorAlertGenerator.MonitorGrid.UpdateRoute(gpsid, mdvrid, model);
                                }
                                catch (Exception ex)
                                {
                                    LoggerManager.Logger.Error(ex.Message);                                  
                                }
                            }
                        }
                       
                       
                        ////回复
                        _ch.BasicAck(e.DeliveryTag, false);
                    }
                }
                catch (Exception ex)
                {                   
                    //_BusinessConnected = false;
                    ClearConn();                 
                    //日志
                    LoggerManager.Logger.Error(ex);
                    LoggerManager.Logger.Error("An error occurs,recconnect in mq.......");                 
                }           
            }
        }
              
        /// <summary>
        /// 写入消息
        /// </summary>
        /// <param name="exchange">exchange名称</param>
        /// <param name="route">路由</param>
        /// <param name="msg">消息</param>
        public static void PublishMessage(string exchange, string route, byte[] msg)
        {
            try
            {
                IBasicProperties bp = _ch.CreateBasicProperties();

                bp.Type = "1";
                bp.Priority = 1;
                _ch.BasicPublish(exchange, route, bp, msg);
                LoggerManager.Logger.Info(string.Format("exchange={0},route={1}",exchange,route));
            }
            catch (Exception ex)
            {
                //日志
                LoggerManager.Logger.Error(ex);
            }
        }
        private static GPS GetGPS(GpsInfo gpsinfo)
        {
            GPS gps = new GPS();
            gps.AlarmFlag = gpsinfo.AlarmFlag;
            gps.Direction = gpsinfo.Direction;
            gps.Status = gpsinfo.Status;
            gps.SourceMode = 0;
            try
            {
                gps.GpsTime = DateTime.Parse(gpsinfo.GpsTime);
            }
            catch (Exception)
            {

            }

            gps.Valid = gpsinfo.Valid;
            gps.Height = gpsinfo.Height;
            gps.Latitude = gpsinfo.Latitude;
            gps.Longitude = gpsinfo.Longitude;
            gps.Speed = gpsinfo.Speed;
            gps.UID = gpsinfo.UID;
            gps.DeviceID = gpsinfo.UID;
            return gps;
        }

        public static byte[] ObjectToBytes(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                return ms.GetBuffer();
            }
        }
    }
}
