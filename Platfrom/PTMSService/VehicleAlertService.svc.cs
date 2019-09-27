using Gsafety.PTMS.Alert.Contract;
using Gsafety.PTMS.Alert.Contract.Data;
using Gsafety.PTMS.Alert.Repository;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.Common.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Common.Data;
namespace Gs.PTMS.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "VehicleAlertService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select VehicleAlertService.svc or VehicleAlertService.svc.cs at the Solution Explorer and start debugging.
    public class VehicleAlertService : Gsafety.PTMS.BaseInfo.BaseService, IVehicleAlertService
    {
        private VehicleAlertRespository Respository = new VehicleAlertRespository();

        /// <summary>
        /// this function work for handleAlert and icon windows,when the 
        /// data change ,the icon will get more data
        /// </summary>
        /// <param name="vehicleDoorAlertId"></param>
        /// <returns></returns>
        public SingleMessage<VehicleAlertDetail> GetVehicleAlertDetail(string vehicleDoorAlertId)
        {
            try
            {
                Info("GetVehicleAlertDetail");
                Info("vehicleDoorAlertId:" + vehicleDoorAlertId.ToString());
                SingleMessage<VehicleAlertDetail> result = new SingleMessage<VehicleAlertDetail>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = Respository.GetVehicleAlertDetail(context, vehicleDoorAlertId);
                }

                Log<VehicleAlertDetail>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<VehicleAlertDetail> { ExceptionMessage = ex };
            }
        }


        public MultiMessage<VehicleAlertEx> GetVehicleHandledAlert(string proviceCode, string cityCode, string companyCode, string vehicleId, DateTime? startTime, DateTime? endTime, int alertType, PagingInfo pagingInfo)
        {
            try
            {
                Info("GetVehicleHandledAlert");
                Info("proviceCode:" + Convert.ToString(proviceCode) + ";" + "cityCode:" + Convert.ToString(cityCode) + ";" + "companyCode:" + Convert.ToString(companyCode) + ";" + "vehicleId:" + Convert.ToString(vehicleId) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime) + ";" + "alertType:" + alertType.ToString() + ";" + "pagingInfo:" + pagingInfo.ToString());
                MultiMessage<VehicleAlertEx> result = new MultiMessage<VehicleAlertEx>();

                using (PTMSEntities context = new PTMSEntities())
                {
                    result = Respository.GetSecurityVehicleAlertEx(context, vehicleId, startTime, endTime, alertType, pagingInfo);
                }

                Log<VehicleAlertEx>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<VehicleAlertEx>() { ExceptionMessage = ex, Result = null };
            }

        }

        /// <summary>
        /// add Alert Handle Msg
        /// </summary>
        /// <param name="alertTreatment"></param>
        /// <returns></returns>
        public SingleMessage<bool> AddVechileAlertTreatment(VehicleAlertTreatment alertTreatment)
        {
            try
            {
                Info("AddVechileAlertTreatment");
                Info(alertTreatment.ToString());
                SingleMessage<bool> result = new SingleMessage<bool>();
                alertTreatment.AlertTime = DateTime.UtcNow;
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = Respository.AddVechileAlertTreatment(context, alertTreatment);
                }


                Log<bool>(result);

                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        /// <summary>
        /// In and out of the electronic fence alarm
        /// </summary>
        /// <param name="vehicleID"></param>
        /// <param name="fenceID"></param>
        /// <param name="alertType"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public MultiMessage<VehicleFenceAlert> GetVehicleFenceAlert(string vehicleID, string fenceID, short alertType, DateTime? startTime, DateTime? endTime)
        {
            try
            {
                Info("GetVehicleFenceAlert");
                Info("vehicleID:" + Convert.ToString(vehicleID) + ";" + "fenceID:" + Convert.ToString(fenceID) + ";" + "alertType:" + Convert.ToString(alertType) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime));
                UserInfoMessageHeader userinfo = GetUserInfo();
                MultiMessage<VehicleFenceAlert> resultr = new MultiMessage<VehicleFenceAlert>();
                if (userinfo.Group == "SecurityManager" || userinfo.Group == "SecurityAdmin")
                {

                    using (PTMSEntities context = new PTMSEntities())
                    {
                        //resultr = Respository.GetVehicleFenceAlert(context, userinfo, vehicleID, fenceID, alertType, startTime, endTime);
                    }

                }

                Log<VehicleFenceAlert>(resultr);

                return resultr;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<VehicleFenceAlert>() { ExceptionMessage = ex, Result = null, IsSuccess = false };
            }
        }


        public MultiMessage<BusinessAlertEx> GetUnHandleAlertByClient(string clientID, List<string> orgnizations)
        {
            Info("GetUnHandleAlertByClient");
            Info("clientID:" + Convert.ToString(clientID));
            UserInfoMessageHeader userinfo = GetUserInfo();
            try
            {
                MultiMessage<BusinessAlertEx> resultr = new MultiMessage<BusinessAlertEx>();

                using (PTMSEntities context = new PTMSEntities())
                {
                    resultr = Respository.GetUnHandleAlertByClient(context, clientID, orgnizations);
                }

                Log<BusinessAlertEx>(resultr);

                return resultr;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<BusinessAlertEx>() { ExceptionMessage = ex, Result = null, IsSuccess = false };
            }
        }


        public SingleMessage<AlertHandleResult> InsertBusinessAlertHandle(BusinessAlertHandle model)
        {
            Info("InsertBusinessAlertHandle");
            Info(model.ToString());
            try
            {
                SingleMessage<AlertHandleResult> result = null;
                model.HandleTime = DateTime.UtcNow;
                using (var context = new PTMSEntities())
                {
                    result = Respository.InsertBusinessAlertHandle(context, model);
                }
                Log<AlertHandleResult>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<AlertHandleResult>(false, ex);
            }
        }

        public SingleMessage<BusinessAlertHandle> GetBusinessAlertHandleByAlertID(string alertID)
        {
            Info("GetBusinessAlertHandleByAlertID");
            Info(alertID.ToString());
            try
            {
                SingleMessage<BusinessAlertHandle> result = null;
                using (var context = new PTMSEntities())
                {
                    result = Respository.GetBusinessAlertHandle(context, alertID);
                }
                Log<BusinessAlertHandle>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<BusinessAlertHandle>(false, ex);
            }
        }


        public MultiMessage<BusinessAlertEx> GetAllBusinessAlert(string vehicleId, DateTime? startTime, DateTime? endTime, PagingInfo pagingInfo, List<string> orgnizations, int alerttype)
        {
            Info("GetAllBusinessAlert");

            try
            {
                MultiMessage<BusinessAlertEx> result = null;
                using (var context = new PTMSEntities())
                {
                    result = Respository.GetAllBusinessAlert(context, vehicleId, startTime, endTime, pagingInfo, orgnizations,  alerttype);
                }
                Log<BusinessAlertEx>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<BusinessAlertEx>(false, ex);
            }
        }

        /// <summary>
        /// 获取车辆告警的处置信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vehicleId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public SingleMessage<BusinessAlertEx> GetVehicleAlertDisposeInfo(string id, string vehicleId, string clientId)
        {
            Info("GetVehicleAlertDisposeInfo");
            Info(id.ToString());
            try
            {
                SingleMessage<BusinessAlertEx> result = null;
                using (var context = new PTMSEntities())
                {
                    result = Respository.GetVehicleAlertDisposeInfoResp(context, id, vehicleId, clientId);
                }
                Log<BusinessAlertEx>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<BusinessAlertEx>(false, ex);
            }
        }
    }
}
