using Gsafety.Common.Logging;
using Gsafety.MQ;
using Gsafety.PTMS.Analysis.Helper;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.BaseInfo;


namespace Gsafety.PTMS.MessageLib
{
    public partial class MessageService
    {
        private string _queue;
        private IModel _ch;
        private IConnection _conn;
        private QueueingBasicConsumer _consumer = null;
        private bool _connected = false;
        bool _stop;

        public static BaseService baseservice = new BaseService();
        private object _lockObj = new object();

        public void ClearConn()
        {
            try
            {
                lock (_lockObj)
                {
                   // _stop = true;
                    if (_ch != null)
                    {
                        _ch.Dispose();
                        _ch = null;
                    }

                    try
                    {
                        if (_conn != null)
                        {
                            if (_conn.IsOpen)
                                _conn.Close();
                            _conn.Dispose();
                        }
                    }
                    catch (EndOfStreamException ex)
                    {
                        baseservice.Error(ex, "MessageService");
                        LoggerManager.Logger.Error(ex);
                    }
                    finally
                    {
                        _conn = null;
                    }
                }
            }
            catch (Exception ex)
            {
                baseservice.Error(ex, "MessageService");
                LoggerManager.Logger.Error(ex);
            }
        }

        public void Init()
        {
            try
            {
                lock (_lockObj)
                {
                    _connected = false;
                    _stop = false;
                    Task.Run(() =>
                    {
                        RecMessage();
                    });

                    Task.Run(() =>
                    {
                        HeartbeatSending(_queue);
                    });

                    Task.Run(() =>
                    {
                        UserOnlineHeartBeatSending();
                    });
                }
            }
            catch (Exception ex)
            {
                baseservice.Error(ex, "MessageService");
                LoggerManager.Logger.Error(ex);
                ClearConn();
            }
        }

        private void InitialMQ()
        {
            try
            {
                _conn = null;

                while (_conn == null)
                {
                    if (_stop)
                        return;
                    _conn = new MQFactory().CreateConnection();
                }
                _ch = _conn.CreateModel();

                _ch.ExchangeDeclare(Constdefine.MDVREXCHANGE, "topic", true);
                _ch.ExchangeDeclare(Constdefine.GPSEXCHANGE, "topic", true);
                _ch.ExchangeDeclare(Constdefine.APPEXCHANGE, "topic", true);

                Hashtable args = new Hashtable();
                args.Add("x-expires", 600000);

                //jjAdd
                //1.QUEUE NAME 
                //2 QUEUE WHETHER PERSISTENCE
                //3 THE CONNECTION IS NOT ALIVE,QUEUE WHETHER AUTO DEL 
                //4 THE CONSUMER IF NOT ALIVE ,QUEUE WHETHER AUTO DEL 
                //5 EXTENDED PARAMETERS x-ha-policy:be used on mirrored queue ;x-expires:Auto_del_time (ms)
                _ch.QueueDeclare(_queue, true, false, true, args);
                _ch.QueueBind(_queue, Constdefine.APPEXCHANGE, UserRoute.ForceLogout);
                _ch.QueueBind(_queue, Constdefine.APPEXCHANGE, UserRoute.ForceMultiplyLogout);
                _ch.QueueBind(_queue, Constdefine.APPEXCHANGE, MonitorRoute.HandleOnOfflineKey + "*");
                _ch.QueueBind(_queue, Constdefine.APPEXCHANGE, AlarmRoute.AlarmInfoKey + "*");
                _ch.QueueBind(_queue, Constdefine.APPEXCHANGE, AlertRoute.HandleBusinessAlertKey + "*");
                _ch.QueueBind(_queue, Constdefine.APPEXCHANGE, AlertRoute.HandleDeviceAlertKey + "*");
                _ch.QueueBind(_queue, Constdefine.APPEXCHANGE, VideoRoute.QueryMdvrFileListAppResponseKey + "*");
                _ch.QueueBind(_queue, Constdefine.APPEXCHANGE, VideoRoute.TakePictureAppResponseKey + "*");

                _ch.QueueBind(_queue, Constdefine.GPSEXCHANGE, GPSRoute.SuiteKey + "*");
                _ch.QueueBind(_queue, Constdefine.GPSEXCHANGE, GPSRoute.MobileKey + "*");
                _ch.QueueBind(_queue, Constdefine.GPSEXCHANGE, GPSRoute.GPSKey + "*");
                _ch.QueueBind(_queue, Constdefine.APPEXCHANGE, AlarmRoute.CompleteAlarmNotice + "*");
                _ch.QueueBind(_queue, Constdefine.APPEXCHANGE, AlertRoute.CompleteAlertNotice + "*");
                _ch.QueueBind(_queue, Constdefine.APPEXCHANGE, UserMessageRoute.InstallCompleteNotification);
                _ch.QueueBind(_queue, Constdefine.APPEXCHANGE, UserMessageRoute.AuthenticationReponse);
                _ch.BasicQos(0, 200, false);
                _consumer = new QueueingBasicConsumer(_ch);
                _ch.BasicConsume(_queue, false, _consumer);

                _connected = true;
            }
            catch (Exception ex)
            {
                baseservice.Error(ex, "MessageService");
                _connected = false;
                ClearConn();
                LoggerManager.Logger.Error("InitialMQ", ex);
            }
        }

