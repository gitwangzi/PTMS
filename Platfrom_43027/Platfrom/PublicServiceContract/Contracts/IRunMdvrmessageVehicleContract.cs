using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ComponentModel.DataAnnotations;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;
namespace Gsafety.PTMS.PublicService.Contract
{
    ///<summary>
    ///消息车辆
    ///</summary>
    [ServiceContract]
    public interface IRunMdvrmessageVehicle
    {

        /// <summary>
        /// 添加消息车辆
        /// </summary>
        /// <param name="model">消息车辆</param>
        [OperationContract]
        SingleMessage<bool> InsertRunMdvrmessageVehicle(List<MdvrMessageVehicle> model);

        /// <summary>
        /// 删除消息车辆
        /// </summary>
        /// <param name="model">消息车辆</param>
        [OperationContract]
        SingleMessage<bool> DeleteRunMdvrmessageVehicleByID(string ID);

        /// <summary>
        /// 获取消息车辆列表
        /// </summary>
        /// <returns>获取消息车辆</returns>
        [OperationContract]
        MultiMessage<MdvrMessageVehicle> GetRunMdvrmessageVehicleList(string messageID, int pageIndex, int pageSize);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vehicles"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<bool> DeliverRunMdvrmessageToVehicle(List<MdvrMessageVehicle> vehicles);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<MdvrMessageVehicle> GetAllRunMdvrmessageVehicleListBySpeedID(string clientID, string messageId);
    }
}

