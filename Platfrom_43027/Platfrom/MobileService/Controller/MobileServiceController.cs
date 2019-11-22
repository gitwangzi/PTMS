using Gsafety.Ant.BaseInformation.Repository;
using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.MQ;
using Gsafety.PTMS.Alarm.Repository;
using Gsafety.PTMS.Alert.Repository;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using GSafety.PTMS.PublicService.Repository;
using MobileService.Filter;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace MobileService.Controller
{
    public class MobileServiceController : ApiController
    {
        [CustomerHeaderFilter]
        [HttpGet]
        public SingleMessage<AuthenticationResult> GetAuthenticate(string sim, string vehiclenum, string license, string operationlicense)
        {
            try
            {
                LoggerManager.Logger.Info(sim + ":" + vehiclenum + ":" + license + ":" + operationlicense);
                SingleMessage<AuthenticationResult> result = null;
                string clientID = string.Empty;
                Vehicle vehicle = null;
                using (var context = new PTMSEntities())
                {
                    result = ChauffeurRepository.Authenticate(context, sim, vehiclenum, license, operationlicense);
                    if (result.IsSuccess)
                    {
                        vehicle = (from veh in context.BSC_VEHICLE.Where(v => v.VALID == 1)
                                   join orgation in context.USR_ORGANIZATION on veh.ORGNIZATION_ID equals orgation.ID
                                   join vt in context.BSC_VEHICLE_TYPE.Where(t => t.VALID == 1) on veh.VEHICLE_TYPE equals vt.ID
                                   where veh.VEHICLE_ID == vehiclenum
                                   select new Vehicle()
                                   {
                                       OrgnizationId = veh.ORGNIZATION_ID,
                                       OrgnizationName = orgation.NAME,
                                       VehicleId = veh.VEHICLE_ID,
                                       ServiceType = veh.SERVICE_TYPE == null ? VehicleSeviceType.Comercial : (VehicleSeviceType)veh.SERVICE_TYPE,
                                       VehicleType = new VehicleType() { ID = veh.VEHICLE_TYPE == null ? string.Empty : veh.VEHICLE_TYPE },
                                       Region = veh.REGION,
                                       OperationLicense = veh.OPERATION_LICENSE,
                                       Owner = veh.OWNER,
                                       Contact = veh.OWNER,
                                       ContactPhone = veh.CONTACT_PHONE,
                                       ContactAddress = veh.CONTACT_ADDRESS,
                                       BrandModel = veh.BRAND_MODEL,
                                       VehicleSn = veh.VEHICLE_SN,
                                       EngineId = veh.ENGINE_ID,
                                       StartYear = veh.START_YEAR,
                                       VehicleStatus = veh.VEHICLE_STATUS == null ? VehicleConditionType.Available : (VehicleConditionType)veh.VEHICLE_STATUS,
                                       Note = veh.NOTE,
                                       VehicleTypeDescribe = vt.NAME,
                                       VehicleTypeImage = vt.ICON,
                                       DistrictCode = veh.DISTRICT_CODE,
                                       ClientId = veh.CLIENT_ID
                                   }).FirstOrDefault();

                        var mdvrvehicle = (from suite in context.RUN_SUITE_WORKING
                                           where suite.VEHICLE_ID == vehicle.VehicleId
                                           select new Vehicle()
                                           {
                                               MDVROnline = suite.ONLINE_FLAG,
                                               MDVR_SN = suite.MDVR_CORE_SN,
                                           }).FirstOrDefault();

                        if (mdvrvehicle != null)
                        {
                            vehicle.MDVROnline = mdvrvehicle.MDVROnline;
                            vehicle.MDVR_SN = mdvrvehicle.MDVR_SN;
                        }

                        var gpsvehicle = (from gps in context.RUN_GPS_WORKING
                                          where gps.VEHICLE_ID == vehicle.VehicleId
                                          select new Vehicle()
                                          {
                                              GPSOnline = gps.ONLINE_FLAG,
                                              GPS_SN = gps.GPS_SN,
                                          }).FirstOrDefault();

                        if (gpsvehicle != null)
                        {
                            vehicle.GPSOnline = gpsvehicle.GPSOnline;
                            vehicle.GPS_SN = gpsvehicle.GPS_SN;
                        }

                        var mobilevehicle = (from mobile in context.RUN_MOBILE_WORKING
                                             where mobile.VEHICLE_ID == vehicle.VehicleId
                                             select new Vehicle()
                                             {
                                                 MobileOnline = mobile.ONLINE_FLAG,
                                                 Mobile_SN = mobile.MOBILE_NUMBER,
                                             }).FirstOrDefault();

                        if (mobilevehicle != null)
                        {
                            vehicle.MobileOnline = mobilevehicle.MobileOnline;
                            vehicle.Mobile_SN = mobilevehicle.Mobile_SN;
                        }
                    }
                }

                if (result.IsSuccess && (!string.IsNullOrEmpty(vehicle.MDVR_SN) || !string.IsNullOrEmpty(vehicle.GPS_SN)))
                {
                    byte[] sendbytes = ConvertHelper.ObjectToBytes(vehicle);

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
                        _ch.BasicPublish(Constdefine.APPEXCHANGE, UserMessageRoute.InstallCompleteNotification, bp, sendbytes);
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

                return result;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
            return null;
        }

        [CustomerHeaderFilter]
        [HttpGet]
        public MultiMessage<string> GetVehicles(string authCode, string id)
        {
            try
            {
                LoggerManager.Logger.Info(authCode + id);
                MultiMessage<string> result = new MultiMessage<string>();
                string clientID = string.Empty;
                string authcode = System.Web.HttpUtility.UrlDecode(authCode);
                if (authcode.Contains(id))
                {
                    List<string> list = new List<string>();
                    using (var context = new PTMSEntities())
                    {
                        result.Result = (from p in context.BSC_VEHICLE_CHAUFFEUR
                                         join c in context.BSC_CHAUFFEUR on p.CHAUFFEUR_ID equals c.ID
                                         where (c.CELLPHONE == id || c.PHONE == id) && c.VALID == 1
                                         select p.VEHICLE_ID).ToList();
                    }
                }
                else
                {
                    result.IsSuccess = false;
                }

                return result;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
            return null;
        }

        [CustomerHeaderFilter]
        [HttpGet]
        public MultiMessage<Gsafety.PTMS.Common.Data.AlarmInfoEx> GetAlarm(string authCode, string pageIndex, string pageValue, string starttime, string endtime)
        {
            try
            {
                LoggerManager.Logger.Info("GetAlarm");
                LoggerManager.Logger.Info(authCode);
                LoggerManager.Logger.Info(pageIndex);
                LoggerManager.Logger.Info(pageValue);
                LoggerManager.Logger.Info(starttime);
                LoggerManager.Logger.Info(endtime);
                string code = System.Web.HttpUtility.UrlDecode(authCode);
                string[] fields = code.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string carNumber = fields[1];
                int pageindex = Convert.ToInt32(pageIndex);
                int pagesize = Convert.ToInt32(pageValue);

                DateTime StartTime = GetTime(starttime);
                DateTime EndTime = GetTime(endtime);

                MultiMessage<AlarmInfoEx> result = new MultiMessage<AlarmInfoEx>();

                using (PTMSEntities context = new PTMSEntities())
                {
                    result = VehicleAlarmRepository.GetAlarm(context, carNumber, pageindex, pagesize, StartTime, EndTime);
                }


                return result;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                return new MultiMessage<Gsafety.PTMS.Common.Data.AlarmInfoEx>() { Result = null, ExceptionMessage = ex };
            }
        }

        private DateTime GetTime(string timestring)
        {
            int year = Convert.ToInt32(timestring.Substring(0, 4));
            int month = Convert.ToInt32(timestring.Substring(4, 2));
            int day = Convert.ToInt32(timestring.Substring(6, 2));
            int hour = Convert.ToInt32(timestring.Substring(8, 2));
            int min = Convert.ToInt32(timestring.Substring(10, 2));
            int second = Convert.ToInt32(timestring.Substring(12, 2));

            DateTime time = new DateTime(year, month, day, hour, min, second);

            return time;
        }

        [CustomerHeaderFilter]
        [HttpGet]
        public MultiMessage<BusinessAlertEx> GetAlert(string authCode, string pageIndex, string pageValue, string starttime, string endtime, string type)
        {
            try
            {
                LoggerManager.Logger.Info("GetAlert");
                LoggerManager.Logger.Info(authCode);
                LoggerManager.Logger.Info(pageIndex);
                LoggerManager.Logger.Info(pageValue);
                LoggerManager.Logger.Info(starttime);
                LoggerManager.Logger.Info(endtime);
                LoggerManager.Logger.Info(type);
                string code = System.Web.HttpUtility.UrlDecode(authCode);
                string[] fields = code.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string carNumber = fields[1];
                int pageindex = Convert.ToInt32(pageIndex);
                int pagesize = Convert.ToInt32(pageValue);
                DateTime StartTime = GetTime(starttime);
                DateTime EndTime = GetTime(endtime);
                MultiMessage<BusinessAlertEx> result = null;
                using (var context = new PTMSEntities())
                {
                    if (type.ToLower() == "all")
                    {
                        result = VehicleAlertRespository.GetAlert(context, carNumber, pageindex, pagesize, StartTime, EndTime, null);
                    }
                    else
                    {
                        result = VehicleAlertRespository.GetAlert(context, carNumber, pageindex, pagesize, StartTime, EndTime, Convert.ToInt32(type));
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                return new MultiMessage<BusinessAlertEx>(false, ex);
            }
        }

        [CustomerHeaderFilter]
        [HttpGet]
        public MultiMessage<FoundRegistry> GetFoundRegistry(string authCode, string pageIndex, string pageValue, string starttime, string endtime, string type)
        {
            try
            {
                LoggerManager.Logger.Info("GetFoundRegistry");
                LoggerManager.Logger.Info(authCode);
                LoggerManager.Logger.Info(pageIndex);
                LoggerManager.Logger.Info(pageValue);
                LoggerManager.Logger.Info(starttime);
                LoggerManager.Logger.Info(endtime);
                LoggerManager.Logger.Info(type);
                string code = System.Web.HttpUtility.UrlDecode(authCode);
                string[] fields = code.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string sim = fields[0];
                DateTime StartTime = GetTime(starttime);
                DateTime EndTime = GetTime(endtime);
                MultiMessage<FoundRegistry> result = null;
                using (var context = new PTMSEntities())
                {
                    if (type.ToLower() == "all")
                    {
                        result = FoundRegistryRepository.GetFoundRegistryByMobile(context, sim, pageIndex, pageValue, StartTime, EndTime, null);
                    }
                    else
                    {
                        result = FoundRegistryRepository.GetFoundRegistryByMobile(context, sim, pageIndex, pageValue, StartTime, EndTime, Convert.ToInt32(type));
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                return new MultiMessage<FoundRegistry>(false, ex);
            }
        }

        [CustomerHeaderFilter]
        [HttpGet]
        public MultiMessage<MaintainApplication> GetApplication(string authCode, string pageIndex, string pageValue, string starttime, string endtime, string type)
        {
            try
            {
                LoggerManager.Logger.Info("GetApplication");
                LoggerManager.Logger.Info(authCode);
                LoggerManager.Logger.Info(pageIndex);
                LoggerManager.Logger.Info(pageValue);
                LoggerManager.Logger.Info(starttime);
                LoggerManager.Logger.Info(endtime);
                LoggerManager.Logger.Info(type);
                string[] fields = authCode.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string vehicleid = fields[1];
                DateTime StartTime = GetTime(starttime);
                DateTime EndTime = GetTime(endtime);
                MultiMessage<MaintainApplication> result = null;
                using (var context = new PTMSEntities())
                {
                    if (type.ToLower() == "all")
                    {
                        result = FoundRegistryRepository.GetApplicationByVehicle(context, vehicleid, pageIndex, pageValue, StartTime, EndTime, null);
                    }
                    else
                    {
                        result = FoundRegistryRepository.GetApplicationByVehicle(context, vehicleid, pageIndex, pageValue, StartTime, EndTime, Convert.ToInt32(type));
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                return new MultiMessage<MaintainApplication>(false, ex);
            }
        }

        [CustomerHeaderFilter]
        [HttpGet]
        public MultiMessage<AppMessageVehicle> GetMessage(string authCode, string pageIndex, string pageValue, string starttime, string endtime, string type)
        {
            try
            {
                LoggerManager.Logger.Info("GetApplication");
                LoggerManager.Logger.Info(authCode);
                LoggerManager.Logger.Info(pageIndex);
                LoggerManager.Logger.Info(pageValue);
                LoggerManager.Logger.Info(starttime);
                LoggerManager.Logger.Info(endtime);
                string[] fields = authCode.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string vehicleid = fields[1];
                string num = fields[0];
                DateTime StartTime = GetTime(starttime);
                DateTime EndTime = GetTime(endtime);
                MultiMessage<AppMessageVehicle> result = null;
                using (var context = new PTMSEntities())
                {
                    if (type.ToLower() == "all")
                    {
                        result = AppMessageVehicleRepository.GetMessage(context, vehicleid, num, pageIndex, pageValue, StartTime, EndTime, null);
                    }
                    else
                    {
                        result = AppMessageVehicleRepository.GetMessage(context, vehicleid, num, pageIndex, pageValue, StartTime, EndTime, Convert.ToInt32(type));
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                return new MultiMessage<AppMessageVehicle>(false, ex);
            }
        }

        [CustomerHeaderFilter]
        [HttpPost]
        public SingleMessage<bool> AddFoundRegistry(FoundRegistry entity)
        {
            try
            {
                LoggerManager.Logger.Info("AddFoundRegistry");
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    entity.ID = Guid.NewGuid().ToString();
                    entity.CreateTime = DateTime.Now.ToUniversalTime();
                    result = FoundRegistryRepository.InsertFoundRegistry(context, entity);
                }
                return result;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        [CustomerHeaderFilter]
        [HttpPost]
        public SingleMessage<bool> AddApplication(MaintainApplication entity)
        {
            try
            {
                LoggerManager.Logger.Info("AddApplication");
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    entity.ID = Guid.NewGuid().ToString();
                    entity.CreateTime = DateTime.Now.ToUniversalTime();
                    result = FoundRegistryRepository.InsertAddApplication(context, entity);
                }

                return result;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        [CustomerHeaderFilter]
        [HttpPost]
        public SingleMessage<bool> UnBind(string authCode)
        {
            try
            {
                SingleMessage<bool> result = null;
                string[] fields = authCode.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string vehicleid = fields[1];
                string num = fields[0];
                using (var context = new PTMSEntities())
                {
                    result = ChauffeurRepository.UnBind(context, num, vehicleid);
                }

                return result;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }


        [CustomerHeaderFilter]
        [HttpGet]
        public SingleMessage<AppMessageVehicle> GetMessageByID(string authCode, string id)
        {
            try
            {
                LoggerManager.Logger.Info("GetMessageByID");
                LoggerManager.Logger.Info(authCode);
                LoggerManager.Logger.Info(id);
                string code = System.Web.HttpUtility.UrlDecode(authCode);
                SingleMessage<AppMessageVehicle> result = null;
                using (var context = new PTMSEntities())
                {
                    result = AppMessageVehicleRepository.GetMessageByID(context, id);
                }
                return result;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                return new SingleMessage<AppMessageVehicle>(false, ex);
            }
        }

        [CustomerHeaderFilter]
        [HttpGet]
        public SingleMessage<FoundRegistry> GetFoundRegistryByID(string authCode, string id)
        {
            try
            {
                LoggerManager.Logger.Info("GetFoundRegistryByID");
                LoggerManager.Logger.Info(authCode);
                LoggerManager.Logger.Info(id);
                string code = System.Web.HttpUtility.UrlDecode(authCode);
                SingleMessage<FoundRegistry> result = null;
                using (var context = new PTMSEntities())
                {
                    result = FoundRegistryRepository.GetFoundRegistryByID(context, id);
                }
                return result;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                return new SingleMessage<FoundRegistry>(false, ex);
            }
        }

        [CustomerHeaderFilter]
        [HttpGet]
        public SingleMessage<MaintainApplication> GetMaintainApplicationByID(string authCode, string id)
        {
            try
            {
                LoggerManager.Logger.Info("GetMaintainApplicationByID");
                LoggerManager.Logger.Info(authCode);
                LoggerManager.Logger.Info(id);
                SingleMessage<MaintainApplication> result = null;
                using (var context = new PTMSEntities())
                {
                    result = FoundRegistryRepository.GetMaintainApplicationByID(context, id);
                }
                return result;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                return new SingleMessage<MaintainApplication>(false, ex);
            }
        }

        [CustomerHeaderFilter]
        [HttpPost]
        public SingleMessage<bool> UpdateFoundRegistry(string authCode, MaintainApplication model)
        {
            try
            {
                LoggerManager.Logger.Info("UpdateFoundRegistry");
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = FoundRegistryRepository.UpdateFoundRegistry(context, model);
                }
                return result;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }
    }
}
