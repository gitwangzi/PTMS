using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.MQ;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Newtonsoft.Json;
using Gsafety.PTMS.BaseInfo;

namespace Gsafety.PTMS.TransferLib
{
    public class TransferProcess
    {
        private static IModel _shareCh;
        private static IConnection _shareConn;
        private static QueueingBasicConsumer _shareConsumer;
        private static bool _shareStop = false;

        public static BaseService baseservice = new BaseService();
        private static bool _ShareConnected = false;
        private static string _shareQueue = "Transfer.Share";
        private static HttpSelfHostServer host = null;
        public static void Start()
        {
            try
            {
                System.Diagnostics.Debugger.Launch();
                ////cache data
                if (CacheData())
                {
                    string ServiceUrl = System.Configuration.ConfigurationManager.AppSettings["ServiceUrl"].ToString();

                    HttpSelfHostConfiguration config = new HttpSelfHostConfiguration(ServiceUrl);
                    config.Routes.MapHttpRoute("location", "api/{controller}/{vehicleID}/{clientID}/");
                    config.Routes.MapHttpRoute("locationhistory", "api/{controller}/{vehicleID}/{startTime}/{endTime}/{cliendID}/");
                    config.Routes.MapHttpRoute("appealfeedback", "api/{controller}/{alarmid}/{operationTime}/{operationperson}/{judgeresult}/{note}/");
                    config.Routes.MapHttpRoute("disposefeedback", "api/{controller}/{alarmid}/{alarmType}/{operationTime}/{operationperson}/{judgeresult}/{note}/");
                    host = new HttpSelfHostServer(config);
                    host.OpenAsync().Wait();

                    Task.Factory.StartNew(() => { DequeueShareMessage(); });

                    LoggerManager.Logger.Info("TransferProcess service starts successfully!");
                }
                else
                {
                    LoggerManager.Logger.Error("TransferProcess service failed to Start!");
                }
            }
            catch (Exception ex)
            {
                baseservice.Error(ex,"TransferService");
                LoggerManager.Logger.Error("An exception occurred when the TransferProcess service starts!" + ex);
            }
        }

