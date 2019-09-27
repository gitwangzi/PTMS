using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Monitor.Contract.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
namespace Gsafety.PTMS.Monitor.Contract
{
    ///<summary>
    ///分组
    ///</summary>
    [ServiceContract]
    public interface IRunMonitorGroupService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupObj"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<bool> ChangeRunMonitorGroup(ObservableCollection<RunMonitorGroup> groupModel, string userID);

        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="model">分组</param>
        [OperationContract]
        SingleMessage<bool> DeleteRunMonitorGroupByID(string ID);

        /// <summary>
        /// 获取分组列表
        /// </summary>
        /// <returns>获取分组</returns>
        [OperationContract]
        MultiMessage<RunMonitorGroup> GetRunMonitorGroupList(int pageIndex, int pageSize, string groupId);

        [OperationContract]
        SingleMessage<bool> ChangeRunMonitorGroupVehicle(string carNo, string groupId);

        /// <summary>
        /// 删除分组车辆
        /// </summary>
        /// <param name="model">分组车辆</param>
        [OperationContract]
        SingleMessage<bool> DeleteRunMonitorGroupVehicleByID(string ID, string userID);

        /// <summary>
        /// 获取分组车辆列表
        /// </summary>
        /// <returns>获取分组车辆</returns>
        [OperationContract]
        MultiMessage<RunMonitorGroupVehicle> GetRunMonitorGroupVehicleList(int pageIndex, int pageSize, string userID);

    }
}

