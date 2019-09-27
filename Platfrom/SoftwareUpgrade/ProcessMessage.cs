/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 067018b8-4724-4622-9f25-fab882e7536d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis
/////    Project Description:    
/////             Class Name: SoftwareUpgrade
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/6 12:08:49
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/6 12:08:49
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.Common.Logging;
using Gsafety.MQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Gsafety.PTMS.SoftwareUpgrade
{
    public class ProcessMessage
    {
        private static string _queue = "TASK.UserMessageForUpgrade" + DateTime.Now.ToString("HHmmssfff");

        private static IModel _ch;
        private static IConnection _conn;
        private static QueueingBasicConsumer _consumer;

        private static bool _stop = false;

        public static void Start()
        {
            try
            {
                UpgradeManage.BatchSendUpgradeCMD();
                Task.Factory.StartNew(() => { DequeueMessage(); });
                LoggerManager.Logger.Info("SoftwareUpgrade service starts successfully!");
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("An exception occurred when the SoftwareUpgrade service starts!" + ex);
            }
        }

        public static void Stop()
        {
            try
            {
                _stop = true;
                LoggerManager.Logger.Info("SoftwareUpgrade service Stop successfully!");
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("An exception occurred when the stopping SoftwareUpgrade service!" + ex);
            }
        }

        public static void ClearConn()
        {
            try
            {
                if (_conn != null)
                {
                    if (_conn.IsOpen)
                        _conn.Close();
                    _conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        private static void Init()
        {
            _conn = new MQFactory().CreateConnection();
            _ch = _conn.CreateModel();

            _ch.ExchangeDeclare(Constdefine.MDVREXCHANGE, "topic", true);
            _ch.ExchangeDeclare(Constdefine.APPEXCHANGE, "topic", true);

            _ch.QueueDeclare(_queue, false, false, true, null);

            _ch.QueueBind(_queue, Constdefine.APPEXCHANGE, MonitorRoute.OriginalUpgradeCMDKey + "*");

            _ch.QueueBind(_queue, Constdefine.MDVREXCHANGE, ReplyRoute.OriginalUpgradeReplyKey + "*");

            _ch.QueueBind(_queue, Constdefine.APPEXCHANGE, MonitorRoute.OriginalUpgradeStatusKey + "*");

            _ch.QueueBind(_queue, Constdefine.MDVREXCHANGE, ReplyRoute.OriginalUpgradeStatusReplyKey + "*");

            _ch.QueueBind(_queue, Constdefine.APPEXCHANGE, UserMessageRoute.OriginalUpgradeNotifyKey);

            _ch.BasicQos(0, 200, false);
            _consumer = new QueueingBasicConsumer(_ch);
            _ch.BasicConsume(_queue, false, _consumer);
        }

        private static void DequeueMessage()
        {
            Init();
            while (true)
            {
                if (_stop) break;
                object sender = null;
                bool rec = false;
                try
                {
                    rec = (_consumer.Queue.Dequeue(10, out sender));
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                    LoggerManager.Logger.Error("An error occurs,recconnect in mq.......");

                    if (_stop) break;
                    ClearConn();
                    Init();
                }

                if (rec)
                {
                    BasicDeliverEventArgs e = sender as BasicDeliverEventArgs;
                    IBasicProperties props = e.BasicProperties;
                    byte[] bytes = (byte[])(e.Body);
                    IBasicProperties bp = _ch.CreateBasicProperties();

                    bp.Type = "1";
                    bp.Priority = 1;

                    if (e.RoutingKey.Contains(MonitorRoute.OriginalUpgradeCMDKey))
                    {
                        UpgradeManage.ProcessUpgrade(bytes, e.RoutingKey);
                    }

                    else if (e.RoutingKey.Contains(ReplyRoute.OriginalUpgradeReplyKey))
                    {
                        UpgradeManage.ProcessUpgradeReply(bytes);
                    }

                    else if (e.RoutingKey.Contains(MonitorRoute.OriginalUpgradeStatusKey))
                    {
                        UpgradeManage.UpgradeStatusCMD(bytes, e.RoutingKey);
                    }

                    else if (e.RoutingKey.Contains(ReplyRoute.OriginalUpgradeStatusReplyKey))
                    {
                        UpgradeManage.UpgradeStatusReply(bytes);
                    }

                    else if (e.RoutingKey.Contains(UserMessageRoute.OriginalUpgradeNotifyKey))
                    {
                        UpgradeManage.StartBatchSoftwareUpgrade(bytes);
                    }

                    _ch.BasicAck(e.DeliveryTag, false);
                }
            }
        }

        public static void PublishMessage(string exchange, string route, byte[] msg)
        {
            try
            {
                IBasicProperties bp = _ch.CreateBasicProperties();

                bp.Type = "1";
                bp.Priority = 1;
                _ch.BasicPublish(exchange, route, bp, msg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }
    }
}