        /// <summary>
        /// RecMessage
        /// </summary>
        /// <param name="queue"></param>
        public void RecMessage()
        {
            while (true)
            {
                lock (_lockObj)
                {
                    if (_stop) return;

                    try
                    {
                        if (!_connected)
                        {
                            InitialMQ();
                        }

                        try
                        {
                            object sender;
                            bool rec = false;
                            rec = (_consumer.Queue.Dequeue(10, out sender));

                            if (rec)
                            {
                                BasicDeliverEventArgs e = sender as BasicDeliverEventArgs;
                                IBasicProperties props = e.BasicProperties;

                                byte[] bytes = (byte[])(e.Body);
                                LoggerManager.Logger.Info(string.Format("RecMessage:{0}", e.RoutingKey));

                                Task task = Task.Factory.StartNew(() =>
                                {
                                    OnMessage(e.RoutingKey, bytes);
                                });


                                _ch.BasicAck(e.DeliveryTag, false);
                            }
                        }
                        catch (Exception ex)
                        {
                            baseservice.Error(ex, "MessageService");
                            _connected = false;
                            ClearConn();
                            LoggerManager.Logger.Error("RecMessage", ex);
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        baseservice.Error(ex, "MessageService");
                        _connected = false;
                        ClearConn();
                        LoggerManager.Logger.Error("RecMessage", ex);
                    }
                }
            }
        }

        public void SendMessage(string exchange, string route, byte[] msg)
        {
            lock (_lockObj)
            {
                if (_ch == null)
                {
                    LoggerManager.Logger.Error("_Message channel is null");
                    return;
                }
                try
                {
                    IBasicProperties bp = _ch.CreateBasicProperties();

                    bp.Type = "1";
                    bp.Priority = 1;
                    _ch.BasicPublish(exchange, route, bp, msg);
                    LoggerManager.Logger.Info("send message:" + System.Text.ASCIIEncoding.ASCII.GetString(msg));
                }
                catch (Exception ex)
                {
                    baseservice.Error(ex, "MessageService");
                    LoggerManager.Logger.Error("SendMessage", ex);
                }
            }
        }

        public void OnMessage(string key, byte[] bytes)
        {
            if (key == UserRoute.ForceLogout)
            {
                OnForceLogout(bytes);
            }
            else if (key == UserRoute.ForceMultiplyLogout)
            {
                OnForceMultiplyLogout(bytes);
            }
            else if (key.Contains(GPSRoute.SuiteKey))
            {
                OnSuiteGPS(bytes, key);
            }
            else if (key.Contains(GPSRoute.GPSKey))
            {
                OnGPSGPS(bytes, key);
            }
            else if (key.Contains(GPSRoute.MobileKey))
            {
                OnMobileGPS(bytes, key);
            }
            else if (key.Contains(MonitorRoute.HandleOnOfflineKey))
            {
                OnOffLine(bytes, key);
            }
            else if (key.Contains(AlarmRoute.AlarmInfoKey))
            {
                OnAlarm(key, bytes);
            }
            else if (key.Contains(AlertRoute.HandleBusinessAlertKey))
            {
                OnBusinessAlert(key, bytes);
            }
            else if (key.Contains(AlertRoute.HandleDeviceAlertKey))
            {
                OnDeviceAlert(key, bytes);
            }
            else if (key.Contains(AlarmRoute.CompleteAlarmNotice))
            {
                OnCompleteAlarmNotice(key, bytes);
            }
            else if (key.Contains(AlertRoute.CompleteAlertNotice))
            {
                OnCompleteAlertNotice(key, bytes);
            }
            else if (key.Contains(VideoRoute.QueryMdvrFileListAppResponseKey))
            {
                OnVideoList(key, bytes);
            }
            else if (key.Contains(VideoRoute.TakePictureAppResponseKey))
            {
                OnTakePicture(key, bytes);
            }
            else if (key.Contains(UserMessageRoute.InstallCompleteNotification))
            {
                OnInstallCompleteNotification(key, bytes);
            }
            else if (key.Contains(UserMessageRoute.AuthenticationReponse))
            {
                OnAuthenticationReponse(bytes);
            }
        }
    }
}
