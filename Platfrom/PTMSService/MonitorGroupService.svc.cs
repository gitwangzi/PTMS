using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.BaseInfo;

namespace Gs.PTMS.Service
{
    /// <summary>
    /// Monitor Group Service
    /// </summary>
    public class MonitorGroupService : BaseService, IMonitorGroupService
    {
        /// <summary>
        /// Repository
        /// </summary>
        private MonitorGroupRepository Repository = new MonitorGroupRepository();

        /// <summary>
        /// Get Monitor Groups
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public MultiMessage<MonitorGroup> GetMonitorGroups(string UserID)
        {
            try
            {
                Info("GetMonitorGroups");
                Info("UserID:" + Convert.ToString(UserID));
                var temp = Repository.GetMonitorGroups(UserID);
                MultiMessage<MonitorGroup> result = new MultiMessage<MonitorGroup>() { Result = temp };
                Log<MonitorGroup>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<MonitorGroup>() { ExceptionMessage = ex };
            }
        }

        /// <summary>
        /// Add Monitor Group
        /// </summary>
        /// <param name="monitorGroup"></param>
        /// <returns></returns>
        public SingleMessage<bool> AddMonitorGroup(MonitorGroup monitorGroup)
        {
            try
            {
                Info("AddMonitorGroup");
                Info("monitorGroup:" + Convert.ToString(monitorGroup));
                var temp = Repository.AddMonitorGroup(monitorGroup);
                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        /// <summary>
        /// Update MonitorGroup
        /// </summary>
        /// <param name="monitorGroup"></param>
        /// <returns></returns>
        public SingleMessage<bool> UpdateVehicle(MonitorGroup monitorGroup)
        {
            try
            {
                Info("UpdateVehicle");
                Info("monitorGroup:" + Convert.ToString(monitorGroup));
                var temp = Repository.UpdateVehicle(monitorGroup);
                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        /// <summary>
        /// Batch Add MonitorGroup
        /// </summary>
        /// <param name="monitorGroup"></param>
        /// <param name="adUser"></param>
        /// <returns></returns>
        public SingleMessage<bool> BatchAddMonitorGroup(List<MonitorGroup> monitorGroup, string adUser)
        {
            try
            {
                Info("BatchAddMonitorGroup");
                Info("monitorGroup:" + Convert.ToString(monitorGroup) + ";" + "adUser:" + Convert.ToString(adUser));
                var temp = Repository.BatchAddMonitorGroup(monitorGroup, adUser);

                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }
    }
}
