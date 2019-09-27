using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.Alarm.Contract;
using Gsafety.PTMS.Alarm.Contract.Data;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Alarm.Repository;
using Gsafety.Common.Logging;
using Gsafety.PTMS.BaseInfo;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using System.Threading.Tasks;
using System.Diagnostics;
using Gsafety.PTMS.DBEntity;


namespace Gsafety.PTMS.Alarm.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "VehicleAlertService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select VehicleAlarmService.svc or VehicleAlarmService.svc.cs at the Solution Explorer and start debugging.
    public class VehicleAlarmService : Gsafety.PTMS.BaseInfo.BaseService, IVehicleAlarmService
    {
        private VehicleAlarmRepository _ehicleAlarmRepository = new VehicleAlarmRepository();

        /// <summary>
        /// Get Handle Alarm
        /// </summary>
        /// <returns></returns>
        public MultiMessage<AlarmInfo> GetHandledAlarms(string carNumber, DateTime? startTime, DateTime? endTime, short? isTrueAlarm, PagingInfo pagingInfo)
        {
            try
            {
                Info("Method:GetHandledAlarms");
                Info("carNumber:" + Convert.ToString(carNumber) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime) + ";" + "isTrueAlarm:" + Convert.ToString(isTrueAlarm) + ";" + "pagingInfo:" + Convert.ToString(pagingInfo));
                UserInfoMessageHeader userInfo = GetUserInfo();
                MultiMessage<AlarmInfo> result = new MultiMessage<AlarmInfo>();
                if (userInfo.Group == "SecurityMonitor" || userInfo.Group == "SecurityManager" || userInfo.Group == "SecurityAdmin" || userInfo.Group.Equals("AlarmFilterCommissioner"))
                {

                    using (PTMSEntities context = new PTMSEntities())
                    {
                        //result = _ehicleAlarmRepository.GetSecurityAlarms(context, userInfo, carNumber, startTime, endTime, isTrueAlarm, pagingInfo);
                    }

                }
                else
                {
                    result = null;
                }
                this.Log<AlarmInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<AlarmInfo>() { Result = null, ExceptionMessage = ex };
            }
        }

        public MultiMessage<AlarmInfo> GetUnHandledAllAlarms(PagingInfo pagingInfo)
        {
            Info("Method:GetUnHandledAllAlarms");
            Info(pagingInfo.ToString());
            try
            {
                Info(pagingInfo.ToString());
                UserInfoMessageHeader userinfo = GetUserInfo();
                MultiMessage<AlarmInfo> result = new MultiMessage<AlarmInfo>();
                if (userinfo.Group.Equals("SecurityManager") || userinfo.Group.Equals("SecurityAdmin") || userinfo.Group.Equals("AlarmFilterCommissioner"))
                {
                    result = new MultiMessage<AlarmInfo>();
                    using (PTMSEntities context = new PTMSEntities())
                    {
                        //result = _ehicleAlarmRepository.GetUnHandledAllAlarms(context, userinfo, pagingInfo);
                    }

                }
                this.Log<AlarmInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                return new MultiMessage<AlarmInfo>() { ExceptionMessage = ex, Result = null };
            }
        }

        /// <summary>
        /// Alarm wheather Handled
        /// </summary>
        /// <param name="vehicleAlarmId"></param>
        /// <returns></returns>
        public SingleMessage<bool> IfAlarmDetail(string vehicleAlarmId)
        {
            Info("Method:IfAlarmDetail");
            Info(vehicleAlarmId);
            try
            {
                Info("vehicleAlarmId:" + Convert.ToString(vehicleAlarmId));
                var result = new SingleMessage<bool>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = _ehicleAlarmRepository.IfAlarmDetail(context, vehicleAlarmId);
                }


                this.Log<bool>(result);

                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex };
            }
        }

        /// <summary>
        /// Get Alarm GPS
        /// </summary>
        public MultiMessage<GPS> GetAlarmGPSTrack(string vehicleId, DateTime startTime, DateTime endTime)
        {
            try
            {
                Info("Method:GetAlarmGPSTrack");
                Info("vehicleId:" + Convert.ToString(vehicleId) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime));
                var result = new List<GPS>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = _ehicleAlarmRepository.GetAlarmGPSTrack(context, vehicleId, startTime, endTime);
                }

                MultiMessage<GPS> GPSresult = new MultiMessage<GPS>() { Result = result };
                this.Log<GPS>(GPSresult);
                return GPSresult;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<GPS>() { ExceptionMessage = ex };
            }
        }

        /// <summary>
        /// find alrm from alarm_dispose
        /// </summary>
        public MultiMessage<AlarmTreatment> GetAlarmTreatments(string alarmID)
        {
            try
            {
                Info("Method:GetAlarmTreatments");
                Info("alarmID:" + Convert.ToString(alarmID));
                var result = new List<AlarmTreatment>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = _ehicleAlarmRepository.GetAlarmTreatment(context, alarmID);
                }

                MultiMessage<AlarmTreatment> atresult = new MultiMessage<AlarmTreatment>() { Result = result };
                this.Log<AlarmTreatment>(atresult);
                return atresult;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<AlarmTreatment>() { ExceptionMessage = ex };
            }
        }

        /// <summary>
        /// the 911 treatment conditions
        /// </summary>
        /// <param name="alarmID"></param>
        /// <returns></returns>
        public SingleMessage<Alarm911Treatment> GetAlarm911Treatments(string alarmID)
        {
            try
            {
                Info("Method:GetAlarm911Treatments");
                Info("alarmID:" + Convert.ToString(alarmID));
                var temp = new Alarm911Treatment();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = _ehicleAlarmRepository.GetAlarm911Treatments(alarmID);
                }

                SingleMessage<Alarm911Treatment> result = new SingleMessage<Alarm911Treatment>() { Result = temp, IsSuccess = true };
                Log<Alarm911Treatment>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<Alarm911Treatment>() { Result = null, IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<Returninfo> GetAlarm911Result(string alarmID)
        {

            try
            {
                Info("Method:GetAlarm911Result");
                Info("alarmID:" + Convert.ToString(alarmID));
                string requestUrl = ConfigurationManager.AppSettings["Alarm911Url"];
                HttpClient _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var alarm911Info = new Alarm911Treatment();
                using (PTMSEntities context = new PTMSEntities())
                {
                    alarm911Info = _ehicleAlarmRepository.GetAlarm911Treatments( alarmID);
                }

                SingleMessage<Returninfo> result = new SingleMessage<Returninfo>();
                if (alarm911Info != null && alarm911Info.INCIDENT_ID != null)
                {
                    string httpUrl = string.Format(requestUrl, alarm911Info.INCIDENT_ID);
                    Task<HttpResponseMessage> response = _httpClient.GetAsync(httpUrl);
                    response.Result.EnsureSuccessStatusCode();
                    //Task<ReturninfoTemp> task = response.Result.Content.ReadAsAsync<ReturninfoTemp>();
                    Task<ReturninfoTemp> task = null;
                    if (task.Wait(2000))
                    {
                        Returninfo temp = new Returninfo();
                        temp.Content = task.Result.Content;
                        temp.Incidentstatus = task.Result.Incidentstatus;
                        temp.status = task.Result.status;
                        temp.ErrorMsg = task.Result.ErrorMsg;

                        if (temp.status == 1)
                        {
                            result.Result = temp;
                            result.IsSuccess = true;
                        }
                        else
                        {
                            Info(temp.ErrorMsg);
                            result.Result = temp;
                            result.IsSuccess = false;
                            result.ErrorMsg = temp.ErrorMsg;
                        }
                    }
                    else
                    {
                        result.Result = null;
                        result.IsSuccess = false;
                    }
                }
                else
                {
                    result.Result = null;
                    result.IsSuccess = false;
                }

                Log<Returninfo>(result);

                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<Returninfo>() { Result = null, IsSuccess = false };
            }
        }

        public MultiMessage<AlarmInfo> GetHandledAlarms(string carNumber, DateTime? startTime, DateTime? endTime, short? isTrueAlarm, PagingInfo pagingInfo, string clientid)
        {
            throw new NotImplementedException();
        }

        public MultiMessage<AlarmInfo> GetUnHandledAllAlarms(PagingInfo pagingInfo, string clientid)
        {
            throw new NotImplementedException();
        }
    }
}
