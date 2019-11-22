using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Manager.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Gsafety.PTMS.Manager.Contract
{
    ///<summary>
    ///超速车辆关联表
    ///</summary>
    [ServiceContract]
    public interface IVehicleSpeed
    {

        /// <summary>
        /// 添加超速车辆关联表
        /// </summary>
        /// <param name="model">超速车辆关联表</param>
        [OperationContract]
        SingleMessage<bool> InsertVehicleSpeed(List<VehicleSpeed> models);


        /// <summary>
        /// 删除超速车辆关联表
        /// </summary>
        /// <param name="model">超速车辆关联表</param>
        [OperationContract]
        SingleMessage<bool> DeleteVehicleSpeedByID(string ID);

        /// <summary>
        /// 获取超速车辆关联表
        /// </summary>
        /// <returns>获取超速车辆关联表</returns>
        [OperationContract]
        MultiMessage<VehicleSpeed> GetAllVehicleSpeedListBySpeedID(string clientID, string speedID);
        /// <summary>
        /// 获取超速车辆关联表列表
        /// </summary>
        /// <returns>获取超速车辆关联表</returns>
        [OperationContract]
        MultiMessage<VehicleSpeed> GetVehicleSpeedListBySpeedID(string clientID, string speedID, string vehicleName, int pageIndex, int pageSize);

        [OperationContract]
        SingleMessage<bool> DeliverSpeedLimitToVehicle(List<VehicleSpeed> vehicles);

    }
}

