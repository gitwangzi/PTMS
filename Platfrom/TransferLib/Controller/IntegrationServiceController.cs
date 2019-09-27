using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.MQ;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.TransferLib.Filter;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Gsafety.PTMS.TransferLib.Controller
{
    public class IntegrationServiceController : ApiController
    {
        [CustomerHeaderFilter]
        [HttpGet]
        public Location GetLocation(string vehicleID, string clientID)
        {
            try
            {
                LoggerManager.Logger.Info("GetLocation vehicleID:" + vehicleID + ";clientID:" + clientID);

                using (var context = new PTMSEntities())
                {
                    var result = context.RUN_VEHICLE_LOCATION.OrderByDescending(x => x.GPS_TIME)
                          .FirstOrDefault(x => x.VEHICLE_ID == vehicleID);
                    if (result != null)
                    {
                        return LocationConvert(result);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
            return null;
        }

        private Location LocationConvert(RUN_VEHICLE_LOCATION result)
        {
            return new Location
            {
                Direction = result.DIRECTION,
                GPSTime = result.GPS_TIME.HasValue ? result.GPS_TIME.Value.ToString("yyyy-MM-dd hh:mm:ss") : string.Empty,
                Latitude = result.LATITUDE,
                Longitude = result.LONGITUDE,
                Speed = result.SPEED,
            };
        }

        [CustomerHeaderFilter]
        [HttpGet]
        public List<Location> GetLocationHistory(string vehicleID, string startTime, string endTime, string cliendID)
        {
            try
            {
                string msg = "[GetLocationHistory]Args:";
                msg += string.Format("vehicleId={0}", vehicleID);
                msg += string.Format("startTime={0}", startTime);
                msg += string.Format("endTime={0}", endTime);
                LoggerManager.Logger.Info(msg);
                DateTime start = DateTime.Parse(startTime);
                DateTime end = DateTime.Parse(endTime);
                List<Location> locations = new List<Location>();
                using (var context = new PTMSEntities())
                {
                    var result = context.RUN_VEHICLE_LOCATION.OrderByDescending(x => x.GPS_TIME)
                          .Where(n => n.GPS_TIME >= start && n.GPS_TIME <= end && n.VEHICLE_ID == vehicleID).ToList();
                    foreach (var item in result)
                    {
                        locations.Add(LocationConvert(item));
                    }
                }

                return locations;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                return null;
            }
        }

        [CustomerHeaderFilter]
        [HttpGet]
        public ReturnMessage AppealFeedBack(string alarmid, string operationTime, string operationperson, bool judgeresult, string note)
        {
            ReturnMessage message = new ReturnMessage();
            try
            {
                string msg = "[AppealFeedBack]Args:";
                msg += string.Format("alarmid={0}", alarmid);
                msg += string.Format("operationperson={0}", operationperson);
                msg += string.Format("judgeresult={0}", judgeresult);
                msg += string.Format("note={0}", note);
                LoggerManager.Logger.Info(msg);
                using (var context = new PTMSEntities())
                {
                    var result = context.ALM_911_DISPOSE.FirstOrDefault(n => n.ALARM_ID == alarmid);
                    
                    if (result != null)
                    {
                        result.APPREAL_STAFF = operationperson;
                        result.APPEAL_TIME = DateTime.Parse(operationTime);
                        if (judgeresult)
                        {
                            result.ALARM_FLAG = 1;
                        }
                        result.CONTENT = note;
                        context.SaveChanges();

                         var alarmrecord = context.ALM_ALARM_RECORD.FirstOrDefault(n => n.ALARM_UID == alarmid);
                        if (alarmrecord != null)
                        {
                            CompleteAlarm ca = new CompleteAlarm();
                            ca.AlarmGuid = alarmrecord.ALARM_UID;
                            ca.ClientID = alarmrecord.CLIENT_ID;
                            ca.AlarmTime = alarmrecord.GPS_TIME.Value;
                            ca.CompleteTime = result.APPEAL_TIME.Value;
                            ca.HandlerID = result.APPREAL_STAFF;
                            ca.IsRealAlarm = result.ALARM_FLAG==1;
                            ca.VehicleID = alarmrecord.VEHICLE_ID;
                            ca.MdvrCoreId = alarmrecord.MDVR_CORE_SN;

                            var vehicle=context.BSC_VEHICLE.FirstOrDefault(n => n.VEHICLE_ID == ca.VehicleID);
                            if (vehicle != null)
                            {
                                ca.Organizations = new List<string>();
                                ca.Organizations.Add(vehicle.ORGNIZATION_ID);

                                byte[] sendbytes = ConvertHelper.ObjectToBytes(ca);

                                IConnection _conn = null;
                                IModel _ch = null;
                                try
                                {

                                    while (_conn == null)
                                    {
                                        _conn = new MQFactory().CreateConnection();
                                    }

                                    _ch = _conn.CreateModel();

                                    IBasicProperties bp = _ch.CreateBasicProperties();

                                    bp.Type = "1";
                                    bp.Priority = 1;
                                    _ch.BasicPublish(Constdefine.APPEXCHANGE, AlarmRoute.CompleteAlarmNotice, bp, sendbytes);
                                }
                                catch (Exception ex)
                                {
                                    LoggerManager.Logger.Error("SendMessage", ex);
                                }
                                finally
                                {
                                    if (_ch != null)
                                    {
                                        if (_ch.IsOpen)
                                        {
                                            _ch.Close();
                                            _ch.Dispose();
                                        }
                                    }

                                    if (_conn != null)
                                    {
                                        _conn.Close();
                                        _conn.Dispose();
                                    }
                                }
                            }
                        }

                        message.success = "true";
                    }
                    else
                    {
                        message.success = "false";
                        message.error = new ErrorDetail();
                        message.error.message = "record not in exist in database";
                    }
                }

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                message.success = "false";
                message.error = new ErrorDetail();
                message.error.message = ex.Message;
            }

            return message;
        }

        [CustomerHeaderFilter]
        [HttpGet]
        public ReturnMessage DisposeFeedBack(string alarmid, string alarmType, string operationTime, string operationperson, bool judgeresult, string note)
        {
            ReturnMessage message = new ReturnMessage();
            try
            {
                string msg = "[DisposeFeedBack]Args:";
                msg += string.Format("alarmid={0}", alarmid);
                msg += string.Format("alarmType={0}", alarmType);
                msg += string.Format("operationperson={0}", operationperson);
                msg += string.Format("judgeresult={0}", judgeresult);
                msg += string.Format("note={0}", note);
                LoggerManager.Logger.Info(msg);
                using (var context = new PTMSEntities())
                {
                    var result = context.ALM_911_DISPOSE.FirstOrDefault(n => n.ALARM_ID == alarmid);
                    if (result != null)
                    {
                        ALM_ALARM_RECORD record = context.ALM_ALARM_RECORD.FirstOrDefault(n => n.ID == alarmid);
                        if (record != null)
                        {
                            record.STATUS = 4;
                        }

                        result.DISPOSE_STAFF = operationperson;
                        if (judgeresult)
                        {
                            result.ALARM_FLAG = 1;
                        }
                        result.INCIDENT_TYPE = alarmType;
                        result.DISPOSE_TIME = DateTime.UtcNow;
                        result.DISPOSE_CONTENT = note;
                        context.SaveChanges();

                        message.success = "true";
                    }
                    else
                    {
                        message.success = "false";
                        message.error = new ErrorDetail();
                        message.error.message = "record not in exist in database";
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                message.success = "false";
                message.error = new ErrorDetail();
                message.error.message = ex.Message;
            }

            return message;
        }
    }
}
