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
namespace Gsafety.PTMS.Alert.Service
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

        /// <summary>
        /// Get All the Unprocessed alarm information
        /// dzl add 2014-02-19
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public MultiMessage<VehicleAlert> GetAllVehicleUnhandledAlert(DateTime? startTime, DateTime? endTime, PagingInfo pagingInfo)
        {
            try
            {
                Info("GetAllVehicleUnhandledAlert");
                Info("startTime:" + startTime.ToString() + ";" + "endTime:" + endTime.ToString() + ";" + "pagingInfo:" + pagingInfo.ToString());
                UserInfoMessageHeader userinfo = GetUserInfo();
                MultiMessage<VehicleAlert> result = new MultiMessage<VehicleAlert>();
                if (userinfo.Group == "SecurityMonitor" || userinfo.Group == "SecurityManager" || userinfo.Group == "SecurityAdmin")
                {

                    using (PTMSEntities context = new PTMSEntities())
                    {
                        result = Respository.GetUnHandledVehicleAlert(context, userinfo, startTime, endTime, pagingInfo);
                    }

                }

                Log<VehicleAlert>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<VehicleAlert>() { ExceptionMessage = ex, Result = null, IsSuccess = false };
            }

        }

        public MultiMessage<VehicleAlertEx> GetVehicleHandledAlert(string proviceCode, string cityCode, string companyCode, string vehicleId, DateTime? startTime, DateTime? endTime, int alertType, PagingInfo pagingInfo)
        {
            try
            {
                Info("GetVehicleHandledAlert");
                Info("proviceCode:" + Convert.ToString(proviceCode) + ";" + "cityCode:" + Convert.ToString(cityCode) + ";" + "companyCode:" + Convert.ToString(companyCode) + ";" + "vehicleId:" + Convert.ToString(vehicleId) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime) + ";" + "alertType:" + alertType.ToString() + ";" + "pagingInfo:" + pagingInfo.ToString());
                UserInfoMessageHeader userinfo = GetUserInfo();
                MultiMessage<VehicleAlertEx> result = new MultiMessage<VehicleAlertEx>();
                if (userinfo.Group == "SecurityMonitor" || userinfo.Group == "SecurityManager" || userinfo.Group == "SecurityAdmin")
                {

                    using (PTMSEntities context = new PTMSEntities())
                    {
                        result = Respository.GetSecurityVehicleAlertEx(context, userinfo, vehicleId, startTime, endTime, alertType, pagingInfo);
                    }

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
                        resultr = Respository.GetVehicleFenceAlert(context, userinfo, vehicleID, fenceID, alertType, startTime, endTime);
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


        public MultiMessage<VehicleAlert> GetAllVehicleUnhandledAlert(DateTime? startTime, DateTime? endTime, PagingInfo pagingInfo, string clientID)
        {
            throw new NotImplementedException();
        }
    }
}
