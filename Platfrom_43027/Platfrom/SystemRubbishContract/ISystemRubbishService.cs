using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Manager.Contract.Data;

namespace Gsafety.PTMS.SystemRubbish.Contract
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ISystemRubbishService”。
    [ServiceContract]
    public interface ISystemRubbishService
    {
        /// <summary>
        /// 获取删除车辆列表
        /// </summary>
        /// <param name="SearchVehicleId"></param>
        /// <param name="SearchOwner"></param>
        /// <param name="SearchVehicleType"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<Vehicle> GetAbandonVehicleList(string clientID,string SearchVehicleId, string SearchOwner, string SearchVehicleType,  string vehicletypeid);

        /// <summary>
        /// 恢复已删除车辆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<bool> RecoverVehicle(Vehicle model);

        /// <summary>
        /// 获取删除安全套件列表
        /// </summary>
        /// <returns>获取删除安全套件</returns>
        [OperationContract]
        MultiMessage<DevSuite> GetAbandonDevSuiteList(string clientID, string suitID, string mdvrCoreSn, string mdvrSn, string mdvrSim);

        /// <summary>
        /// 恢复已删除套件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<bool> RecoverDevSuite(DevSuite model);


        /// <summary>
        /// 获取删除用户列表
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="qureyUserName"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<GUser> GetAbandonGUserList(string clientID, string qureyUserName);

        /// <summary>
        /// 恢复删除的用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<bool> RecoverGUser(GUser model);
    }
}
