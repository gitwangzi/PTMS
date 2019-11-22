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
    public interface ITrafficRoute
    {

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> InsertTrafficRoute(TrafficRoute model);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> UpdateTrafficRoute(TrafficRoute model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> DeleteTrafficRouteByID(string ID);

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns>获取</returns>
        [OperationContract]
        SingleMessage<TrafficRoute> GetTrafficRoute(string ID);
     
        /// <summary>
        /// 线路是否已存在
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> IsRouteExists(string routeName, string clientID, string routeID);

        [OperationContract]
        MultiMessage<Gsafety.PTMS.Common.Data.TrafficRoute> GetTrafficRouteListByVehicleIDAndRouteName(string routeName, string vehicleID, string clientID);

        /// <summary>
        /// 废弃路线
        /// </summary>
        /// <param name="fenceID"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> ObsoleteTrafficeRoute(string routeID);

        /// <summary>
        ///路线是否下发
        /// </summary>
        /// <param name="fenceID"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> IsRouteDelivered(string routeID);

        [OperationContract]
        MultiMessage<Gsafety.PTMS.Common.Data.TrafficRoute> GetDeliveredTrafficRouteListByVehicleID(string vehicleID, string clientID);

        [OperationContract]
        SingleMessage<bool> InsertRouteQueue(List<RouteQueue> routequeues);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> DeleteRouteQueueByID(string ID);
        /// <summary>
        /// 获取线路车辆列表
        /// </summary>
        /// <returns>获取线路车辆列表</returns>
        [OperationContract]
        MultiMessage<RouteQueue> GetRouteQueueByRouteID(string clientID, string routeID, string vehicleName, int pageIndex, int pageSize);

        [OperationContract]
        MultiMessage<RouteQueue> GetAllRouteQueueByRouteID(string clientID, string routeID);

        /// <summary>
        /// 下发电子围栏
        /// </summary>
        /// <returns>下发电子围栏</returns>
        [OperationContract]
        SingleMessage<bool> DeliverRouteQueueToVehicle(List<RouteQueue> vehicles);

        [OperationContract]
        MultiMessage<Gsafety.PTMS.Common.Data.RouteQueue> GetTrafficRouteListOnVehicleByVehicleID(string vehicleID, string clientID);
    }
}

