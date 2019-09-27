/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: fd9544ec-f82e-4330-afe6-ee48e989b85b      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Video
/////    Project Description:    
/////             Class Name: VideoMessage
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/15 10:28:20
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/15 10:28:20
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.Common.Logging;
using RabbitMQ.Client;
using System.Reflection;
using RabbitMQ.Client.Events;
using Gsafety.MQ;
using Gsafety.PTMS.Analysis.Helper;

namespace Gsafety.PTMS.Video
{
    public class VideoMessage
    {
        private static IModel _Channel;
        private static IConnection _Connection;
        private static QueueingBasicConsumer _MessageConsumer;
        private static string _CommandQueue = "Task.PTMS.VideoMessage";
        private static Dictionary<string, MethodInfo> _methodInfo;
        private static MessageProcessing _messageProcess;

        private static bool _StopState = false;
        private static bool _ConnectedState = false;

        public static void Start()
        {
            try
            {
                ////Get all the business methods to be executed
                _methodInfo = MethodHelper.GetMethodInfo();

                _messageProcess = new MessageProcessing();
                Task.Factory.StartNew(() => { DequeueMessage(); });

                LoggerManager.Logger.Info("Video service starts successfully!");
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("Video service failed to start!" + ex);
            }
        }

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
                    ClearConn();
                    LoggerManager.Logger.Error(ex);
                }
            }
        }

        private static bool InitBusinessQueue()
        {
            try
            {
                if (_ConnectedState)
                    return _ConnectedState;
                ////create link
                _Connection = new MQFactory().CreateConnection();

                _Channel = _Connection.CreateModel();

                _Channel.ExchangeDeclare(Constdefine.MDVREXCHANGE, "topic", true);
                _Channel.ExchangeDeclare(Constdefine.APPEXCHANGE, "topic", true);

                //Disclaimer business message queue
                _Channel.QueueDeclare(_CommandQueue, true, false, false, null);

                #region routing bind

                ////////////online information
                _Channel.QueueBind(_CommandQueue, Constdefine.APPEXCHANGE, SettingRoute.VideoListCMDKey + "*");
                _Channel.QueueBind(_CommandQueue, Constdefine.APPEXCHANGE, SettingRoute.DownloadMdvrFile + "*");

                _Channel.QueueBind(_CommandQueue, Constdefine.APPEXCHANGE, ReplyRoute.HandleDownloadFileV23 + "*");
                _Channel.QueueBind(_CommandQueue, Constdefine.MDVREXCHANGE, ReplyRoute.HandleVideoListReplyKey + "*");
                _Channel.QueueBind(_CommandQueue, Constdefine.MDVREXCHANGE, ReplyRoute.HandleDownloadFileReplyKey + "*");


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

        private static void ClearConn()
        {
            try
            {
                _ConnectedState = false;
                if (_Connection != null)
                {
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
            }
        }

        public static void End()
        {
            try
            {
                _StopState = true;
                ClearConn();
                LoggerManager.Logger.Info("Video Manager service stop successfully!");
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("Video Manager service stop failure!" + ex);
            }
        }
    }
}
