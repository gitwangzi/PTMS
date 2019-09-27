using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
namespace Gsafety.PTMS.Manager.Contract
{
    ///<summary>
    ///汇报策略车辆
    ///</summary>
    [ServiceContract]
    public interface ILocationReportVehicle
    {

        /// <summary>
        /// 添加汇报策略车辆
        /// </summary>
        /// <param name="model">汇报策略车辆</param>
        [OperationContract]
        SingleMessage<bool> InsertLocationReportVehicle(List<LocationReportVehicle> model);

        /// <summary>
        /// 删除汇报策略车辆
        /// </summary>
        /// <param name="model">汇报策略车辆</param>
        [OperationContract]
        SingleMessage<bool> DeleteLocationReportVehicleByID(string ID);

        /// <summary>
        /// 获取汇报策略车辆列表
        /// </summary>
        /// <returns>获取汇报策略车辆</returns>
        [OperationContract]
        MultiMessage<LocationReportVehicle> GetLocationReportVehicleListByLocationReportID(string clientID, string locationReportID, int pageIndex, int pageSize);

        /// <summary>
        /// 获取汇报策略所有相关车辆列表
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="locationReportID"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<LocationReportVehicle> GetAllLocationReportVehicleListByLocationReportID(string clientID, string locationReportID);

        /// <summary>
        /// 下发策略
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="locationReportID"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<bool> DeliverLocationReportRuleToVehicle(List<LocationReportVehicle> vehicles);
    }
}

