using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Gsafety.MQ;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.Common.Logging;
using RabbitMQ.Client.Events;
using System.Reflection;
using ServiceStack.Redis;
using System.Configuration;

namespace OnOffLineManagement
{
    public static class DealWithMessage
    {
        private static IModel _businessCh;

        private static IConnection _businessConn;

        private static QueueingBasicConsumer _businessConsumer;

        private static bool _businessStop = false;

        private static bool _BusinessConnected = false;

        private static string _businessQueue = "Task.PTMS.OnOffLineMessage";

        private static Dictionary<string, MethodInfo> _methodInfo;
        private static DealWithProcess dealWithProcess;

        public static void Start()
        {
            try
            {
                _methodInfo = MethodHelper.GetMethodInfo();
                dealWithProcess = new DealWithProcess();
                Task.Factory.StartNew(() => { DequeueBusinessMessage(); });
                LoggerManager.Logger.Info("DealWithMessage service starts successfully!");
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("An exception occurred when the DealWithMessage service starts!" + ex);
            }
        }

        public static void Stop()
        {
            try
            {
                _businessStop = true;
                ClearBusinessConn();
                LoggerManager.Logger.Info("DealWithMessage service Stop successfully!");
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("An exception occurred when the stopping DealWithMessage service!" + ex);
            }
        }

        private static void DequeueBusinessMessage()
        {
            object sender;
            bool rec;
            while (true)
            {
                if (_businessStop)
                    return;
                if (!InitBusinessQueue())
                    continue;

                rec = false;
                try
                {
                    rec = _businessConsumer.Queue.Dequeue(10, out sender);
                    if (rec)
                    {
                        BasicDeliverEventArgs e = sender as BasicDeliverEventArgs;

                        IBasicProperties props = e.BasicProperties;

                        byte[] bytes = (byte[])(e.Body);

                        IBasicProperties bp = _businessCh.CreateBasicProperties();

                        bp.Type = "1";
                        bp.Priority = 1;

                        Task.Factory.StartNew(() => { InvokeMethod(e.RoutingKey, bytes); });

                        _businessCh.BasicAck(e.DeliveryTag, false);

                    }
                }
                catch (Exception ex)
                {
                    if (!_businessStop)
                    {
                        ClearBusinessConn();
                        LoggerManager.Logger.Error(ex);
                    }
                }
            }
        }

        private static bool InitBusinessQueue()
        {
            try
            {
                if (_BusinessConnected)
                    return _BusinessConnected;

                _businessConn = new MQFactory().CreateConnection();

                _businessCh = _businessConn.CreateModel();

                _businessCh.ExchangeDeclare(Constdefine.MDVREXCHANGE, "topic", true);
                _businessCh.ExchangeDeclare(Constdefine.GPSEXCHANGE, "topic", true);

                _businessCh.QueueDeclare(_businessQueue, true, false, false, null);

                _businessCh.QueueBind(_businessQueue, Constdefine.MDVREXCHANGE, MonitorRoute.OriginalOnlineKey + "*");

                _businessCh.QueueBind(_businessQueue, Constdefine.MDVREXCHANGE, MonitorRoute.OriginalOfflineA1Key + "*");

                _businessCh.QueueBind(_businessQueue, Constdefine.MDVREXCHANGE, MonitorRoute.OriginalOfflineA2Key + "*");

                _businessCh.QueueBind(_businessQueue, Constdefine.MDVREXCHANGE, MonitorRoute.OriginalShutDowmKey + "*");

                _businessCh.QueueBind(_businessQueue, Constdefine.GPSEXCHANGE, GPSRoute.OriginalGPSKey + "*");

                _businessCh.BasicQos(0, 200, false);
                _businessConsumer = new QueueingBasicConsumer(_businessCh);
                _businessCh.BasicConsume(_businessQueue, false, _businessConsumer);
                _BusinessConnected = true;

            }
            catch (Exception ex)
            {
                _BusinessConnected = false;
                ClearBusinessConn();
                LoggerManager.Logger.Error(ex);
            }
            return _BusinessConnected;
        }

        public static void ClearBusinessConn()
        {
            try
            {
                _BusinessConnected = false;
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

        private static void InvokeMethod(string key, object msg)
        {
            try
            {
                if (_methodInfo != null)
                {
                    var method = _methodInfo.Where(x => key.ToUpper().Contains(x.Key.ToUpper())).FirstOrDefault().Value;
                    if (method != null)
                    {
                        method.Invoke(dealWithProcess, new object[] { msg });
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
    }
}
