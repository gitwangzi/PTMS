using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Message.Contract;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.MQ;
using Gsafety.PTMS.Message.Contract.Data.Video;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class MessageService : IMessageService
    {
        private RuleInfo _ruleInfo;
        private List<string> _ruleListKey = new List<string>();

        private string _queue;
        private IModel _ch;
        private IConnection _conn;
        private QueueingBasicConsumer _consumer = null;


        private bool _stop = false;

        private object _lockObj = new object();

        private readonly int HeartbeatSpan = 30000;


        private IMessageCallBackContract messageCallBack = OperationContext.Current.GetCallbackChannel<IMessageCallBackContract>();

        public MessageService()
        {
            try
            {
                OperationContext.Current.Channel.Closed += Channel_Closed;
                OperationContext.Current.Channel.Faulted += Channel_Faulted;
                HeartbeatSpan = ConfigHelper.HeartbeatTimeSpan;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        void Channel_Faulted(object sender, EventArgs e)
        {
            try
            {
                _stop = true;
                ClearConn();
                LoggerManager.Logger.Info("Client:" + _queue + "  Faulted");
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }


        void Channel_Closed(object sender, EventArgs e)
        {
            try
            {
                _stop = true;
                ClearConn();
                LoggerManager.Logger.Info("Client:" + _queue + "  Closed");
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        //public void SendCompleteAlarmMessage(CompleteAlarm model)
        //{
        //    try
        //    {
        //        byte[] msg = ConvertHelper.ObjectToBytes(model);
        //        SendMessage(Constdefine.APPEXCHANGE, UserMessageRoute.CompleteAlarmKey, msg);
        //        if (!model.IsRealAlarm)
        //        {
        //            SendMessage(Constdefine.APPEXCHANGE, UserMessageRoute.DisarmAlarmKey, msg);
        //        }
        //        LoggerManager.Logger.Info("Alarm processing is complete,Send Complete Alarm Message!");
        //    }
        //    catch (Exception e)
        //    {
        //        LoggerManager.Logger.Error(e);
        //    }
        //}

        public void SendDeviceInstallMessage(DeviceInstall model)
        {
            try
            {
                byte[] msg = ConvertHelper.ObjectToBytes(model);
               // SendMessage(Constdefine.APPEXCHANGE, UserMessageRoute.DeviceInstallKey, msg);
            }
            catch (Exception e)
            {
                LoggerManager.Logger.Error(e);
            }
        }

        public void SendDeviceMaintainMessage(DeviceMaintain model)
        {
            try
            {
                byte[] msg = ConvertHelper.ObjectToBytes(model);
                //SendMessage(Constdefine.APPEXCHANGE, UserMessageRoute.DeviceMaintainKey, msg);
            }
            catch (Exception e)
            {
                LoggerManager.Logger.Error(e);
            }
        }

        public void SendHandingAlertMessage(HandingAlert model)
        {
            try
            {
                byte[] msg = ConvertHelper.ObjectToBytes(model);
                SendMessage(Constdefine.APPEXCHANGE, UserMessageRoute.HandingAlertKey, msg);
            }
            catch (Exception e)
            {
                LoggerManager.Logger.Error(e);
            }
        }

        public void SendCompleteAlertMessage(Gsafety.PTMS.Message.Contract.Data.CompleteAlert model)
        {
            try
            {
                byte[] msg = ConvertHelper.ObjectToBytes(model);
                SendMessage(Constdefine.APPEXCHANGE, UserMessageRoute.CompleteAlertKey, msg);
            }
            catch (Exception e)
            {
                LoggerManager.Logger.Error(e);
            }
        }


        public void SendStartInstallMessage(StartInstall model)
        {
            try
            {
                byte[] msg = ConvertHelper.ObjectToBytes(model);
                //SendMessage(Constdefine.APPEXCHANGE, UserMessageRoute.StartInstallKey, msg);
            }
            catch (Exception e)
            {
                LoggerManager.Logger.Error(e);
            }
        }

        public void SendDeleteInstallMessage(DeleteInstall model)
        {
            try
            {
                byte[] msg = ConvertHelper.ObjectToBytes(model);
                //SendMessage(Constdefine.APPEXCHANGE, UserMessageRoute.DeleteInstallKey, msg);
            }
            catch (Exception e)
            {
                LoggerManager.Logger.Error(e);
            }
        }

        public void SendDeleteUserMessage(DeleteUser model)
        {
            try
            {
                byte[] msg = ConvertHelper.ObjectToBytes(model);
                SendMessage(Constdefine.APPEXCHANGE, UserMessageRoute.DeleteUserKey, msg);
            }
            catch (Exception e)
            {
                LoggerManager.Logger.Error(e);
            }
        }


        public void SendChangeUserMessage(ChangeUser model)
        {
            try
            {
                byte[] msg = ConvertHelper.ObjectToBytes(model);
                SendMessage(Constdefine.APPEXCHANGE, UserMessageRoute.ChangeUserKey, msg);
            }
            catch (Exception e)
            {
                LoggerManager.Logger.Error(e);
            }
        }


 
        /// <summary>
        /// send gps 
        /// </summary>
        /// <param name="model">gps model</param>
        public void SendSettingGpsUploadCMDSet(GpsSendUpModel model)
        {
            try
            {
                byte[] msg = ConvertHelper.ObjectToBytes(model);
                string routeKey = SettingRoute.GpsSettingObjectKey + "*";
                LoggerManager.Logger.Info("Client:" + _queue + "  GpsSettingObjectKey ,route:" + routeKey + ",data:" + ConvertHelper.ConvertModelToJson(model));
                SendMessage(Constdefine.APPEXCHANGE, routeKey, msg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }
    

        public void SendSettingOneKeyAlarmUploadCMDSet(OneKeyAlarmSendUpModel model)
        {
            try
            {
                byte[] msg = ConvertHelper.ObjectToBytes(model);
                string routeKey = SettingRoute.OneKeyAlarmSettingObjectKey + "*";
                LoggerManager.Logger.Info("Client:" + _queue + "  OneKeyAlarmSettingObjectKey ,route:" + routeKey + ",data:" + ConvertHelper.ConvertModelToJson(model));
                SendMessage(Constdefine.APPEXCHANGE, routeKey, msg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }
        public void SendInfomationCommand(SendInfomationModel model)
        {
            try
            {
                byte[] msg = ConvertHelper.ObjectToBytes(model);
                string routeKey = SettingRoute.SendInfomationKey + "*";
                LoggerManager.Logger.Info("Client:" + _queue + "  SendInfomationKey ,route:" + routeKey + ",data:" + ConvertHelper.ConvertModelToJson(model));
                SendMessage(Constdefine.APPEXCHANGE, routeKey, msg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }

        }
        //Video
        public void SendGetVideoListCMD(QueryMdvrFileList model)
        {
            try
            {
                byte[] msg = ConvertHelper.ObjectToBytes(model);
                string routeKey = SettingRoute.VideoListCMDKey + "*";
                LoggerManager.Logger.Info("Client:" + _queue + "  VideoListCMDKey ,route:" + routeKey + ",data:" + ConvertHelper.ConvertModelToJson(model));
                SendMessage(Constdefine.APPEXCHANGE, routeKey, msg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        //DownMdvrFile
        public void SendDownloadMdvrFile(DownloadFile model)
        {
            try
            {
                byte[] msg = ConvertHelper.ObjectToBytes(model);
                string routeKey = SettingRoute.DownloadMdvrFile + "*";
                LoggerManager.Logger.Info("Client:" + _queue + "  DownloadMdvrFile ,route:" + routeKey + ",data:" + ConvertHelper.ConvertModelToJson(model));
                SendMessage(Constdefine.APPEXCHANGE, routeKey, msg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        public void SendDeleteRuleCMD(RuleDeleteModel model)
        {
            try
            {
                byte[] msg = ConvertHelper.ObjectToBytes(model);
                string routeKey = SettingRoute.DeleteRuleObjectKey + "*";
                LoggerManager.Logger.Info("Client:" + _queue + "  DeleteRuleObjectKey ,route:" + routeKey + ",data:" + ConvertHelper.ConvertModelToJson(model));
                SendMessage(Constdefine.APPEXCHANGE, routeKey, msg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }


        /// <summary>
        /// Init
        /// </summary>
        public void Init(string QUEUE, RuleInfo ruleInfo)
        {
            try
            {
                lock (_lockObj)
                {

                    QUEUE = QUEUE.Contains("TASK.PTMS.Client_") ? QUEUE : "TASK.PTMS.Client_" + QUEUE;
                    LoggerManager.Logger.Info("Client:" + QUEUE + "  Init");
                    this._queue = QUEUE;
                    this._ruleInfo = ruleInfo;
                    AnalysisRule();

                    _conn = new MQFactory().CreateConnection();
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
                    _ch.QueueDeclare(QUEUE, false, false, true, args);


                    _ch.BasicQos(0, 200, false);
                    _consumer = new QueueingBasicConsumer(_ch);
                    _ch.BasicConsume(_queue, false, _consumer);
                    _stop = false;
                    Task.Run(() =>
                    {
                        RecMessage();
                    });

                    Task.Run(() =>
                    {
                        HeartbeatSending(_queue);
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                ClearConn();
            }
        }


        private void AnalysisRule()
        {
            if (_ruleInfo != null && _ruleInfo.ListProvince.Count() > 0)
            {
                foreach (var provinceItem in _ruleInfo.ListProvince)
                {

                    if (provinceItem.ListCity.Count > 0)
                    {
                        foreach (var cityItem in provinceItem.ListCity)
                        {
                            _ruleListKey.Add(string.Format(".{0}.{1}", provinceItem.ProvinceId, cityItem.CityId));
                        }
                    }
                    else
                    {
                        _ruleListKey.Add(string.Format(".{0}.{1}", provinceItem.ProvinceId, "*"));
                    }
                }

                foreach (var ruleItem in _ruleListKey)
                {
                    LoggerManager.Logger.Info(string.Format("ruleKey:{0}", ruleItem));
                }
            }
            else
            {
                LoggerManager.Logger.Warn(string.Format("client:{0},ruleinfo is null!", _queue));
            }
        }

        /// <summary>
        /// RegisterMessage
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="route"></param>
        public void RegisterMessage(string exchange, string route)
        {
            lock (_lockObj)
            {
                try
                {

                    if (exchange.Equals(Constdefine.GPSEXCHANGE) || exchange.Equals(Constdefine.APPEXCHANGE))
                    {

                        _ch.QueueBind(_queue, exchange, route);
                        LoggerManager.Logger.Info(string.Format("client:{0} register message : {1}!", _queue, route));
                    }
                    else
                    {

                        if (_ruleListKey.Count > 0)
                        {
                            foreach (var ruleItem in _ruleListKey)
                            {

                                _ch.QueueBind(_queue, exchange, route + ruleItem);
                                LoggerManager.Logger.Info(string.Format("client:{0} register message : {1}!", _queue, route + ruleItem));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                }
            }
        }

        /// <summary>
        /// RelieveMessage
        /// </summary>
        /// <param name="exchangeEnum"></param>
        /// <param name="route"></param>
        public void RelieveMessage(string exchange, string route)
        {
            lock (_lockObj)
            {
                if (exchange.Equals(Constdefine.GPSEXCHANGE) || exchange.Equals(Constdefine.APPEXCHANGE))
                {
                    _ch.QueueUnbind(_queue, exchange, route, null);
                    LoggerManager.Logger.Info(string.Format("client:{0} relieve message : {1}!", _queue, route));
                }
                else
                {
                    if (_ruleListKey.Count > 0)
                    {
                        foreach (var ruleItem in _ruleListKey)
                        {
                            _ch.QueueUnbind(_queue, exchange, route + ruleItem, null);
                            LoggerManager.Logger.Info(string.Format("client:{0} relieve message : {1}!", _queue, route + ruleItem));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// SendMessage
        /// </summary>
        /// <param name="exchangeEnum"></param>
        /// <param name="route"></param>
        /// <param name="msg"></param>
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
                    LoggerManager.Logger.Error("SendMessage", ex);
                }

            }
        }

        public void ClearConn()
        {
            try
            {
                lock (_lockObj)
                {
                    _stop = true;
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
                        LoggerManager.Logger.Error(ex);
                    }
                    finally
                    {
                        _conn = null;
                    }

                    _ruleListKey.Clear();
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }



        /// <summary>
        /// RecMessage
        /// </summary>
        /// <param name="queue"></param>
        public void RecMessage()
        {

            lock (_lockObj)
            {
                _ch.BasicConsume(_queue, false, _consumer);
            }


            while (true)
            {
                lock (_lockObj)
                {
                    if (_stop) return;
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

                            if (e.RoutingKey.Contains(GPSRoute.OriginalGPSKey))  ////GPS
                            {
                                var str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
                                //GPS model = new GPS(str);
                                //messageCallBack.MessageCallBack(model);
                            }
                            else
                            {
                                var model = ConvertHelper.BytesToObject(bytes);
                                LoggerManager.Logger.Info("Message Call Back " + model.ToString());
                                messageCallBack.MessageCallBack(model);
                            }


                            _ch.BasicAck(e.DeliveryTag, false);
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerManager.Logger.Error("RecMessage", ex);
                        ClearConn();
                        return;
                    }
                }
            }
        }

        private void HeartbeatSending(string queue)
        {
            HeartbeatInfo heartbeatInfo = new HeartbeatInfo();
            heartbeatInfo.SessionId = queue;
            while (true)
            {
                try
                {
                    lock (_lockObj)
                    {
                        if (_stop)
                        {
                            return;
                        }

                        if (messageCallBack == null)
                        {
                            continue;
                        }

                        heartbeatInfo.CurrentTime = DateTime.Now;
                        messageCallBack.MessageCallBack(heartbeatInfo);
                    }

                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                    ClearConn();
                    return;
                }
                Thread.Sleep(HeartbeatSpan);
            }
        }
    }
}
