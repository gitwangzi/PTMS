using Gsafety.MQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gs.PTMS.MessageCenterService
{
    public class RabbitMqManager
    {
        private static IModel _businessCh = null;
        private static IConnection _businessConn;
        private static QueueingBasicConsumer _businessConsumer;
        private static bool _businessStop = false;
        private static bool _BusinessConnected = false;
        private static string _businessQueue = "Task.PTMS.TransforMessage";
        private static string _APPEXCHANGE = "APP_EXCHANGE";
        /// <summary>
        /// Start
        /// </summary>
        public static void Start()
        {
            try
            {
                Task.Factory.StartNew(() => { DequeueBusinessMessage(); });
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Init Business Queue
        /// </summary>
        private static bool InitBusinessQueue()
        {
            try
            {
                if (_BusinessConnected)
                    return _BusinessConnected;
                _businessConn = new MQFactory().CreateConnection();

                _businessCh = _businessConn.CreateModel();

                _businessCh.ExchangeDeclare(_APPEXCHANGE, "topic", true);

                ////business queue
                _businessCh.QueueDeclare(_businessQueue, true, false, false, null);

                ////binding route
                //// process alarm
                //_businessCh.QueueBind(_businessQueue, _APPEXCHANGE, AlarmRoute.OriginalAlarmKey + "*");
                ////Original Transparent CMD Key
                //_businessCh.QueueBind(_businessQueue, _APPEXCHANGE, MonitorRoute.OriginalTransparentCMDKey + "*");
                ////online 
                //_businessCh.QueueBind(_businessQueue, _APPEXCHANGE, MonitorRoute.OriginalOnlineKey + "*");
                ////offline A1
                //_businessCh.QueueBind(_businessQueue, _APPEXCHANGE, MonitorRoute.OriginalOfflineA1Key + "*");
                ////offline A2
                //_businessCh.QueueBind(_businessQueue, _APPEXCHANGE, MonitorRoute.OriginalOfflineA2Key + "*");
                ////shutdown send up V20

                //// Camera No Signal Alert Key
                //_businessCh.QueueBind(_businessQueue, _APPEXCHANGE, AlertRoute.OriginalCameraNoSignalAlertKey + "*");
                ////Original Camera Occlusion Alert Key
                // Protocol is not supported

                ////Original Temperature Alert Key
                //_businessCh.QueueBind(_businessQueue, _APPEXCHANGE, AlertRoute.OriginalTemperatureAlertKey + "*");
                ////Original Mdvr MemoryCard Error Alert Key
                //_businessCh.QueueBind(_businessQueue, _APPEXCHANGE, AlertRoute.OriginalMdvrMemoryCardErrorAlertKey + "*");
                ////Original Gps Receiver Fault Alert Key
                //_businessCh.QueueBind(_businessQueue, _APPEXCHANGE, AlertRoute.OriginalGpsReceiverFaultAlertKey + "*");
                // Protocol is not supported
                ////Original Voltage Abnormal AlertKey
                //_businessCh.QueueBind(_businessQueue, _APPEXCHANGE, AlertRoute.OriginalVoltageAbnormalAlertKey + "*");



                ////Original Over Speed AlerKey
                //_businessCh.QueueBind(_businessQueue, _APPEXCHANGE, AlertRoute.OriginalOverSpeedAlerKey + "*");
                ////Original Region AlertKey
                //_businessCh.QueueBind(_businessQueue, _APPEXCHANGE, AlertRoute.OriginalRegionAlertKey + "*");

                ////Original Remove Device Suite AlertKey
                //_businessCh.QueueBind(_businessQueue, _APPEXCHANGE, AlertRoute.OriginalRemoveDeviceSuiteAlertKey + "*");

                ////miles
                //_businessCh.QueueBind(_businessQueue, _APPEXCHANGE, MonitorRoute.OriginalRapterMileageKey + "*");

                _businessCh.BasicQos(0, 200, false);
                _businessConsumer = new QueueingBasicConsumer(_businessCh);
                _businessCh.BasicConsume(_businessQueue, false, _businessConsumer);
                _BusinessConnected = true;

            }
            catch (Exception ex)
            {
                _BusinessConnected = false;
                ClearBusinessConn();
            }
            return _BusinessConnected;
        }

        /// <summary>
        /// Dequeue Business Message
        /// </summary>
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

                        Task.Factory.StartNew(() => { SocketManager.OnRabbitMqMessage(e.RoutingKey, bytes); });

                        ////reply
                        _businessCh.BasicAck(e.DeliveryTag, false);
                    }
                }
                catch (Exception ex)
                {
                    if (!_businessStop)
                    {

                        ClearBusinessConn();
                    }
                }
            }
        }

        /// <summary>
        /// dispose connetiong
        /// </summary>
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
                    _businessConn = null;
                }
            }
            catch (Exception ex)
            {

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
                IBasicProperties bp = _businessCh.CreateBasicProperties();
                bp.Type = "1";
                bp.Priority = 1;
                _businessCh.BasicPublish(exchange, route, bp, msg);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
