using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ComponentModel.DataAnnotations;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Base.Contract.Data;
namespace Gsafety.PTMS.PublicService.Contract
{
    ///<summary>
    ///
    ///</summary>
    [ServiceContract]
    public interface IAppMessageVehicle
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> InsertAppMessageVehicle(List<AppMessageVehicle> model, RunAppMessage message);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> DeleteAppMessageVehicleByID(string ID);

        /// <summary>
        /// 获取消息车辆列表
        /// </summary>
        /// <returns>获取心跳规则车辆</returns>
        [OperationContract]
        MultiMessage<AppMessageVehicle> GetAppMessageVehicleListByAppID(string clientID, string appID, int pageIndex, int pageSize);

        [OperationContract]
        MultiMessage<AppMessageVehicle> GetAllAppMessageVehicleListByAppID(string clientID, string appID);

        /// <summary>
        /// 下发车辆规则
        /// </summary>
        /// <returns>获取心跳规则车辆</returns>
        [OperationContract]
        SingleMessage<bool> DeliverAppMessageToVehicle(List<AppMessageVehicle> vehicles);
    }
}

