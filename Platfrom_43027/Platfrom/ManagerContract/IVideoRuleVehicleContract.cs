using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Gsafety.PTMS.Manager.Contract
{
    ///<summary>
    ///视频车辆关联表
    ///</summary>
    [ServiceContract]
    public interface IVideoRuleVehicle
    {

        /// <summary>
        /// 添加视频车辆关联表
        /// </summary>
        /// <param name="model">视频车辆关联表</param>
        [OperationContract]
        SingleMessage<bool> InsertVideoRuleVehicle(List<VideoRuleVehicle> models);

        /// <summary>
        /// 删除视频车辆关联表
        /// </summary>
        /// <param name="model">视频车辆关联表</param>
        [OperationContract]
        SingleMessage<bool> DeleteVideoRuleVehicleByID(string ID);

        /// <summary>
        /// 获取汇报策略车辆列表
        /// </summary>
        /// <returns>获取汇报策略车辆</returns>
        [OperationContract]
        MultiMessage<VideoRuleVehicle> GetVideoRuleVehicleListByVideoRuleID(string clientID, string locationReportID, int pageIndex, int pageSize);

        /// <summary>
        /// 获取汇报策略所有相关车辆列表
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="locationReportID"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<VideoRuleVehicle> GetAllVideoRuleVehicleListByVideoRuleID(string clientID, string locationReportID);

        /// <summary>
        /// 下发策略
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="locationReportID"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<bool> DeliverVideoRuleToVehicle(List<VideoRuleVehicle> vehicles);
    }
}

