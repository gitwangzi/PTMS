using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;
namespace Gsafety.PTMS.Traffic.Contract
{
    ///<summary>
    ///
    ///</summary>
    [ServiceContract]
    public interface ITrafficFence
    {

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> InsertTrafficFence(TrafficFence model);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> UpdateTrafficFence(TrafficFence model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> DeleteTrafficFenceByID(string ID);

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns>获取</returns>
        [OperationContract]
        SingleMessage<TrafficFence> GetTrafficFence(string ID);

        /// <summary>
        /// 电子围栏名称是否已存在
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> IsTrafficExists(string fenceName, string clientID, string fenceID);

        [OperationContract]
        MultiMessage<TrafficFence> GetDeliveredTrafficFenceListByVehicleID(string vehicleID, string clientID);

        [OperationContract]
        MultiMessage<TrafficFence> GetTrafficFenceListOnVehicleByVehicleID(string vehicleID, string clientID);

        [OperationContract]
        MultiMessage<TrafficFence> GetTrafficFenceListByVehicleIDAndFenceName(string fenceName, string vehicleID, string clientID);

        [OperationContract]
        SingleMessage<bool> InsertFenceQueue(List<FenceQueue> fencequeues);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> DeleteFenceQueueByID(string ID);

        /// <summary>
        /// 废弃电子围栏
        /// </summary>
        /// <param name="fenceID"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> ObsoleteFence(string fenceID);

        /// <summary>
        /// 电子围栏是否下发
        /// </summary>
        /// <param name="fenceID"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> IsFenceDelivered(string fenceID);


        [OperationContract]
        MultiMessage<FenceQueue> GetAllFenceQueueListByFenceID(string clientID, string fenceID);

        [OperationContract]
        MultiMessage<FenceQueue> GetFenceQueueListByFenceID(string fenceID, string clientID, string vehicleName, int pageIndex, int pageSize);
        /// <summary>
        /// 下发电子围栏
        /// </summary>
        /// <returns>下发电子围栏</returns>
        [OperationContract]
        SingleMessage<bool> DeliverFenceQueueToVehicle(List<FenceQueue> vehicles);
    }
}

