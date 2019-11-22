using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Gsafety.PTMS.Manager.Contract
{
    ///<summary>
    ///心跳规则车辆
    ///</summary>
    [ServiceContract]
    public interface IHeartbeatVehicle
    {

        /// <summary>
        /// 添加心跳规则车辆
        /// </summary>
        /// <param name="model">心跳规则车辆</param>
        [OperationContract]
        SingleMessage<bool> InsertHeartbeatVehicle(List<HeartbeatVehicle> models);

        /// <summary>
        /// 删除心跳规则车辆
        /// </summary>
        /// <param name="model">心跳规则车辆</param>
        [OperationContract]
        SingleMessage<bool> DeleteHeartbeatVehicleByID(string ID);

        /// <summary>
        /// 获取心跳规则车辆列表
        /// </summary>
        /// <returns>获取心跳规则车辆</returns>
        [OperationContract]
        MultiMessage<HeartbeatVehicle> GetHeartbeatVehicleListByHeartBeatID(string clientID, string heartBeatRuleID, int pageIndex, int pageSize);

        [OperationContract]
        MultiMessage<HeartbeatVehicle> GetAllHeartbeatVehicleListByHeartBeatID(string clientID, string heartBeatRuleID);

        /// <summary>
        /// 下发车辆规则
        /// </summary>
        /// <returns>获取心跳规则车辆</returns>
        [OperationContract]
        SingleMessage<bool> DeliverHeartBeatRuleToVehicle(List<HeartbeatVehicle> vehicles);
    }
}

