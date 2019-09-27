/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 22a38852-9f81-4773-8ed6-d542ba41bb57      
/////             clrversion: 4.0.30319.34003
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: CommandManagement
/////    Project Description:    
/////             Class Name: CommandManager
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/2/7 04:37:50
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/2/7 04:37:50
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.Common.Logging;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.MQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.CommandManagement
{
    public class CommandManager
    {
        private static IModel _Channel;
        private static IConnection _Connection;
        private static QueueingBasicConsumer _MessageConsumer;
        private static string _CommandQueue = "Task.PTMS.CommandMessage";
        private static Dictionary<string, MethodInfo> _methodInfo;
        private static MessageProcessing _messageProcess;

        private static bool _StopState = false;
        private static bool _ConnectedState = false;

        public static void Start(PTMSEntities context)
        {
            try
            {
                ////Get all the business methods to be executed
                _methodInfo = MethodHelper.GetMethodInfo();

                _messageProcess = new MessageProcessing();
                ////cache data
                SuiteStatusInfoManage.Init();
                WaitSendManager.Init(context);
                SendingManager.Init(context);
                BasicInfoManager.Init(context);
                Task.Factory.StartNew(() => { DequeueMessage(); });
                Task.Factory.StartNew(() => { System.Threading.Thread.Sleep(2000); WaitSendManager.WaitTimeoutControl(context); });
                Task.Factory.StartNew(() => { System.Threading.Thread.Sleep(2000); SendingManager.SendTimeoutControl(context); });
                LoggerManager.Logger.Info("Command Manager service starts successfully!");
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("Command Manager service failed to start!" + ex);
            }
        }

        public static void End()
        {
            try
            {
                _StopState = true;
                ClearConn();
                LoggerManager.Logger.Info("Command Manager service stop successfully!");
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("Command Manager service stop failure!" + ex);
            }
        }

        /// <summary>
        /// Initialzaiongbusiness maessage queue
        /// </summary>
        private static bool InitBusinessQueue()
        {
            try
            {
                if (_ConnectedState)
                    return _ConnectedState;
                ////create link
                _Connection = new MQFactory().CreateConnection();

                _Channel = _Connection.CreateModel();

                LoggerManager.Logger.Error("Channel Created!");

                _Channel.ExchangeDeclare(Constdefine.MDVREXCHANGE, "topic", true);
                _Channel.ExchangeDeclare(Constdefine.APPEXCHANGE, "topic", true);

                //Disclaimer business message queue
                _Channel.QueueDeclare(_CommandQueue, true, false, false, null);

                #region routing bind

                ////////////online information
                _Channel.QueueBind(_CommandQueue, Constdefine.MDVREXCHANGE, MonitorRoute.OriginalOnlineKey + "*");
                ////////////offline A1
                _Channel.QueueBind(_CommandQueue, Constdefine.MDVREXCHANGE, MonitorRoute.OriginalOfflineA1Key + "*");
                ////////////offling A2
                _Channel.QueueBind(_CommandQueue, Constdefine.MDVREXCHANGE, MonitorRoute.OriginalOfflineA2Key + "*");

                ////////////electronic fences,set up monitoring points
                _Channel.QueueBind(_CommandQueue, Constdefine.APPEXCHANGE, MonitorRoute.OriginalFenceKey + "*");
                ////////////electronic fences,，monitoring point setting results reply(equipment resplies)
                _Channel.QueueBind(_CommandQueue, Constdefine.MDVREXCHANGE, ReplyRoute.OriginalFenceyDeviceReplyKey + "*");
                ////electronic fence reply（business process response, including monitoring points)
                _Channel.QueueBind(_CommandQueue, Constdefine.APPEXCHANGE, ReplyRoute.OriginalFenceyBusinessReplyKey + "*");
                ////////////speeding settings
                _Channel.QueueBind(_CommandQueue, Constdefine.APPEXCHANGE, MonitorRoute.OriginalSettingOverSpeedCMDKey + "*");
                ////////////Overspeed setting result replies
                _Channel.QueueBind(_CommandQueue, Constdefine.MDVREXCHANGE, ReplyRoute.OriginalSettingOverSpeedReplyKey + "*");
                if (ConfigInfo.OpenMDVRGPSMoniter)
                {
                    ////////////Daily monitoring of the GPS information request MDVR
                    _Channel.QueueBind(_CommandQueue, Constdefine.APPEXCHANGE, UserMessageRoute.OriginalLocationMonitorKey);
                    ////////////Cancellation vehicle location monitoring
                    _Channel.QueueBind(_CommandQueue, Constdefine.APPEXCHANGE, UserMessageRoute.CancelLocationMonitorKey);
                    ////////////Daily monitoring of the GPS information request MDVR Reply
                    //_Channel.QueueBind(_CommandQueue, Constdefine.MDVREXCHANGE, ReplyRoute.HandleGPSMonitorReplyKey + "*");
                }
                //_Channel.QueueBind(_CommandQueue, Constdefine.APPEXCHANGE, SettingRoute.GpsSettingObjectKey + "*");
                //_Channel.QueueBind(_CommandQueue, Constdefine.MDVREXCHANGE, ReplyRoute.HandleGPSMonitorReplyKey + "*");

                //_Channel.QueueBind(_CommandQueue, Constdefine.APPEXCHANGE, SettingRoute.TemperatureSettingObjectKey + "*");
                //_Channel.QueueBind(_CommandQueue, Constdefine.MDVREXCHANGE, ReplyRoute.HandleTemperatureReplyKey + "*");

                //_Channel.QueueBind(_CommandQueue, Constdefine.APPEXCHANGE, SettingRoute.OneKeyAlarmSettingObjectKey + "*");
                //_Channel.QueueBind(_CommandQueue, Constdefine.MDVREXCHANGE, ReplyRoute.HandleDelayAlarmReplyKey + "*");
                //_Channel.QueueBind(_CommandQueue, Constdefine.MDVREXCHANGE, ReplyRoute.HandleOneKeyAlarmReplyKey + "*");

                //   _Channel.QueueBind(_CommandQueue, Constdefine.APPEXCHANGE, SettingRoute.AbnormalDoorSettingObjectKey + "*");
                //_Channel.QueueBind(_CommandQueue, Constdefine.MDVREXCHANGE, ReplyRoute.HandleAbnormalDoorReplyKey + "*");

                _Channel.QueueBind(_CommandQueue, Constdefine.APPEXCHANGE, SettingRoute.ElectricFenceObjectKey + "*");

                _Channel.QueueBind(_CommandQueue, Constdefine.APPEXCHANGE, SettingRoute.OverSpeedObjectKey + "*");

                //_Channel.QueueBind(_CommandQueue, Constdefine.APPEXCHANGE, SettingRoute.DeleteRuleObjectKey + "*");


                _Channel.QueueBind(_CommandQueue, Constdefine.APPEXCHANGE, SettingRoute.SendInfomationKey + "*");


                //Update Bug Send FenceCmd to  New Mdvr Error By xiay --2015-7-1 

                _Channel.QueueBind(_CommandQueue, Constdefine.APPEXCHANGE, UserMessageRoute.SuiteMaintainKey);
                ////devic install seccessful
                _Channel.QueueBind(_CommandQueue, Constdefine.APPEXCHANGE, UserMessageRoute.ComplateSuiteInstallKey);


                #endregion

                _Channel.BasicQos(0, 200, false);
                _MessageConsumer = new QueueingBasicConsumer(_Channel);
                _Channel.BasicConsume(_CommandQueue, false, _MessageConsumer);
                _ConnectedState = true;

            }
            catch (Exception ex)
            {
                _ConnectedState = false;
                ClearConn();
                LoggerManager.Logger.Error(ex);
            }
            return _ConnectedState;
        }

        /// <summary>
        /// Release link
        /// </summary>
        private static void ClearConn()
        {
            try
            {
                _ConnectedState = false;
                if (_Connection != null)
                {
                    if (_Channel != null)
                    {
                        _Channel.Close();
                    }

                    LoggerManager.Logger.Error("Channel Closed!");

                    if (_Connection.IsOpen)
                    {
                        _Connection.Close();
                    }
                    _Connection.Dispose();
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// Loop handle business news
        /// </summary>
        private static void DequeueMessage()
        {
            object sender;
            bool rec;
            while (true)
            {
                if (_StopState)
                    return;
                if (!InitBusinessQueue())
                    continue;

                rec = false;
                try
                {
                    rec = _MessageConsumer.Queue.Dequeue(10, out sender);
                    if (rec)
                    {
                        BasicDeliverEventArgs e = sender as BasicDeliverEventArgs;

                        IBasicProperties props = e.BasicProperties;

                        byte[] bytes = (byte[])(e.Body);

                        IBasicProperties bp = _Channel.CreateBasicProperties();

                        bp.Type = "1";
                        bp.Priority = 1;


                        Task.Factory.StartNew(() => { InvokeMethod(e.RoutingKey, bytes); });

                        _Channel.BasicAck(e.DeliveryTag, false);

                    }
                }
                catch (Exception ex)
                {
                    if (!_StopState)
                    {
                        ClearConn();
                        LoggerManager.Logger.Error(ex);
                    }
                }
            }
        }

        /// <summary>
        /// According to the routing Key，Way to find you need to perform and execute
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="msg"></param>
        private static void InvokeMethod(string key, object msg)
        {
            try
            {
                if (_methodInfo != null)
                {
                    var method = _methodInfo.Where(x => key.ToUpper().Contains(x.Key.ToUpper())).FirstOrDefault().Value;
                    if (method != null)
                    {
                        method.Invoke(_messageProcess, new object[] { msg, key });
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
        /// Write messages
        /// </summary>
        /// <param name="exchange">exchange</param>
        public static void PublishMessage(string exchange, string route, byte[] msg)
        {
            try
            {
                IBasicProperties bp = _Channel.CreateBasicProperties();
                bp.Type = "1";
                bp.Priority = 1;
                _Channel.BasicPublish(exchange, route, bp, msg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                LoggerManager.Logger.Error(ex.StackTrace);
            }
        }
    }
}
