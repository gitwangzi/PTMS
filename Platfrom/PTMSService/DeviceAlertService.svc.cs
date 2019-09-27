using Gsafety.PTMS.Alert.Contract;
using Gsafety.PTMS.Alert.Contract.Data;
using Gsafety.PTMS.Alert.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.Common.Logging;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Common.Data;

namespace Gs.PTMS.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DeviceAlertService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DeviceAlertService.svc or DeviceAlertService.svc.cs at the Solution Explorer and start debugging.
    public class DeviceAlertService : BaseService, IDeviceAlertService
    {
        private DeviceAlertRespository Respository = new DeviceAlertRespository();
        public SingleMessage<Gsafety.PTMS.Alert.Contract.Data.DeviceAlert> AddDeviceAlert(Gsafety.PTMS.Alert.Contract.Data.DeviceAlert alert)
        {
            Info("AddDeviceAlert");
            Info(alert.ToString());

            using (PTMSEntities context = new PTMSEntities())
            {
                //return Respository.AddDeviceAlert(context, alert);
                return null;
            }

        }

        /// <summary>
        /// Modify the status of suite by MDVR ID
        /// </summary>
        /// <param name="mdvrCoreSN">MDVR ID</param>
        /// <param name="status">status of suite</param>
        /// <param name="alertType">Alert Type</param>
        public SingleMessage<Boolean> ModifySecuritySuiteStatus(string mdvrCoreSN, DeviceSuiteStatus status, Gsafety.PTMS.Base.Contract.Data.DeviceAlertType alertType)
        {
            try
            {
                Info("ModifySecuritySuiteStatus");
                Info("mdvrCoreSN:" + Convert.ToString(mdvrCoreSN) + ";" + "status:" + Convert.ToString(status) + ";" + "alertType:" + Convert.ToString(alertType));

                using (PTMSEntities context = new PTMSEntities())
                {
                    //Respository.ModifySecuritySuiteStatus(context, mdvrCoreSN, status, alertType);
                }

                SingleMessage<Boolean> result = new SingleMessage<Boolean> { Result = true, IsSuccess = true };
                Log<Boolean>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<Boolean>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public MultiMessage<DeviceAlertEx> GetDeviceAlertEx1(string CarNumber, string sutieId, List<decimal?> alertType, DateTime? startTime, DateTime? endTime, PagingInfo pagingInfo)
        {
            try
            {
                Info("GetDeviceAlertEx1");
                Info("CarNumber:" + Convert.ToString(CarNumber) + ";" + "sutieId:" + Convert.ToString(sutieId) + ";" + "alertType:" + Convert.ToString(alertType) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime) + ";" + "pagingInfo:" + Convert.ToString(pagingInfo));
                int totalCount = 0;
                List<DeviceAlertEx> temp = null;
                using (PTMSEntities context = new PTMSEntities())
                {
                   temp = Respository.GetDeviceAlertEx1(context, CarNumber, sutieId, alertType, startTime, endTime, pagingInfo, out totalCount);
                }

                MultiMessage<DeviceAlertEx> result = new MultiMessage<DeviceAlertEx>(temp, totalCount);
                Log<DeviceAlertEx>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<DeviceAlertEx>() { ExceptionMessage = ex, TotalRecord = 0 };
            }
        }

        /************************************************************************************************************************************************/

        /// <summary>
        /// 修改维修申请
        /// </summary>
        public SingleMessage<bool> UpdateDeviceAlert(Gsafety.PTMS.Alert.Contract.Data.DeviceAlert model)
        {
            Info("UpdateDeviceAlert");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = DeviceAlertRespository.UpdateDeviceAlert(context, model);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        /// <summary>
        /// 删除维修申请
        /// </summary>
        public SingleMessage<bool> DeleteDeviceAlertByID(string ID)
        {
            Info("DeleteDeviceAlertByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = DeviceAlertRespository.DeleteDeviceAlertByID(context, ID);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }


        /// <summary>
        /// 获取维修申请
        /// </summary>
        public MultiMessage<Gsafety.PTMS.Alert.Contract.Data.DeviceAlert> GetDeviceAlertList(string clientID, string vehicleID, int? alertType, DateTime? StartTime, DateTime? EndTime, List<string> stationids, int pageIndex, int pageSize)
        {
            Info("GetDeviceAlertList");
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            try
            {
                MultiMessage<Gsafety.PTMS.Alert.Contract.Data.DeviceAlert> result = null;
                using (var context = new PTMSEntities())
                {
                    result = DeviceAlertRespository.GetDeviceAlertList(context, clientID, vehicleID, alertType, StartTime, EndTime, stationids, pageIndex, pageSize);
                }
                Log<Gsafety.PTMS.Alert.Contract.Data.DeviceAlert>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Gsafety.PTMS.Alert.Contract.Data.DeviceAlert>(false, ex);
            }
        }



    }
}
