using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Gsafety.PTMS.BaseInformation.Contract
{
    /// <summary>
    /// Monitor Group Contract
    /// </summary>
    [ServiceContract]
    public interface IMonitorGroupService
    {
        /// <summary>
        /// Get Monitor Groups
        /// </summary>
        /// <param name="UserID">User  ID</param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<MonitorGroup> GetMonitorGroups(string UserID);

        /// <summary>
        /// Add Monitor Group
        /// </summary>
        /// <param name="monitorGroup">monitor Group</param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> AddMonitorGroup(MonitorGroup monitorGroup);

        /// <summary>
        /// Update Vehicle
        /// </summary>
        /// <param name="monitorGroup">monitor Group</param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> UpdateVehicle(MonitorGroup monitorGroup);

        /// <summary>
        /// Batch Add Monitor Group
        /// </summary>
        /// <param name="monitorGroup"></param>
        /// <param name="adUser"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> BatchAddMonitorGroup(List<MonitorGroup> monitorGroup, string adUser);

    }
}
