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
    [ServiceContract]
    public interface IMonitorGroupVehicleService
    {
        #region
        ///// <summary>
        ///// Move
        ///// </summary>
        ///// <param name="Vehicle_ID"></param>
        ///// <param name="TRACED_FLAG"></param>
        ///// <param name="VEHICLE_INDEX"></param>
        ///// <param name="oldGroup_ID"></param>
        ///// <param name="newGroup_ID"></param>
        ///// <returns></returns>
        //[OperationContract]
        //SingleMessage<bool> Move(string Vehicle_ID, short? TRACED_FLAG, int? VEHICLE_INDEX, string oldGroup_ID, string newGroup_ID);

        #endregion
        /// <summary>
        /// Get All Monitor Groups Vehicle
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<MonitorGroupVehicle> GetAllMonitorGroupsVehicle(string userid);

        /// <summary>
        /// Add Monitor Groups Vehicle
        /// </summary>
        /// <param name="monitorGroup"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> AddMonitorGroupsVehicle(string Group_ID, string Vehicle_ID, short? TRACED_FLAG, int? VEHICLE_INDEX,string clientID);

        /// <summary>
        /// Remove Vehicle from  MonitorGroups 
        /// </summary>
        /// <param name="monitorGroup"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> RemoveMonitorGroupsVehicle(string monitorGroup_ID, string Vehicle_ID);

        [OperationContract]
        SingleMessage<bool> MoveMonitorGroupsVehicle(string OldGroup_ID, string NewGroup_ID, string Vehicle_ID, short? TRACED_FLAG, int? VEHICLE_INDEX);
        /// <summary>
        /// Set MonitorGroups VehicleState
        /// </summary>
        /// <param name="monitorGroup_ID"></param>
        /// <param name="Vehicle_ID"></param>
        /// <param name="IsTrace"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> SetMonitorGroupsVehicleState(string monitorGroup_ID, string Vehicle_ID, bool IsTrace);



    }
}