        public static void Stop()
        {
            _shareStop = true;
            ClearBusinessConn();
            host.CloseAsync();
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
                    lock (CacheDataManager.DistrictNames)
                    {
                        foreach (var item in entites.BSC_DISTRICT.ToList())
                        {
                            CacheDataManager.DistrictNames.Add(item.CODE, item.NAME);
                        }
                    }

                    lock (CacheDataManager.Districts)
                    {
                        foreach (var item in entites.CFG_TRANSFER_MAPPING.ToList())
                        {
                            CacheDataManager.Districts.Add(item.DISTRICT_CODE, item.URL);
                        }
                    }
                }
                catch (Exception ex)
                {
                    baseservice.Error(ex, "TransferService");
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
                        LoggerManager.Logger.Info(e.RoutingKey);

                        if (e.RoutingKey.Contains(AlarmRoute.DirectTransferKey))
                        {
                            AlarmInfoEx model = (AlarmInfoEx)ConvertHelper.BytesToObject(e.Body);
                            LoggerManager.Logger.Info("Model:" + model);
                            LoggerManager.Logger.Info("DistrictCode:" + model.DistrictCode);
                            if (CacheDataManager.DistrictNames.ContainsKey(model.DistrictCode))
                            {
                                try
                                {
                                    foreach (var item in CacheDataManager.Districts)
                                    {
                                        LoggerManager.Logger.Info("Cache Item:" + item.Key + ":" + item.Value);
                                    }

                                    string url = string.Empty;
                                    string code = string.Empty;
                                    if (CacheDataManager.Districts.ContainsKey(model.DistrictCode))
                                    {
                                        url = CacheDataManager.Districts[model.DistrictCode];
                                    }
                                    else
                                    {
                                        code = model.DistrictCode;
                                        while (!CacheDataManager.Districts.ContainsKey(code))
                                        {
                                            int indext = model.DistrictCode.LastIndexOf("-");
                                            if (indext != -1)
                                            {
                                                code = model.DistrictCode.Substring(0, indext);
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }

                                        if (CacheDataManager.Districts.ContainsKey(code))
                                        {
                                            url = CacheDataManager.Districts[code];
                                        }
                                    }
                                    LoggerManager.Logger.Info("URL:" + url);

                                    if (url != string.Empty)
                                    {

                                        if (!url.EndsWith("/"))
                                        {
                                            url += "/";
                                        }
                                        url += System.Configuration.ConfigurationManager.AppSettings["AppealPart"].ToString();                           
                                        AppealModel appealmodel = new AppealModel();
                                        appealmodel.centerCode = "000000";
                                        appealmodel.deviceId = model.VehicleId;
                                        appealmodel.deviceName = model.VehicleSn;
                                        appealmodel.systemName = "PTMS";
                                        appealmodel.factoryCode = "PTMS";
                                        appealmodel.alarmId = model.ID;
                                        appealmodel.alarmType = "5";
                                        appealmodel.alarmPersonId = model.ClientId;
                                        appealmodel.alarmPersonName = "";
                                        appealmodel.alarmDescription = model.AlarmContent;
                                        appealmodel.incidentTime = model.AlarmTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                                        appealmodel.incidentLevel = model.IncidentLevel;
                                        appealmodel.incidentAddress = model.IncidentAddress;
                                        appealmodel.districtCode = model.DistrictCode;
                                        appealmodel.longitude = model.Longitude;
                                        appealmodel.latitude = model.Latitude;
                                        appealmodel.installerName = "";
                                        appealmodel.installerPhone = "";
                                        appealmodel.creatTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");                                        
                                        
                                        HttpClient client = new HttpClient();
                                        ServicePointManager.ServerCertificateValidationCallback = (sendern, certificate, chain, sslPolicyErrors) => true;
                                        var hrm = client.PostAsJsonAsync(url, appealmodel).Result;
                                        if (hrm.IsSuccessStatusCode)
                                        {
                                            string id = hrm.Content.ReadAsStringAsync().Result;
                                            ReturnMessage message = JsonHelper.FromJsonString<ReturnMessage>(id);
                                            LoggerManager.Logger.Info("message success: "+message.success);
                                            LoggerManager.Logger.Info("message data:" +message.data);
                                            LoggerManager.Logger.Info("message dataCount:" + message.dataCount.ToString());
                                            if (message.error != null)
                                            {
                                                LoggerManager.Logger.Info("message error code:" + message.error.code);
                                                LoggerManager.Logger.Info("message error msg:" + message.error.message);
                                            }

                                            if (message.success.ToLower() == "true")
                                            {
                                                LoggerManager.Logger.Info("ALM_911_DISPOSE found Model:" + model.ID);
                                                using (PTMSEntities entites = new PTMSEntities())
                                                {
                                                    ALM_911_DISPOSE record = entites.ALM_911_DISPOSE.FirstOrDefault(n => n.ALARM_ID == model.ID);
                                                    LoggerManager.Logger.Info("ALM_911_DISPOSE found:" + record.ID);
                                                    if (record != null)
                                                    {
                                                        record.FORWARDED_FLAG = 1;
                                                        record.INCIDENT_ID = id;
                                                        record.FORWARD_TIME = DateTime.UtcNow;
                                                        record.INCIDENT_ID = message.data;

                                                        entites.SaveChanges();
                                                    }
                                                    else
                                                    {
                                                        record = new ALM_911_DISPOSE();
                                                        record.ID = Guid.NewGuid().ToString();
                                                        record.FORWARD_TIME = DateTime.UtcNow;
                                                        record.INCIDENT_ID = message.data;
                                                        record.FORWARDED_FLAG = 1;
                                                        record.ALARM_ID = model.ID;
                                                        record.CREATE_TIME = DateTime.UtcNow;
                                                        entites.ALM_911_DISPOSE.Add(record);
                                                        entites.SaveChanges();

                                                    }

                                                }

                                                //reply
                                                _shareCh.BasicAck(e.DeliveryTag, false);
                                            }
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    _ShareConnected = false;
                                    ClearBusinessConn();
                                }
                            }
                        }
                        else if (e.RoutingKey.Contains(AlarmRoute.JudgeTransferKey))
                        {

                            AlarmInfoEx model = (AlarmInfoEx)ConvertHelper.BytesToObject(e.Body);
                            LoggerManager.Logger.Info("Model:" + model);
                            LoggerManager.Logger.Info("DistrictCode:" + model.DistrictCode);

                            if (CacheDataManager.DistrictNames.ContainsKey(model.DistrictCode))
                            {
                                try
                                {
                                    foreach (var item in CacheDataManager.Districts)
                                    {
                                        LoggerManager.Logger.Info("Cache Item:" + item.Key + ":" + item.Value);
                                    }

                                    string url = string.Empty;
                                    string code = string.Empty;
                                    if (CacheDataManager.Districts.ContainsKey(model.DistrictCode))
                                    {
                                        url = CacheDataManager.Districts[model.DistrictCode];
                                    }
                                    else
                                    {
                                        code = model.DistrictCode;
                                        while (!CacheDataManager.Districts.ContainsKey(code))
                                        {
                                            int indext = model.DistrictCode.LastIndexOf("-");
                                            if (indext != -1)
                                            {
                                                code = model.DistrictCode.Substring(0, indext);
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }

                                        if (CacheDataManager.Districts.ContainsKey(code))
                                        {
                                            url = CacheDataManager.Districts[code];
                                        }
                                    }
                                    LoggerManager.Logger.Info("URL:" + url);

                                    if (url != string.Empty)
                                    {
                                        if (!url.EndsWith("/"))
                                        {
                                            url += "/";
                                        }

                                        url += System.Configuration.ConfigurationManager.AppSettings["DisposePart"].ToString(); ;
                                       // DisposeModel disposalmodel = new DisposeModel();
                                        LoggerManager.Logger.Info("url" + url);

                                        AppealModel appealmodel = new AppealModel();
                                        appealmodel.centerCode = "000000";
                                        appealmodel.deviceId = model.MdvrCoreId;
                                        appealmodel.deviceName = model.VehicleId;
                                        appealmodel.systemName = "GSEYE";
                                        appealmodel.factoryCode = "gsafety-gseye";
                                        appealmodel.alarmId = model.ID;
                                        appealmodel.alarmType = "5";
                                        appealmodel.alarmPersonId = model.User;
                                        appealmodel.alarmPersonName = model.UserName;
                                        appealmodel.alarmDescription = model.AlarmContent;
                                        appealmodel.incidentTime = model.AlarmTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                                        appealmodel.incidentLevel = model.IncidentLevel;
                                        //appealmodel.incidentAddress = model.IncidentAddress;
                                        appealmodel.incidentType= model.IncidentType;
                                        //if (string.IsNullOrEmpty(model.IncidentAddress))
                                        //{
                                        //   appealmodel.incidentAddress = model.Province +" "+ model.City;                                        
                                        //}                                       

                                        //appealmodel.districtCode = model.DistrictCode;
                                        appealmodel.longitude = model.Longitude;
                                        appealmodel.latitude = model.Latitude;

                                        //if (appealmodel.longitude != null && appealmodel.latitude != null)
                                        //{

                                        //    MapPoint mapPoint =
                                        //                           ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(new MapPoint
                                        //                               (Convert.ToDouble(appealmodel.longitude, System.Globalization.CultureInfo.InvariantCulture),
                                        //                               Convert.ToDouble(appealmodel.latitude, System.Globalization.CultureInfo.InvariantCulture)));

                                        //    appealmodel.longitude = mapPoint.X.ToString();
                                        //    appealmodel.latitude = mapPoint.Y.ToString();
                                        //}
                                      
                                        appealmodel.installerName = "";
                                        appealmodel.installerPhone = "";
                                        appealmodel.installationTime = "";
                                        appealmodel.creatTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");;
                                        
                                        LoggerManager.Logger.Info("The JudgeTransferKey Transfer id:"+ appealmodel.alarmId);
                                        LoggerManager.Logger.Info("The JudgeTransferKey Transfer appealCenterCode:" + appealmodel.centerCode);


                                        LoggerManager.Logger.Info(JsonConvert.SerializeObject(appealmodel));
                                        HttpClient client = new HttpClient();
                                        ServicePointManager.ServerCertificateValidationCallback = (sendern, certificate, chain, sslPolicyErrors) => true;
                                        var hrm = client.PostAsJsonAsync(url, appealmodel).Result;
                                        if (hrm.IsSuccessStatusCode)
                                        {
                                            //string id = hrm.Content.ReadAsStringAsync().Result;
                                            //ReturnMessage message = JsonHelper.FromJsonString<ReturnMessage>(id);
                                            //LoggerManager.Logger.Info("message success: " + message.success);
                                            //LoggerManager.Logger.Info("message data:" + message.data);
                                            //LoggerManager.Logger.Info("message dataCount:" + message.dataCount.ToString());
                                            //if (message.error != null)
                                            //{
                                            //    LoggerManager.Logger.Info("message error code:" + message.error.code);
                                            //    LoggerManager.Logger.Info("message error msg:" + message.error.message);
                                            //}

                                            //if (message.success.ToLower() == "true")
                                            //{
                                                try
                                                {
                                                    LoggerManager.Logger.Info("ALM_911_DISPOSE found Model:" + model.ID);
                                                    using (PTMSEntities entites = new PTMSEntities())
                                                    {
                                                        ALM_911_DISPOSE record = entites.ALM_911_DISPOSE.FirstOrDefault(n => n.ALARM_ID == model.ID);
                                                        LoggerManager.Logger.Info("ALM_911_DISPOSE found:" + record.ID);
                                                        if (record != null)
                                                        {
                                                            record.FORWARDED_FLAG = 1;
                                                            record.INCIDENT_ID = model.ID;
                                                            record.FORWARD_TIME = DateTime.UtcNow;
                                                            record.INCIDENT_ID = model.ID;

                                                            entites.SaveChanges();
                                                        }
                                                    }
                                                    //reply
                                                    _shareCh.BasicAck(e.DeliveryTag, false);

                                                }
                                                catch (Exception ex)
                                                {
                                                    LoggerManager.Logger.Error(ex);
                                                 
                                                }

                                            //}


                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    baseservice.Error(ex, "TransferService");
                                    LoggerManager.Logger.Error(ex);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    baseservice.Error(ex, "TransferService");
                    _ShareConnected = false;
                    ClearBusinessConn();
                    LoggerManager.Logger.Error(ex);
                }
            }
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

                _shareCh.ExchangeDeclare(Constdefine.APPEXCHANGE, "topic", true);
                ////business queue
                _shareCh.QueueDeclare(_shareQueue, true, false, false, null);

                _shareCh.QueueBind(_shareQueue, Constdefine.APPEXCHANGE, AlarmRoute.DirectTransferKey + "*");
                _shareCh.QueueBind(_shareQueue, Constdefine.APPEXCHANGE, AlarmRoute.JudgeTransferKey + "*");

                _shareCh.BasicQos(0, 200, false);
                _shareConsumer = new QueueingBasicConsumer(_shareCh);
                _shareCh.BasicConsume(_shareQueue, false, _shareConsumer);
                _ShareConnected = true;

            }
            catch (Exception ex)
            {
                baseservice.Error(ex, "TransferService");
                _ShareConnected = false;
                ClearBusinessConn();
                LoggerManager.Logger.Error(ex);
            }
            return _ShareConnected;
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
                baseservice.Error(ex, "TransferService");
                LoggerManager.Logger.Error(ex);
            }
        }
    }
}
