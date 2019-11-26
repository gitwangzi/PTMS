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
using Gsafety.PTMS.Common.Data;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;
using System.Transactions;



namespace Gs.PTMS.Service
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
        [Obsolete]
        public MultiMessage<Gsafety.PTMS.Common.Data.AlarmInfoEx> GetHandledAlarms(string carNumber, DateTime? startTime, DateTime? endTime, short? isTrueAlarm, PagingInfo pagingInfo, string clientid)
        {
            try
            {
                Info("Method:GetHandledAlarms");
                Info("carNumber:" + Convert.ToString(carNumber) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime) + ";" + "isTrueAlarm:" + Convert.ToString(isTrueAlarm) + ";" + "pagingInfo:" + Convert.ToString(pagingInfo));
                UserInfoMessageHeader userInfo = GetUserInfo();
                MultiMessage<AlarmInfoEx> result = new MultiMessage<AlarmInfoEx>();

                using (PTMSEntities context = new PTMSEntities())
                {
                    result = _ehicleAlarmRepository.GetSecurityAlarms(context, carNumber, startTime, endTime, isTrueAlarm, pagingInfo);
                }


                this.Log<AlarmInfoEx>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Gsafety.PTMS.Common.Data.AlarmInfoEx>() { Result = null, ExceptionMessage = ex };
            }
        }

        public MultiMessage<AlarmInfoEx> GetAllAlarms(string carNumber, DateTime? startTime, DateTime? endTime, PagingInfo pagingInfo, string clientid, List<string> organizations)
        {
            try
            {
                Info("Method:GetHandledAlarms");
                Info("carNumber:" + Convert.ToString(carNumber) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime) + ";" + "pagingInfo:" + Convert.ToString(pagingInfo));
                UserInfoMessageHeader userInfo = GetUserInfo();
                MultiMessage<AlarmInfoEx> result = new MultiMessage<AlarmInfoEx>();

                using (PTMSEntities context = new PTMSEntities())
                {
                    result = _ehicleAlarmRepository.GetAllAlarms(context, carNumber, startTime, endTime, pagingInfo, organizations);
                }


                this.Log<AlarmInfoEx>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Gsafety.PTMS.Common.Data.AlarmInfoEx>() { Result = null, ExceptionMessage = ex };
            }
        }

        public MultiMessage<AlarmNoteInfo> GetAllAlarmNote(string clientid)
        {
            try
            {
                Info("Method:GetAllAlarmNote");


                MultiMessage<AlarmNoteInfo> result = new MultiMessage<AlarmNoteInfo>();

                using (PTMSEntities context = new PTMSEntities())
                {
                    result = _ehicleAlarmRepository.GetAllAlarmNote(context, clientid);
                }


                this.Log<AlarmNoteInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Gsafety.PTMS.Common.Data.AlarmNoteInfo>() { Result = null, ExceptionMessage = ex };
            }
        }

        public MultiMessage<AlarmTypeInfo> GetAllAlarmType()
        {
            try
            {
                Info("Method:GetAllAlarmType");
                MultiMessage<Gsafety.PTMS.Common.Data.AlarmTypeInfo> result = null;

                string IncidentTypeUrl = System.Configuration.ConfigurationManager.AppSettings["IncidentTypeUrl"];


                string Typeinfo = GetDataAsync(IncidentTypeUrl);


                if (!string.IsNullOrEmpty(Typeinfo))
                {
                    List<AlarmTypeInfo> finduser = JsonConvert.DeserializeObject<List<AlarmTypeInfo>>(Typeinfo);
                    result = new MultiMessage<Gsafety.PTMS.Common.Data.AlarmTypeInfo>
                    {
                        TotalRecord = finduser.Count(),
                        Result = finduser
                    };
                    return result;
                }

                return new MultiMessage<Gsafety.PTMS.Common.Data.AlarmTypeInfo>() { Result = null, ExceptionMessage = null };

            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Gsafety.PTMS.Common.Data.AlarmTypeInfo>() { Result = null, ExceptionMessage = ex };
            }
         

        }

        public string GetDataAsync(string url)
        {
            var result = string.Empty;
            try
            {

                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                var httpwebrequest = (HttpWebRequest)WebRequest.Create(url);
                httpwebrequest.ContentType = "application/json";
                httpwebrequest.Method = "Get";


                var httpResponse = (HttpWebResponse)httpwebrequest.GetResponse();
                using (var sr = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    return result;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return null;

            }
        }

        public string PostDataAsync(string url, string arg)
        {
            var result = string.Empty;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                var httpwebrequest = (HttpWebRequest)WebRequest.Create(url);
                httpwebrequest.ContentType = "application/json";
                httpwebrequest.Method = "POST";
                if (!string.IsNullOrEmpty(arg))
                {
                    using (var sw = new StreamWriter(httpwebrequest.GetRequestStream()))
                    {
                        string param = arg;
                        sw.Write(param);
                        sw.Flush();
                        sw.Close();

                    }
                }

                var httpResponse = (HttpWebResponse)httpwebrequest.GetResponse();
                using (var sr = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    return result;

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return null;

            }

        }

        public MultiMessage<AlarmEmailInfo> GetAllAlarmEmail(string clientid)
        {
            try
            {
                Info("Method:GetAllAlarmEmail");


                MultiMessage<AlarmEmailInfo> result = new MultiMessage<AlarmEmailInfo>();

                using (PTMSEntities context = new PTMSEntities())
                {
                    result = _ehicleAlarmRepository.GetAllAlarmEmail(context, clientid);
                }


                this.Log<AlarmEmailInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Gsafety.PTMS.Common.Data.AlarmEmailInfo>() { Result = null, ExceptionMessage = ex };
            }
        }


        public SingleMessage<bool> AddAlarmEmail(AlarmEmailInfo eamil)
        {
            try
            {
                Info("Method:AddAlarmEmail");              
                var result = new SingleMessage<bool>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = _ehicleAlarmRepository.AddAlarmEmail(context, eamil);
                }

                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }


        public SingleMessage<bool> UpdateAlarmEmail(AlarmEmailInfo eamil)
        {
            try
            {
                Info("Method:UpdateAlarmEmail");
                var result = new SingleMessage<bool>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = _ehicleAlarmRepository.UpdateAlarmEmail(context, eamil);
                }

                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        public SingleMessage<bool> DeleteAlarmEmail(string ID)
        {
            try
            {
                Info("Method:DeleteAlarmEmail");
                Info("DeleteAlarmEmail:" + Convert.ToString(ID));
                var result = new SingleMessage<bool>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = _ehicleAlarmRepository.DeleteAlarmEmail(context, ID);
                }

                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        public SingleMessage<bool> AddAlarmNote(string ID,string clientID,string note)
        {
            try
            {
                Info("Method:AddAlarmNote");
                Info("AddAlarmNote:" + Convert.ToString(ID));
                var result = new SingleMessage<bool>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = _ehicleAlarmRepository.AddAlarmNote(context, ID,clientID,note);
                }

                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        public SingleMessage<bool> UpdateAlarmNote(string ID,string note)
        {
            try
            {
                Info("Method:UpdateAlarmNote");
                Info("UpdateAlarmNote:" + Convert.ToString(ID));
                var result = new SingleMessage<bool>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = _ehicleAlarmRepository.UpdateAlarmNote(context, ID, note);
                }

                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }
        public SingleMessage<bool> DeleteAlarmNote(string ID)
        {
            try
            {
                Info("Method:DeletAlarmNote");
                Info("DeleteAlarmNote:" + Convert.ToString(ID));
                var result = new SingleMessage<bool>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = _ehicleAlarmRepository.DeleteAlarmNote(context, ID);
                }

                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }
        public MultiMessage<AlarmInfoEx> GetUnHandledAllAlarms(PagingInfo pagingInfo, string clientid)
        {
            Info("Method:GetUnHandledAllAlarms");
            Info(pagingInfo.ToString());
            try
            {
                Info(pagingInfo.ToString());
                UserInfoMessageHeader userinfo = GetUserInfo();
                MultiMessage<AlarmInfoEx> result = new MultiMessage<AlarmInfoEx>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    //result = _ehicleAlarmRepository.GetUnHandledAllAlarms(context, userinfo, pagingInfo);
                }

                this.Log<AlarmInfoEx>(result);
                return result;
            }
            catch (Exception ex)
            {
                return new MultiMessage<AlarmInfoEx>() { ExceptionMessage = ex, Result = null };
            }
        }

        public MultiMessage<AlarmInfoEx> GetUnHandledAlarms(string clientid, List<string> Organizations)
        {
            Info("Method:GetUnHandledAllAlarms");
            Info(clientid.ToString());
            if (Organizations != null && Organizations.Count > 0)
            {
                Info("Organizations");
                foreach (string organization in Organizations)
                {
                    Info("Organization:" + organization);
                }
            }
            try
            {
                MultiMessage<AlarmInfoEx> result = null;

                result = new MultiMessage<AlarmInfoEx>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = _ehicleAlarmRepository.GetUnHandledAllAlarms(context, clientid, Organizations);
                }


                this.Log<AlarmInfoEx>(result);
                return result;
            }
            catch (Exception ex)
            {
                return new MultiMessage<AlarmInfoEx>() { ExceptionMessage = ex, Result = null };
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

        public SingleMessage<AlarmHandleResult> HandleAlarm(string alarmid, string user, bool alarmresult, string note, DateTime time, bool istransfer, int transfermode, int incidentlevel, string incidentaddress, string incidenttype)
        {

            try
            {
                Info("Method:HandleAlarm");
                Info("alarmid:" + Convert.ToString(alarmid));
                Info("user:" + Convert.ToString(user));
                Info("alarmresult:" + Convert.ToString(alarmresult));
                Info("note:" + Convert.ToString(note));

                SingleMessage<AlarmHandleResult> result = null;
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = _ehicleAlarmRepository.HandleAlarm(context, alarmid, user, alarmresult, note, time, istransfer, transfermode, incidentlevel, incidentaddress,incidenttype);
                }

                Log<AlarmHandleResult>(result);

                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<AlarmHandleResult>() { Result = null, IsSuccess = false };
            }
        }


        public SingleMessage<ApealDispose> GetApealDisposeByAlarmID(string alarmID)
        {
            Info("GetApealDisposeByAlarmID");
            Info(alarmID.ToString());
            try
            {
                SingleMessage<ApealDispose> result = null;
                using (var context = new PTMSEntities())
                {
                    result = _ehicleAlarmRepository.GetApealDisposeByAlarmID(context, alarmID);
                }
                Log<ApealDispose>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<ApealDispose>(false, ex);
            }
        }


        public SingleMessage<TransferDispose> GetTransferDisposeByAlarmID(string AlarmID)
        {
            Info("GetTransferDisposeByAlarmID");
            Info(AlarmID.ToString());
            try
            {
                SingleMessage<TransferDispose> result = null;
                using (var context = new PTMSEntities())
                {
                    result = _ehicleAlarmRepository.GetTransferDisposeByAlarmID(context, AlarmID);
                }
                Log<TransferDispose>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<TransferDispose>(false, ex);
            }
        }

        public SingleMessage<int> GetTransferDisposeByAlarmID_CAD(string AlarmID)
        {
            Info("GetTransferDisposeByAlarmID_CAD");
            Info(AlarmID.ToString());
            try
            {
                SingleMessage<int> result = null;

                string IncidentStateUrl = System.Configuration.ConfigurationManager.AppSettings["IncidentStateUrl"];

                if (IncidentStateUrl != string.Empty)
                {

                    if (!IncidentStateUrl.EndsWith("/"))
                    {
                        IncidentStateUrl += "/";
                    }
                }

                string stateinfo = GetDataAsync(IncidentStateUrl + AlarmID);

                int status = 1;

                if (!string.IsNullOrEmpty(stateinfo))
                {
                    if (Int32.TryParse(stateinfo,out status))
                    {
                        result = new SingleMessage<int>
                        {
                            IsSuccess=true,
                            Result=status
                        };
                    
                    }
                    else
                    { 
                    
                    }
                }


                Log<int>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<int>(false, ex);
            }
        }

        /// <summary>
        /// 安装流程中检查报警
        /// </summary>
        /// <param name="installationDetailID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public SingleMessage<bool> GetAlarmCheck(string installationDetailID, DateTime date)
        {
            try
            {
                Info("Method:GetAlarmCheck");
                Info("installationDetailID:" + Convert.ToString(installationDetailID));
                var result = new SingleMessage<bool>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = _ehicleAlarmRepository.GetAlarmCheck(context, installationDetailID, date);
                }

                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        /// <summary>
        /// 安装流程中检查报警
        /// </summary>
        /// <param name="installationDetailID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public SingleMessage<AlarmInfoEx> InsertManualAlarm(ManualAlarmInfo alarminfo)
        {
            try
            {
                Info("Method:InsertManualAlarm");
                Info(alarminfo);
                SingleMessage<AlarmInfoEx> result = null;
                alarminfo.GPSTime = DateTime.UtcNow;
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = _ehicleAlarmRepository.InsertManualAlarm(context, alarminfo);
                }

                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<AlarmInfoEx>(false, ex);
            }
        }
    }
}
