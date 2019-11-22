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

namespace Gsafety.PTMS.Alert.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DeviceAlertService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DeviceAlertService.svc or DeviceAlertService.svc.cs at the Solution Explorer and start debugging.
    public class DeviceAlertService : BaseService, IDeviceAlertService
    {
        private DeviceAlertRespository Respository = new DeviceAlertRespository();

        public SingleMessage<DeviceAlert> AddDeviceAlert(DeviceAlert alert)
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
        public SingleMessage<Boolean> ModifySecuritySuiteStatus(string mdvrCoreSN, DeviceSuiteStatus status, DeviceAlertType alertType)
        {
            try
            {
                Info("ModifySecuritySuiteStatus");
                Info("mdvrCoreSN:" + Convert.ToString(mdvrCoreSN) + ";" + "status:" + Convert.ToString(status) + ";" + "alertType:" + Convert.ToString(alertType));

                using (PTMSEntities context = new PTMSEntities())
                {
                    Respository.ModifySecuritySuiteStatus(context, mdvrCoreSN, status, alertType);
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

        public MultiMessage<DeviceAlert> GetDeviceAlertEx1(string CarNumber, string sutieId, List<decimal?> alertType, DateTime startTime, DateTime endTime, PagingInfo pagingInfo)
        {
            try
            {
                Info("GetDeviceAlertEx1");
                Info("CarNumber:" + Convert.ToString(CarNumber) + ";" + "sutieId:" + Convert.ToString(sutieId) + ";" + "alertType:" + Convert.ToString(alertType) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime) + ";" + "pagingInfo:" + Convert.ToString(pagingInfo));
                int totalCount = 0;
                var temp = new List<DeviceAlert>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    Respository.GetDeviceAlertEx1(context, CarNumber, sutieId, alertType, startTime, endTime, pagingInfo, out totalCount, GetUserInfo());
                }

                MultiMessage<DeviceAlert> result = new MultiMessage<DeviceAlert>() { Result = temp, TotalRecord = totalCount };
                Log<DeviceAlert>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<DeviceAlert>() { ExceptionMessage = ex, TotalRecord = 0 };
            }
        }
    }
}
